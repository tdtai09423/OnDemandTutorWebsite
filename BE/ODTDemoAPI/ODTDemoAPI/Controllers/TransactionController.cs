using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using Stripe.Checkout;
using Stripe;
using Microsoft.AspNetCore.Authorization;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(OnDemandTutorContext context, ILogger<TransactionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //sao kê
        [HttpGet("get-all-transactions/{accountId}")]
        public async Task<IActionResult> GetAllTransactions(int accountId)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
                if (account == null)
                {
                    return NotFound("Account not found");
                }

                var transactions = await _context.Transactions.Where(t => t.AccountId == account.Id).ToListAsync();

                return Ok(transactions);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromForm] int tutorId, int amount)
        {
            try
            {
                var tutor = await _context.Tutors.Include(t => t.TutorNavigation).FirstOrDefaultAsync(t => t.TutorId == tutorId);
                if(tutor == null)
                {
                    return NotFound("Tutor not found");
                }

                if(amount == 0)
                {
                    return BadRequest(new { message = "Invalid amount." });
                }

                var accountWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == tutorId);
                if (tutor.TutorNavigation.Wallet == null)
                {
                    if(accountWallet == null)
                    {
                        var wallet = new Wallet
                        {
                            WalletId = tutorId,
                            Balance = 0,
                        };
                        _context.Wallets.Add(wallet);

                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        decimal currBalance = accountWallet.Balance;
                        _context.Wallets.Remove(accountWallet);
                        await _context.SaveChangesAsync();

                        var wallet = new Wallet
                        {
                            WalletId = tutorId,
                            Balance = currBalance,
                        };
                        _context.Wallets.Add(wallet);
                        await _context.SaveChangesAsync();
                    }
                }

                var totalEarnings = await _context.LearnerOrders
                                        .Where(o => o.Curriculum!.TutorId == tutorId && o.OrderStatus == "Paid")
                                        .SumAsync(o => o.Total);

                var totalWithdrawn = await _context.Transactions
                                        .Where(t => t.AccountId == tutorId && t.TransactionType == "Withdraw")
                                        .SumAsync(t => t.Amount);

                var availableAmount = totalEarnings - totalWithdrawn;

                if(availableAmount == 0)
                {
                    return BadRequest(new { message = "Your funds has reached limit. No more withdrawal until your next booking." });
                }

                if(amount > availableAmount)
                {
                    return BadRequest(new { message = "Your withdrawal exceeds the available funds." });
                }

                var tutorWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == tutorId);
                tutorWallet!.Balance += amount;
                _context.Wallets.Update(tutorWallet);

                var transaction = new Transaction
                {
                    AccountId = tutorId,
                    Amount = amount,
                    TransactionDate = DateTime.Now,
                    TransactionType = "Withdraw",
                };

                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return Ok(new { balance = tutorWallet.Balance, message1 = "Withdrawal successfull!", message2 = totalEarnings == totalWithdrawn ? "Your funds has reached limit. No more withdrawal until your next booking." : null });
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
