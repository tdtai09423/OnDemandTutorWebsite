using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using Stripe;
using Stripe.Checkout;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public MembershipController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpPost("upgrade-membership")]
        [Authorize(Roles = "LEARNER")]
        public async Task<IActionResult> UpgradeMembership([FromBody] int learnerId, [FromQuery] string membershipLevel)
        {
            try
            {
                var learner = await _context.Learners
                                    .Include(l => l.Membership)
                                    .Include(l => l.LearnerNavigation)
                                    .ThenInclude(a => a.Wallet)
                                    .FirstOrDefaultAsync(l => l.LearnerId == learnerId);
                if(learner == null)
                {
                    return NotFound("Not found learner");
                }

                decimal cost = 0;

                if(learner.Membership == null)
                {
                    cost = membershipLevel == "SILVER" ? 300 : 700;
                }
                else if(learner.Membership.MembershipLevel == "SILVER" && membershipLevel == "PREMIUM")
                {
                    var date = (DateTime)learner.MembershipCreatedDate!;
                    DateTime membershipEndDate = date.AddDays(learner.Membership.DurationInDays);
                    var totalDays = membershipEndDate.Day - DateTime.Now.Day;
                    cost = totalDays > 15 ? 300 : 500;
                }
                else
                {
                    return BadRequest("Your membership level has maximum authorities with a user.");
                }

                var balance = learner.LearnerNavigation.Wallet!.Balance;

                if (balance < cost || learner.LearnerNavigation.Wallet == null)
                {
                    var session = await CreateStripeSession(cost - balance, learner.LearnerId);
                    return Ok(new { message = "Hết tiền rồi má ơi, nạp vô rồi hẳn đặt. Tui để link ở dưới cho má nạp nè mệt ghê. Trông chán thiệt sự!", url = session.Url });
                }

                learner.LearnerNavigation.Wallet.Balance -= cost;
                _context.Wallets.Update(learner.LearnerNavigation.Wallet);
                await _context.SaveChangesAsync();

                var transaction = new Transaction
                {
                    AccountId = learnerId,
                    Amount = cost,
                    TransactionDate = DateTime.Now,
                    TransactionType = $"Membership {membershipLevel}"
                };

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                if (learner.Membership == null)
                {
                    var membership = await _context.Memberships.FirstOrDefaultAsync(m => m.MembershipLevel == membershipLevel);
                    learner.MembershipId = membership!.MembershipId;
                    learner.Membership = membership;

                    _context.Learners.Update(learner);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = $"You have been upgraded to the membership {membershipLevel}", Learner = learner }) ;
                }
                else
                {
                    if(learner.Membership.MembershipLevel == "SILVER")
                    {
                        var membership = await _context.Memberships.FirstOrDefaultAsync(m => m.MembershipLevel == "PRENIUM");
                        learner.MembershipId = membership!.MembershipId;
                        learner.Membership = membership;

                        _context.Learners.Update(learner);
                        await _context.SaveChangesAsync();

                        return Ok("Membership upgraded to PRENIUM");
                    }
                    else
                    {
                        return BadRequest("Your membership level has maximum authorities with a user.");
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task<Session> CreateStripeSession(decimal amount, int learnerId)
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(amount * 100),
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = "Upgrade membership",
                            },
                        },
                        Quantity = 1,
                    },
                },
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Description = "Upgrade membership for learner.",
                },
                Mode = "payment",
                SuccessUrl = $"https://localhost:7010/api/LearnerOrder/payment-success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = "https://localhost:7010/api/LearnerOrder/payment-failed",
                UiMode = "hosted",
                ClientReferenceId = learnerId.ToString(),
            };

            var service = new SessionService();
            Session session;
            try
            {
                session = await service.CreateAsync(options);
            }
            catch (StripeException)
            {
                throw;
            }

            Response.Headers.Append("Location", session.Url);

            return session;
        }
    }
}
