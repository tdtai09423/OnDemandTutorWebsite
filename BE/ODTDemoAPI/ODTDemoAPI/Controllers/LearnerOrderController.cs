using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ODTDemoAPI.Entities;
using ODTDemoAPI.OperationModel;
using ODTDemoAPI.Services;
using Stripe;
using Stripe.Checkout;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LearnerOrderController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;
        private readonly ILogger<LearnerOrderController> _logger;
        private readonly string _stripeSK;
        private readonly IMemoryCache _memoryCache;

        public LearnerOrderController(OnDemandTutorContext context, IOptions<StripeSettings> stripeSettings, ILogger<LearnerOrderController> logger, IMemoryCache memoryCache)
        {
            _context = context;
            _stripeSK = stripeSettings.Value.SecretKey;
            StripeConfiguration.ApiKey = _stripeSK;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpPost("short-term-booking")]
        public async Task<IActionResult> CreateShortTermBooking([FromBody] ShortTermBookingRequest request)
        {
            //Khai báo biến cho việc lưu trữ thời gian trong memory cache
            DateTime startTime = request.startTime;
            int duration = request.Duration;

            //lưu vào memory cache + nếu tutor không chấp nhận trong vòng 48 giờ => auto reject => refund
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_startTime_ST", startTime, TimeSpan.FromHours(48));
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_duration_ST", duration, TimeSpan.FromHours(48));
            
            return await CreateBooking(request);
        }

        [HttpPost("long-term-booking")]
        public async Task<IActionResult> CreateLongTermBooking([FromBody] LongTermBookingRequest request)
        {
            //Khai báo biến cho việc lưu trữ thời gian trong memory cache
            TimeSpan startTime = request.startTime;
            DayOfWeek day1 = request.Day1;
            DayOfWeek day2 = request.Day2;
            int duration = request.Duration;

            // lưu vào memory cache + nếu tutor không chấp nhận trong vòng 48 giờ => auto reject => refund
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_startTime_LT", startTime, TimeSpan.FromHours(48));
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_day1_LT", day1, TimeSpan.FromHours(48));
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_day2_LT", day2, TimeSpan.FromHours(48));
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_duration_LT", duration, TimeSpan.FromHours(48));

            return await CreateBooking(request);
        }

        private async Task<IActionResult> CreateBooking([FromForm] BookingRequest request)
        {
            try
            {
                var tutor = await _context.Tutors.FindAsync(request.TutorId);
                if (tutor == null)
                {
                    return NotFound("Not found tutor");
                }

                var learner = await _context.Learners.FindAsync(request.LearnerId);
                if (learner == null)
                {
                    return NotFound("Not found learner");
                }

                List<Curriculum> curricula; 

                if(request is ShortTermBookingRequest)
                {
                    curricula = await _context.Curricula
                    .Include(c => c.Sections)
                    .Where(c => c.CurriculumType == ShortTermBookingRequest.CurriculumType && c.TutorId == request.TutorId)
                    .ToListAsync();
                }
                else
                {
                    curricula = await _context.Curricula
                    .Include(c => c.Sections)
                    .Where(c => c.CurriculumType == LongTermBookingRequest.CurriculumType && c.TutorId == request.TutorId)
                    .ToListAsync();
                }

                if (curricula.Count == 0 || curricula == null)
                {
                    return NotFound("This tutor does not have this kind of curriculum.");
                }

                var curriculum = curricula.FirstOrDefault(c => c.CurriculumDescription == request.CurriculumDescription);

                if (curriculum == null)
                {
                    return NotFound("Curriculum not found");
                }

                LearnerOrder order;

                if(request is ShortTermBookingRequest)
                {
                    order = new LearnerOrder
                    {
                        OrderDate = DateTime.Now,
                        OrderStatus = "Pending",
                        Total = curriculum.PricePerSection,
                        CurriculumId = curriculum.CurriculumId,
                        LearnerId = request.LearnerId,
                    };
                }
                else
                {
                    var total = curriculum.PricePerSection * curriculum.TotalSlot;
                    order = new LearnerOrder
                    {
                        OrderDate = DateTime.Now,
                        OrderStatus = "Pending",
                        Total = total,
                        CurriculumId = curriculum.CurriculumId,
                        LearnerId = request.LearnerId,
                    };
                }

                _context.LearnerOrders.Add(order);
                await _context.SaveChangesAsync();

                return Ok(new { Order = order });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("checking-out")]
        public async Task<IActionResult> CheckOut([FromBody] LearnerOrder order)
        {
            try
            {
                var learner = await _context.Accounts
                    .Include(a => a.Wallet)
                    .FirstOrDefaultAsync(a => a.Id == order.LearnerId);

                if (learner == null)
                {
                    return NotFound("Learner not found");
                }

                var wallet = learner.Wallet;
                if (wallet != null)
                {
                    wallet = new Wallet
                    {
                        AccountId = learner.Id,
                        Balance = 0,
                    };
                    _context.Wallets.Add(wallet);
                    await _context.SaveChangesAsync();
                }

                decimal total = order.Total;

                var balance = learner.Wallet!.Balance;

                if (balance < total)
                {
                    var session = await CreateStripeSession(total - balance, learner.Id);
                    return Ok(new { url = session.Url });
                }

                balance -= total;
                learner.Wallet.Balance = balance;
                _context.Wallets.Update(learner.Wallet);
                await _context.SaveChangesAsync();

                order.OrderStatus = "Paid";
                _context.LearnerOrders.Update(order);
                await _context.SaveChangesAsync();

                NotifyTutorAboutBooking(order.Curriculum!.TutorId, order);

                NotifyLearnerAboutBooking(order.LearnerId, order);

                return Ok(new { Order = order });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("payment-success")]
        public async Task<IActionResult> PaymentSuccess([FromQuery(Name = "session_id")] string sessionId)
        {
            try
            {
                _logger.LogInformation($"Session ID: {sessionId}");
                if (string.IsNullOrEmpty(sessionId))
                {
                    _logger.LogError("Session ID is null or empty");
                    return BadRequest(new { error = "SessionId is required." });
                }
                var service = new SessionService();
                _logger.LogInformation("Fetching session from stripe...");
                Session session = await service.GetAsync(sessionId);

                if (session == null)
                {
                    _logger.LogError("Session is null from Stripe service");
                    return BadRequest(new { error = "Invalid session." });
                }

                _logger.LogInformation($"Session fetched: {session.Id}");

                if (!int.TryParse(session.ClientReferenceId, out int learnerId))
                {
                    _logger.LogError($"Invalid ClientReferenceId: {session.ClientReferenceId}");
                    return BadRequest(new { error = "Invalid learner ID in session." });
                }

                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var learner = await _context.Accounts.Include(a => a.Wallet).FirstOrDefaultAsync(a => a.Id == learnerId);
                        if (learner == null)
                        {
                            _logger.LogError("Learner not found");
                            return BadRequest("Learner Not Found");
                        }

                        //if(learner.Wallet == null)
                        //{
                        //    learner.Wallet = new Wallet
                        //    {
                        //        AccountId = learnerId,
                        //        Balance = 0,
                        //        Account = learner
                        //    };
                        //    _context.Accounts.Update(learner);
                        //    await _context.SaveChangesAsync();
                        //}

                        var wallet = learner.Wallet ?? new Wallet { AccountId = learnerId, Balance = 0 };
                        if (wallet.AccountId == 0)
                        {
                            _context.Wallets.Add(wallet);
                            await _context.SaveChangesAsync();
                        }

                        var amount = session.AmountTotal!.GetValueOrDefault() / 100m;

                        wallet.Balance += amount;
                        _context.Wallets.Update(wallet);
                        await _context.SaveChangesAsync();

                        var transactionRecord = new Transaction
                        {
                            AccountId = learnerId,
                            Amount = amount,
                            TransactionDate = DateTime.Now,
                            TransactionType = "Top-up",
                        };

                        _context.Transactions.Add(transactionRecord);
                        await _context.SaveChangesAsync();

                        await transaction.CommitAsync();

                        return Ok(new { Message = "Payment succeeded, wallet topped-up", SessionId = sessionId });

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error occurred during transaction processing");
                        await transaction.RollbackAsync();
                        return BadRequest(new { error = "An error occurred while processing payment success." });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error");
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("payment-failed")]
        public IActionResult PaymentFailed()
        {
            return BadRequest("Payment failed!");
        }

        [HttpPost("get-meet-url")]
        public async Task<IActionResult> EnterMeetUrl(string meetUrl)
        {
            //kiểm tra url có hợp lệ hay không
            if (IsValidUrl(meetUrl))
            {
                return BadRequest("Meet url is invalid!");
            }

            //không để cho các learner cũ vào meeting khi không order
            var existingCurri = await _context.Curricula.AnyAsync(c => c.Sections.FirstOrDefault(s => s.MeetUrl == meetUrl)!.MeetUrl == meetUrl);

            if(existingCurri)
            {
                return BadRequest("Meet URL already exists. Please use another URL.");
            }

            HttpContext.Session.SetString("MeetUrl", meetUrl);
            return Ok("Saved meeting url.");
        }

        private static bool IsValidUrl(string url)
        {
            // Kiểm tra nếu chuỗi rỗng hoặc null
            if (string.IsNullOrWhiteSpace(url))
            {
                return false;
            }

            // Thử tạo một đối tượng Uri từ chuỗi URL
            Uri uriResult;
            bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult!)
                          && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }


        [HttpPost("accept-booking")]
        public async Task<IActionResult> AcceptBooking([FromBody] TutorResponse response)
        {
            try
            {
                var order = await _context.LearnerOrders
                    .Include(o => o.Curriculum)
                    .FirstOrDefaultAsync(o => o.OrderId == response.OrderId && o.Curriculum!.TutorId == response.TutorId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                if (order.OrderStatus != "Pending")
                {
                    return BadRequest("Order status is not at pending status");
                }

                order.OrderStatus = "Accepted";
                _context.LearnerOrders.Update(order);
                await _context.SaveChangesAsync();

                NotifyLearnerAboutBookingStatus(order.LearnerId, order, "accepted");

                //TODO: tạo section cho booking
                var curriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.CurriculumId == order.CurriculumId);
                if(curriculum!.CurriculumType == "ShortTerm")
                {
                    var sectionStart = _memoryCache.Get<DateTime>($"{order.LearnerId}request{response.TutorId}_startTime_ST");
                    var sectionEnd = sectionStart.AddMinutes(_memoryCache.Get<int>($"{order.LearnerId}request{response.TutorId}_duration_ST"));
                    var section = new Section
                    {
                        SectionStart = sectionStart,
                        SectionEnd = sectionEnd,
                        SectionStatus = "", //scheduled, completed, pending
                        CurriculumId = curriculum.CurriculumId,
                        MeetUrl = HttpContext.Session.GetString("MeetUrl"),
                    };
                    _context.Sections.Add(section);
                    await _context.SaveChangesAsync();
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_startTime_ST");
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_duration_ST");
                }
                if(curriculum!.CurriculumType == "LongTerm")
                {
                    var startTime = _memoryCache.Get<TimeSpan>($"{order.LearnerId}request{response.TutorId}_startTime_LT");
                    var day1 = _memoryCache.Get<DayOfWeek>($"{order.LearnerId}request{response.TutorId}_day1_LT");
                    var day2 = _memoryCache.Get<DayOfWeek>($"{order.LearnerId}request{response.TutorId}_day2_LT");
                    var duration = _memoryCache.Get<int>($"{order.LearnerId}request{response.TutorId}_duration_LT");
                    List<Section> sections = new();
                    for(int i = 1; i <= curriculum.TotalSlot; i++)
                    {
                        var sectionDay1 = new Section
                        {
                            
                        };
                        sections.Add(sectionDay1);

                        var sectionDay2 = new Section
                        {

                        };
                        sections.Add(sectionDay2);
                    }

                    _context.Sections.AddRange(sections);
                    await _context.SaveChangesAsync();
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_startTime_LT");
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_day1_LT");
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_day2_LT");
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_duration_LT");
                }

                return Ok("Booking accepted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject-booking")]
        public async Task<IActionResult> RejectBooking([FromBody] TutorResponse response)
        {
            try
            {
                var order = await _context.LearnerOrders
                    .Include(o => o.Curriculum)
                    .FirstOrDefaultAsync(o => o.OrderId == response.OrderId && o.Curriculum!.TutorId == response.TutorId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                if (order.OrderStatus != "Pending")
                {
                    return BadRequest("Order status is not at pending status");
                }

                order.OrderStatus = "Rejected";
                _context.LearnerOrders.Update(order);
                await _context.SaveChangesAsync();

                NotifyLearnerAboutBookingStatus(order.LearnerId, order, "rejected");

                await RefundPayment(order.Total, (int)order.LearnerId!);

                return Ok("Booking rejected and payment refunded.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private async Task RefundPayment(decimal amount, int learnerId)
        {
            try
            {
                var learner = await _context.Accounts
                    .Include(a => a.Wallet)
                    .FirstOrDefaultAsync(a => a.Id == learnerId);

                if (learner != null)
                {
                    learner.Wallet!.Balance += amount;
                    _context.Wallets.Update(learner.Wallet);

                    var transaction = new Transaction
                    {
                        AccountId = learner.Id,
                        TransactionDate = DateTime.Now,
                        TransactionType = "Refund",
                        Amount = amount,
                    };
                    _context.Transactions.Add(transaction);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error refunding payment: {ex.Message}");
            }
        }

        private void NotifyLearnerAboutBookingStatus(int? learnerId, LearnerOrder order, string status)
        {
            var notification = new UserNotification
            {
                Content = $"Your booking request for curriculum {order.Curriculum!.CurriculumType} has been {status} by the tutor.",
                NotificateDay = DateTime.Now,
                AccountId = (int)learnerId!
            };

            var learner = _context.Learners.Find(learnerId);
            if (learner != null)
            {
                string subject = "Booking Status Update";
                string message = $"Dear {learner.LearnerNavigation.FirstName}, \n\nYour booking request for curriculum {order.Curriculum!.CurriculumType} has been {status} by the tutor. Please login to your dashboard for more details.";

                var emailService = new EmailService();
                emailService.SendMailAsync(learner.LearnerEmail, subject, message);
            }

            _context.UserNotifications.Add(notification);
            _context.SaveChanges();
        }

        private void NotifyTutorAboutBooking(int? tutorId, LearnerOrder order)
        {
            var notification = new UserNotification
            {
                Content = $"You have received a new booking request for curriculum {order.Curriculum!.CurriculumType}.",
                NotificateDay = DateTime.Now,
                AccountId = (int)tutorId!
            };

            var tutor = _context.Tutors.Find(tutorId);
            if (tutor != null)
            {
                string subject = "New Booking Request";
                string message = $"Dear {tutor.TutorNavigation.FirstName}, \n\nYou have received a new booking request for curriculum {order.Curriculum!.CurriculumType}. Please login to your dhashboard for more details.";

                var emailService = new EmailService();
                emailService.SendMailAsync(tutor.TutorEmail, subject, message);
            }

            _context.UserNotifications.Add(notification);
            _context.SaveChanges();
        }

        private void NotifyLearnerAboutBooking(int? learnerId, LearnerOrder order)
        {
            var notification = new UserNotification
            {
                Content = $"Your booking request for curriculum {order.Curriculum!.CurriculumType} has been sent to the tutor.",
                NotificateDay = DateTime.Now,
                AccountId = (int)learnerId!
            };

            var learner = _context.Learners.Find(learnerId);
            if (learner != null)
            {
                string subject = "Booking Sent";
                string message = $"Dear {learner.LearnerNavigation.FirstName}, \n\nYour booking request for curriculum {order.Curriculum!.CurriculumType} has been sent to the tutor.";

                var emailService = new EmailService();
                emailService.SendMailAsync(learner.LearnerEmail, subject, message);
            }

            _context.UserNotifications.Add(notification);
            _context.SaveChanges();
        }

        [HttpPost("top-up-wallet")]
        public async Task<IActionResult> TopUpWallet(decimal amount, int learnerId)
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
                                Name = "Top-up Wallet",
                            },
                        },
                        Quantity = 1,
                    },
                },
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Description = "Top-up wallet to complete booking payment",
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
                Response.Headers.Append("Location", session.Url);
                return Ok(new { SessionId = session.Id });
            }
            catch (StripeException e)
            {
                // Xử lý lỗi từ Stripe
                return BadRequest(new { error = e.StripeError.Message });
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
                                Name = "Top-up Wallet",
                            },
                        },
                        Quantity = 1,
                    },
                },
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Description = "Top-up wallet to complete booking payment",
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
