using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;
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
        private readonly VNPayService _vNPayService;
        private readonly IMemoryCache _memoryCache;
        private readonly IEmailService _emailService;

        public LearnerOrderController(OnDemandTutorContext context, IOptions<StripeSettings> stripeSettings, ILogger<LearnerOrderController> logger, IMemoryCache memoryCache, IEmailService emailService, VNPayService vNPayService)
        {
            _context = context;
            _stripeSK = stripeSettings.Value.SecretKey;
            StripeConfiguration.ApiKey = _stripeSK;
            _logger = logger;
            _memoryCache = memoryCache;
            _emailService = emailService;
            _vNPayService = vNPayService;
        }

        [HttpPost("short-term-booking")]
        public async Task<IActionResult> CreateShortTermBooking([FromBody] ShortTermBookingRequest request)
        {
            var learner = await _context.Learners.Include(l => l.Membership).FirstOrDefaultAsync(l => l.LearnerId == request.LearnerId);

            if (learner == null)
            {
                return NotFound("Not found learner");
            }

            learner.CheckAndUpdateMembership();
            _context.Learners.Update(learner);
            await _context.SaveChangesAsync();

            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var orders = await _context.LearnerOrders
                                            .Where(o => o.LearnerId == request.LearnerId
                                                        && o.OrderDate.Month == currentMonth
                                                        && o.OrderDate.Year == currentYear)
                                            .ToListAsync();
            int maxOrdersPerMonth = 5;

            if (learner.Membership != null)
            {
                if (learner.Membership.MembershipLevel == "SILVER")
                {
                    maxOrdersPerMonth = 15;
                }
                else
                {
                    maxOrdersPerMonth = int.MaxValue;
                }
            }

            if (orders.Count >= maxOrdersPerMonth)
            {
                return BadRequest("You have reached the limit booking in this month. Upgrade your membership for more benefits.");
            }

            var learnDate = request.startTime.Date;

            var order = await _context.LearnerOrders.OrderByDescending(o => o.OrderId).FirstOrDefaultAsync();

            //Khai báo biến cho việc lưu trữ thời gian trong database
            DateTime startTime = request.startTime;
            int duration = request.Duration;

            //lưu vào database
            var stbcondition = new STBCondition
            {
                OrderId = order!.OrderId + 1,
                StartTime = startTime,
                Duration = duration,
            };
            _context.STBConditions.Add(stbcondition);
            await _context.SaveChangesAsync();

            return await CreateBooking(request);
        }

        [HttpPost("long-term-booking")]
        public async Task<IActionResult> CreateLongTermBooking([FromBody] LongTermBookingRequest request)
        {
            var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == request.LearnerId);

            if (learner == null)
            {
                return NotFound("Not found learner");
            }

            learner.CheckAndUpdateMembership();
            _context.Learners.Update(learner);
            await _context.SaveChangesAsync();

            var currentYear = DateTime.Now.Year;
            var currentMonth = DateTime.Now.Month;
            var orders = await _context.LearnerOrders
                                            .Where(o => o.LearnerId == request.LearnerId
                                                        && o.OrderDate.Month == currentMonth
                                                        && o.OrderDate.Year == currentYear)
                                            .ToListAsync();
            int maxOrdersPerMonth = 5;

            if (learner.Membership != null)
            {
                if (learner.Membership.MembershipLevel == "SILVER")
                {
                    maxOrdersPerMonth = 15;
                }
                else
                {
                    maxOrdersPerMonth = int.MaxValue;
                }
            }

            if (orders.Count >= maxOrdersPerMonth)
            {
                return BadRequest("You have reached the limit booking in this month. Upgrade your membership for more benefits.");
            }
            //Khai báo biến cho việc lưu trữ thời gian trong memory cache
            TimeSpan startTime = request.startTime;
            DayOfWeek day1 = request.Day1;
            DayOfWeek day2 = request.Day2;
            int duration = request.Duration;
            int weekNumber = request.WeekNumber;
            int year = request.Year;

            // lưu vào memory cache + nếu tutor không chấp nhận trong vòng 48 giờ => auto reject => refund
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_startTime_LT", startTime, TimeSpan.FromHours(48));
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_day1_LT", day1, TimeSpan.FromHours(48));
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_day2_LT", day2, TimeSpan.FromHours(48));
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_duration_LT", duration, TimeSpan.FromHours(48));
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_weekNumber_LT", weekNumber, TimeSpan.FromHours(48));
            _memoryCache.Set($"{request.LearnerId}request{request.TutorId}_year_LT", year, TimeSpan.FromHours(48));

            //return await CreateBooking(request);
            return Ok("Tính năng đang được phát triển. Nói chung là do team chưa thống nhất được flow của cái này.");
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

                if (request is ShortTermBookingRequest)
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

                if (request is ShortTermBookingRequest)
                {
                    order = new LearnerOrder
                    {
                        OrderDate = DateTime.Now,
                        OrderStatus = "Pending",
                        Total = curriculum.PricePerSection,
                        CurriculumId = curriculum.CurriculumId,
                        LearnerId = request.LearnerId,
                        IsCompleted = false,
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
                        IsCompleted = false,
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
        public async Task<IActionResult> CheckOut([FromForm] int orderId)
        {
            try
            {
                var order = await _context.LearnerOrders
                    .Include(o => o.Curriculum!)
                    .ThenInclude(c => c.Sections)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order == null)
                {
                    return NotFound("Order not found!");
                }

                var learner = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Id == order.LearnerId);

                if (learner == null)
                {
                    return NotFound("Learner not found!");
                }

                if (order.OrderStatus != "Pending")
                {
                    return BadRequest(new { message = "Order status is not at pending." });
                }

                var accountWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == learner.Id);
                if (learner.Wallet == null)
                {
                    if (accountWallet == null)
                    {
                        var wallet = new Wallet
                        {
                            WalletId = learner.Id,
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
                            WalletId = learner.Id,
                            Balance = currBalance,
                        };
                        _context.Wallets.Add(wallet);
                        await _context.SaveChangesAsync();
                    }

                    learner.Wallet = accountWallet;
                }

                var learnerWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == learner.Id);

                decimal total = order.Total;

                var balance = learnerWallet!.Balance;

                if (balance < total)
                {
                    var session = await CreateStripeSession(total - balance, learner.Id);
                    return Ok(new { message = "Hết tiền rồi má ơi, nạp vô rồi hẳn đặt. Tui để link ở dưới cho má nạp nè mệt ghê. Trông chán thiệt sự!", url = session.Url });
                }

                balance -= total;
                learnerWallet!.Balance = balance;
                _context.Wallets.Update(accountWallet!);

                var transaction = new Transaction
                {
                    AccountId = learner.Id,
                    Amount = total,
                    TransactionDate = DateTime.Now,
                    TransactionType = "Paid for order",
                };

                _context.Transactions.Add(transaction);

                order.OrderStatus = "Paid";
                _context.Database.ExecuteSql($"UPDATE dbo.LearnerOrder SET OrderStatus = 'Paid' WHERE OrderId = {order.OrderId}");
                var saved = false;
                while (!saved)
                {
                    try
                    {
                        // Attempt to save changes to the database
                        await _context.SaveChangesAsync();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        foreach (var entry in ex.Entries)
                        {
                            if (entry.Entity is LearnerOrder)
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in proposedValues.Properties)
                                {
                                    var proposedValue = proposedValues[property];
                                    var databaseValue = databaseValues![property];

                                    // TODO: decide which value should be written to database
                                    // proposedValues[property] = <value to be saved>;
                                }

                                // Refresh original values to bypass next concurrency check
                                entry.OriginalValues.SetValues(databaseValues!);
                            }
                            else
                            {
                                throw new NotSupportedException(
                                    "Don't know how to handle concurrency conflicts for "
                                    + entry.Metadata.Name);
                            }
                        }
                    }
                }

                await NotifyTutorAboutBooking(order.Curriculum!.TutorId!, order.OrderId);

                await NotifyLearnerAboutBooking(order.LearnerId, order.OrderId);

                return Ok(new { Order = order });

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("checking-out-vnpay")]
        public async Task<IActionResult> CheckOutVNPay([FromForm] int orderId)
        {
            try
            {
                var order = await _context.LearnerOrders
                    .Include(o => o.Curriculum!)
                    .ThenInclude(c => c.Sections)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order == null)
                {
                    return NotFound("Order not found!");
                }

                var learner = await _context.Accounts
                    .FirstOrDefaultAsync(a => a.Id == order.LearnerId);

                if (learner == null)
                {
                    return NotFound("Learner not found!");
                }

                if (order.OrderStatus != "Pending")
                {
                    return BadRequest(new { message = "Order status is not at pending." });
                }

                var learnerWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == learner.Id);
                if (learnerWallet == null)
                {
                    return BadRequest(new { message = "Learner wallet not found." });
                }

                decimal total = order.Total;
                decimal balance = learnerWallet.Balance;

                if (balance < total)
                {
                    var paymentUrl = _vNPayService.CreatePaymentUrl(orderId, total - balance, "Thanh toán đơn hàng");
                    return Ok(new { message = "Hết tiền rồi má ơi, nạp vô rồi hẳn đặt. Tui để link ở dưới cho má nạp nè mệt ghê. Trông chán thiệt sự!", url = paymentUrl });
                }

                balance -= total;
                learnerWallet.Balance = balance;
                _context.Wallets.Update(learnerWallet);

                var transaction = new Transaction
                {
                    AccountId = learner.Id,
                    Amount = total,
                    TransactionDate = DateTime.Now,
                    TransactionType = "Paid for order",
                };

                _context.Transactions.Add(transaction);

                order.OrderStatus = "Paid";
                _context.LearnerOrders.Update(order);

                var saved = false;
                while (!saved)
                {
                    try
                    {
                        await _context.SaveChangesAsync();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        foreach (var entry in ex.Entries)
                        {
                            if (entry.Entity is LearnerOrder)
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in proposedValues.Properties)
                                {
                                    var proposedValue = proposedValues[property];
                                    var databaseValue = databaseValues![property];
                                }

                                entry.OriginalValues.SetValues(databaseValues!);
                            }
                            else
                            {
                                throw new NotSupportedException(
                                    "Don't know how to handle concurrency conflicts for "
                                    + entry.Metadata.Name);
                            }
                        }
                    }
                }

                await NotifyTutorAboutBooking(order.Curriculum!.TutorId!, order.OrderId);
                await NotifyLearnerAboutBooking(order.LearnerId, order.OrderId);

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
                        var learner = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == learnerId);
                        if (learner == null)
                        {
                            _logger.LogError("Learner not found");
                            return BadRequest("Learner Not Found");
                        }

                        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == learner.Id);
                        if (wallet == null)
                        {
                            wallet = new Wallet
                            {
                                WalletId = learner.Id,
                                Balance = 0,
                            };
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

                        //return Ok(new { Message = "Payment succeeded, wallet topped-up", SessionId = sessionId });
                        return Redirect("https://localhost:3000");

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

        [HttpGet("get-orders-list/{tutorId}")]
        [Authorize(Roles = "TUTOR")]
        public async Task<IActionResult> GetOrdersListForTutor([FromRoute] int tutorId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                IQueryable<LearnerOrder> query = _context.LearnerOrders.Include(o => o.Curriculum).Where(o => o.Curriculum!.TutorId == tutorId);
                query = query.OrderByDescending(o => o.OrderDate);
                var totalCount = await query.CountAsync();
                var orders = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                if (orders == null || orders.Count == 0)
                {
                    return NotFound("This tutor has no orders yet.");
                }

                var response = new PaginatedResponse<LearnerOrder>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = orders,
                };
                var numOfPages = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    numOfPages++;
                }
                return Ok(new { Response = response, NumOfPages = numOfPages });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("get-meet-url")]
        [Authorize(Roles = "TUTOR")]
        public async Task<IActionResult> EnterMeetUrl(string meetUrl)
        {
            //kiểm tra url có hợp lệ hay không
            if (!IsValidUrl(meetUrl))
            {
                return BadRequest("Meet url is invalid!");
            }

            //không để cho các learner cũ vào meeting khi không order
            var existingCurri = await _context.Curricula.Include(c => c.Sections).AnyAsync(c => c.Sections.FirstOrDefault(s => s.MeetUrl == meetUrl)!.MeetUrl == meetUrl);

            if (existingCurri)
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

        //update booking cho learner
        [HttpPut("update-booking/{orderId}")]
        [Authorize(Roles = "LEARNER")]
        public async Task<IActionResult> UpdateShortTermBooking([FromRoute] int orderId, [FromForm] DateTime? startTime)
        {
            try
            {
                var order = await _context.LearnerOrders
                                            .Include(o => o.Curriculum)
                                            .ThenInclude(c => c!.Sections)
                                            .FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order == null)
                {
                    return NotFound("Order not found");
                }

                if (order.OrderStatus == "Accepted")
                {
                    return BadRequest("Tutor has accepted this booing request. Cannot update. Please contact tutor to cancel and try updating again.");
                }

                if (order.OrderStatus == "Rejected")
                {
                    return BadRequest("This booking request has been rejected. Cannot update.");
                }

                var curriculum = order.Curriculum;
                if (curriculum == null || curriculum.CurriculumType != "ShortTerm")
                {
                    return BadRequest("This order does not belong to a short term curriculum.");
                }

                var stbCondition = await _context.STBConditions.FirstOrDefaultAsync(c => c.OrderId == order.OrderId);
                if (stbCondition == null)
                {
                    return NotFound("Not found short term booking condition.");
                }

                if (startTime.HasValue)
                {
                    stbCondition.StartTime = startTime.Value;
                }

                _context.STBConditions.Update(stbCondition);
                await _context.SaveChangesAsync();

                await NotifyLearnerAboutBookingStatus(order.LearnerId, order.OrderId, "updated");

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //cancel này mới là của admin nha!!!
        [HttpPost("force-to-cancel-booking")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> ForceToCancelBooking([FromBody] int orderId)
        {
            try
            {
                var order = await _context.LearnerOrders
                    .Include(o => o.Curriculum!)
                    .ThenInclude(c => c.Sections)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
                if (order == null)
                {
                    return NotFound("Order not found");
                }

                order.OrderStatus = "Forced to cancel";
                _context.Database.ExecuteSql($"UPDATE dbo.LearnerOrder SET OrderStatus = 'Forced to cancel' WHERE OrderId = {order.OrderId}");
                var saved = false;
                while (!saved)
                {
                    try
                    {
                        // Attempt to save changes to the database
                        await _context.SaveChangesAsync();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        foreach (var entry in ex.Entries)
                        {
                            if (entry.Entity is LearnerOrder)
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in proposedValues.Properties)
                                {
                                    var proposedValue = proposedValues[property];
                                    var databaseValue = databaseValues![property];

                                    // TODO: decide which value should be written to database
                                    // proposedValues[property] = <value to be saved>;
                                }

                                // Refresh original values to bypass next concurrency check
                                entry.OriginalValues.SetValues(databaseValues!);
                            }
                            else
                            {
                                throw new NotSupportedException(
                                    "Don't know how to handle concurrency conflicts for "
                                    + entry.Metadata.Name);
                            }
                        }
                    }
                }

                await NotifyLearnerAboutBookingStatus(order.LearnerId, orderId, "forced to cancel by admin");
                await NotifyTutorAboutBookingStatus(order.Curriculum!.TutorId, orderId, "forced to cancel by admin");
                return Ok(new { message = "Cancel booking successfully!", Order = order });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //cancel của learner nha, đừng có lộn
        [HttpPost("cancel-booking")]
        [Authorize(Roles = "LEARNER")]
        public async Task<IActionResult> CancelBooking(int orderId, int learnerId)
        {
            try
            {
                var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerId);
                if (learner == null)
                {
                    return NotFound("Learner not found!");
                }

                var order = await _context.LearnerOrders.Include(o => o.Learner).Include(o => o.Curriculum).FirstOrDefaultAsync(o => o.OrderId == orderId && o.LearnerId == learnerId);
                if (order == null)
                {
                    return NotFound("Order not found!");
                }

                if (order.OrderStatus == "Accepted")
                {
                    return BadRequest(new { message = "Cannot cancel booking after being accepted. If you really want to cancel, contact Support Centre for more details." });
                }

                if (order.OrderStatus == "Rejected")
                {
                    return BadRequest(new { message = "Cannot cancel booking because the tutor has rejected it. Refunded to your wallet!" });
                }

                var timeSinceBooking = DateTime.Now - order.OrderDate;
                if (timeSinceBooking.TotalHours > 48)
                {
                    return BadRequest(new { message = "Cannot cancel booking after 48 hours. If you really want to cancel, contact Support Centre for more details." });
                }

                bool isPaid = false;
                if (order.OrderStatus == "Paid")
                {
                    isPaid = true;
                    await RefundPayment(order.Total, learnerId);
                }

                order.OrderStatus = "Cancelled";
                _context.LearnerOrders.Update(order);
                await _context.SaveChangesAsync();

                await NotifyLearnerAboutBookingStatus(learnerId, order.OrderId, "cancelled and refunded");
                await NotifyTutorAboutBookingStatus(order.Curriculum!.TutorId, orderId, "cancelled by the learner");

                return Ok(new { message1 = "Cancel booking successfully!", message2 = isPaid ? "Refunded to your wallet!" : null });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("accept-booking")]
        [Authorize(Roles = "TUTOR")]
        public async Task<IActionResult> AcceptBooking([FromBody] TutorResponse response)
        {
            try
            {
                var order = await _context.LearnerOrders
                    .Include(o => o.Curriculum!)
                    .ThenInclude(c => c.Sections)
                    .FirstOrDefaultAsync(o => o.OrderId == response.OrderId && o.Curriculum!.TutorId == response.TutorId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                if (order.OrderStatus != "Paid")
                {
                    return BadRequest("Order status is not at paid status");
                }


                var curriculum = await _context.Curricula.FirstOrDefaultAsync(c => c.CurriculumId == order.CurriculumId);

                if (curriculum!.CurriculumType == "ShortTerm")
                {
                    var stbCondition = await _context.STBConditions.FirstOrDefaultAsync(c => c.OrderId == order.OrderId);
                    var sectionStart = stbCondition!.StartTime;
                    var sectionEnd = sectionStart.AddMinutes(stbCondition!.Duration);

                    //check xem có trùng section trong schedule không
                    var isConflict = await _context.Sections.AnyAsync(s => s.Curriculum!.TutorId == response.TutorId
                                                            && s.SectionStart <= sectionEnd
                                                            && s.SectionEnd >= sectionStart);

                    if (isConflict)
                    {
                        return Conflict("Section time conflicts with existing sections.");
                    }
                }
                else
                {
                    //chưa xử lí
                }

                order.OrderStatus = "Accepted";
                _context.Database.ExecuteSql($"UPDATE dbo.LearnerOrder SET OrderStatus = 'Accepted' WHERE OrderId = {order.OrderId}");
                var saved = false;
                while (!saved)
                {
                    try
                    {
                        // Attempt to save changes to the database
                        await _context.SaveChangesAsync();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        foreach (var entry in ex.Entries)
                        {
                            if (entry.Entity is LearnerOrder)
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in proposedValues.Properties)
                                {
                                    var proposedValue = proposedValues[property];
                                    var databaseValue = databaseValues![property];

                                    // TODO: decide which value should be written to database
                                    // proposedValues[property] = <value to be saved>;
                                }

                                // Refresh original values to bypass next concurrency check
                                entry.OriginalValues.SetValues(databaseValues!);
                            }
                            else
                            {
                                throw new NotSupportedException(
                                    "Don't know how to handle concurrency conflicts for "
                                    + entry.Metadata.Name);
                            }
                        }
                    }
                }

                await NotifyLearnerAboutBookingStatus(order.LearnerId, order.OrderId, "accepted");

                //TODO: tạo section cho booking
                var meetUrl = HttpContext.Session.GetString("MeetUrl");
                if (curriculum!.CurriculumType == "ShortTerm")
                {
                    var stbCondition = await _context.STBConditions.FirstOrDefaultAsync(c => c.OrderId == order.OrderId);
                    var sectionStart = stbCondition!.StartTime;
                    var sectionEnd = sectionStart.AddMinutes(stbCondition!.Duration);

                    var section = new Section
                    {
                        SectionStart = sectionStart,
                        SectionEnd = sectionEnd,
                        SectionStatus = "Not Started", //Not Started, Completed
                        CurriculumId = curriculum.CurriculumId,
                        MeetUrl = meetUrl,
                    };
                    _context.Sections.Add(section);
                    await _context.SaveChangesAsync();
                }
                if (curriculum!.CurriculumType == "LongTerm")
                {
                    var startTime = _memoryCache.Get<TimeSpan>($"{order.LearnerId}request{response.TutorId}_startTime_LT");
                    var day1 = _memoryCache.Get<DayOfWeek>($"{order.LearnerId}request{response.TutorId}_day1_LT");
                    var day2 = _memoryCache.Get<DayOfWeek>($"{order.LearnerId}request{response.TutorId}_day2_LT");
                    var duration = _memoryCache.Get<int>($"{order.LearnerId}request{response.TutorId}_duration_LT");
                    var weekNumber = _memoryCache.Get<int>($"{order.LearnerId}request{response.TutorId}_weekNumber_LT");
                    var year = _memoryCache.Get<int>($"{order.LearnerId}request{response.TutorId}_year_LT");
                    List<Section> sections = new();
                    var loopNum = (curriculum.TotalSlot / 2) - 1;
                    for (int i = 0; i <= loopNum; i++)
                    {
                        var section_start1 = GetDateTimeFromWeek(year, weekNumber + i, day1, startTime);
                        var sectionDay1 = new Section
                        {
                            SectionStart = section_start1,
                            SectionEnd = section_start1.AddMinutes(duration),
                            SectionStatus = "Not Started",
                            CurriculumId = curriculum.CurriculumId,
                            MeetUrl = meetUrl,
                        };
                        sections.Add(sectionDay1);

                        var section_start2 = GetDateTimeFromWeek(year, weekNumber + i, day2, startTime);
                        var sectionDay2 = new Section
                        {
                            SectionStart = section_start2,
                            SectionEnd = section_start2.AddMinutes(duration),
                            SectionStatus = "Not Started",
                            CurriculumId = curriculum.CurriculumId,
                            MeetUrl = meetUrl,
                        };
                        sections.Add(sectionDay2);
                    }

                    _context.Sections.AddRange(sections);
                    await _context.SaveChangesAsync();
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_startTime_LT");
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_day1_LT");
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_day2_LT");
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_duration_LT");
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_weekNumber_LT");
                    _memoryCache.Remove($"{order.LearnerId}request{response.TutorId}_year_LT");
                }

                return Ok("Booking accepted.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("create-cancel-request/{tutorId}")]
        [Authorize(Roles = "TUTOR")]
        public async Task<IActionResult> RequireCancelAcceptedOrder([FromRoute] int tutorId, [FromBody] int orderId)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
                if (tutor == null)
                {
                    return NotFound("Tutor not found");
                }

                var order = await _context.LearnerOrders.FirstOrDefaultAsync(o => o.OrderId == orderId && o.Curriculum!.TutorId == tutorId);
                if (order == null)
                {
                    return NotFound("No order can be found with this tutorId.");
                }

                if (order.OrderStatus != "Accepted")
                {
                    if (order.OrderStatus == "Rejected")
                    {
                        return BadRequest(new { message = "You have rejected this order before." });
                    }
                    else
                    {
                        return BadRequest(new { message = "You have not accepted this order before." });
                    }
                }

                await NotifyLearnerAboutBookingStatus(order.LearnerId, orderId, "required to cancel by the tutor");

                return Ok(new { message = "Request has been sent to learner.", Order = order });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("accept-cancel-request/{order}/{tutorId}")]
        [Authorize(Roles = "LEARNER")]
        public async Task<IActionResult> AcceptCancelRequest([FromRoute] int tutorId, [FromRoute] int orderId)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
                if (tutor == null)
                {
                    return NotFound("Tutor not found");
                }

                var order = await _context.LearnerOrders.FirstOrDefaultAsync(o => o.OrderId == orderId && o.Curriculum!.TutorId == tutorId);
                if (order == null)
                {
                    return NotFound("No order can be found with this tutorId.");
                }

                order.OrderStatus = "Paid";
                _context.LearnerOrders.Update(order);
                await _context.SaveChangesAsync();

                await NotifyTutorAboutCancelRequest(tutorId, orderId, "accepted");

                return Ok(new { message = "Accept cancel request successfully!", Order = order });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reject-cancel-request/{order}/{tutorId}")]
        [Authorize(Roles = "LEARNER")]
        public async Task<IActionResult> RejectCancelRequest([FromRoute] int tutorId, [FromRoute] int orderId)
        {
            try
            {
                var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
                if (tutor == null)
                {
                    return NotFound("Tutor not found");
                }

                var order = await _context.LearnerOrders.FirstOrDefaultAsync(o => o.OrderId == orderId && o.Curriculum!.TutorId == tutorId);
                if (order == null)
                {
                    return NotFound("No order can be found with this tutorId.");
                }

                await NotifyTutorAboutCancelRequest(tutorId, orderId, "rejected");

                return Ok(new { message = "Reject cancel request successfully!", Order = order });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private static DateTime GetDateTimeFromWeek(int year, int weekNumber, DayOfWeek dayOfWeek, TimeSpan timeOfDay)
        {
            var weeks = GetWeeks(year);

            if (weekNumber < 1 || weekNumber > weeks.Count)
            {
                throw new ArgumentOutOfRangeException(nameof(weekNumber), "Invalid week number.");
            }

            var selectedWeek = weeks[weekNumber - 1];

            DateTime result = selectedWeek.StartDate.AddDays((int)dayOfWeek).Add(timeOfDay);
            return result;
        }

        private static List<WeekViewModel> GetWeeks(int year)
        {
            var weeks = new List<WeekViewModel>();
            var startDate = new DateTime(year, 1, 1);

            while (startDate.DayOfWeek != DayOfWeek.Monday)
            {
                startDate = startDate.AddDays(1);
            }

            var endDate = new DateTime(year, 12, 31);

            while (startDate <= endDate)
            {
                var week = new WeekViewModel
                {
                    StartDate = startDate,
                    EndDate = startDate.AddDays(6),
                };
                weeks.Add(week);
                startDate = startDate.AddDays(7);
            }

            return weeks;
        }

        [HttpPost("confirm-section-completion")]
        [Authorize(Roles = "TUTOR")]
        public async Task<IActionResult> ConfirmSectionCompletion([FromQuery] int orderId, [FromQuery] int sectionId)
        {
            var order = await _context.LearnerOrders
                                    .Include(o => o.Curriculum!)
                                    .ThenInclude(c => c.Sections)
                                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound("Not found order");
            }

            if (order.OrderStatus != "Accepted")
            {
                return BadRequest("Order is not at accepted status. Cannot operate.");
            }

            if (order.IsCompleted)
            {
                return BadRequest("Order has been comfirmed as completed before.");
            }

            var section = await  _context.Sections.FirstOrDefaultAsync(s => s.SectionId == sectionId);

            if (section == null)
            {
                return NotFound("Not found section");
            }

            if (section.SectionStatus == "Completed")
            {
                return BadRequest("This section has been completed before. Cannot operate");
            }

            if (section.SectionEnd < DateTime.Now)
            {
                section.SectionStatus = "Completed";
                _context.Sections.Update(section);
                await _context.SaveChangesAsync();

                return Ok(new { Section = section, Order = order });
            }

            return BadRequest("An error has occurred");
        }

        [HttpPost("comfirm-order-completion")]
        [Authorize(Roles = "TUTOR")]
        public async Task<IActionResult> ConfirmOrderCompletion([FromQuery] int orderId)
        {
            var order = await _context.LearnerOrders
                                    .Include(o => o.Curriculum!)
                                    .ThenInclude(c => c.Sections)
                                    .FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                return NotFound("Not found order");
            }

            var tutor = await _context.Tutors
                                    .Include(t => t.TutorNavigation)
                                    .ThenInclude(a => a.Wallet)
                                    .FirstOrDefaultAsync(t => t.TutorId == order.Curriculum!.TutorId);
            if (tutor == null)
            {
                return NotFound("Not found tutor");
            }

            if (order.OrderStatus != "Accepted")
            {
                return BadRequest("Order is not at accepted status. Cannot operate.");
            }

            if (order.IsCompleted)
            {
                return BadRequest("Order has been comfirmed as completed before.");
            }

            var stbCondition = await _context.STBConditions.FirstOrDefaultAsync(c => c.OrderId == order.OrderId);

            var lastSection = await _context.Sections.FirstOrDefaultAsync(s => s.CurriculumId == order.CurriculumId
                                                                    && s.SectionStart == stbCondition!.StartTime);
            if (lastSection == null || lastSection.SectionStatus != "Completed")
            {
                return BadRequest("Cannot confirm order completion. The last section has not ended yet.");
            }

            order.IsCompleted = true;
            _context.LearnerOrders.Update(order);
            await _context.SaveChangesAsync();

            var accountWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == order.Curriculum!.TutorId);
            if (tutor.TutorNavigation.Wallet == null)
            {
                if (accountWallet == null)
                {
                    var wallet = new Wallet
                    {
                        WalletId = tutor.TutorId,
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
                        WalletId = tutor.TutorId,
                        Balance = currBalance,
                    };
                    _context.Wallets.Add(wallet);
                    await _context.SaveChangesAsync();
                }
            }

            var tutorWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == tutor.TutorId);
            tutorWallet!.Balance += order.Total;
            _context.Wallets.Update(tutorWallet);

            var transaction = new Transaction
            {
                AccountId = tutor.TutorId,
                Amount = order.Total,
                TransactionDate = DateTime.Now,
                TransactionType = "Receive Wage",
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                message1 = "Confirmed successfully!", 
                Order = order, 
                message2 = "Money has been to your wallet.", 
                balance = tutorWallet.Balance 
            });
        }

        [HttpPost("reject-booking")]
        [Authorize(Roles = "TUTOR")]
        public async Task<IActionResult> RejectBooking([FromBody] TutorResponse response)
        {
            try
            {
                var order = await _context.LearnerOrders
                    .Include(o => o.Curriculum!)
                    .FirstOrDefaultAsync(o => o.OrderId == response.OrderId && o.Curriculum!.TutorId == response.TutorId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                if (order.OrderStatus != "Paid")
                {
                    return BadRequest("Order status is not at paid status");
                }

                order.OrderStatus = "Rejected";
                _context.Database.ExecuteSql($"UPDATE dbo.LearnerOrder SET OrderStatus = 'Rejected' WHERE OrderId = {order.OrderId}");
                var saved = false;
                while (!saved)
                {
                    try
                    {
                        // Attempt to save changes to the database
                        await _context.SaveChangesAsync();
                        saved = true;
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        foreach (var entry in ex.Entries)
                        {
                            if (entry.Entity is LearnerOrder)
                            {
                                var proposedValues = entry.CurrentValues;
                                var databaseValues = entry.GetDatabaseValues();

                                foreach (var property in proposedValues.Properties)
                                {
                                    var proposedValue = proposedValues[property];
                                    var databaseValue = databaseValues![property];

                                    // TODO: decide which value should be written to database
                                    // proposedValues[property] = <value to be saved>;
                                }

                                // Refresh original values to bypass next concurrency check
                                entry.OriginalValues.SetValues(databaseValues!);
                            }
                            else
                            {
                                throw new NotSupportedException(
                                    "Don't know how to handle concurrency conflicts for "
                                    + entry.Metadata.Name);
                            }
                        }
                    }
                }

                await NotifyLearnerAboutBookingStatus(order.LearnerId, order.OrderId, "rejected by the tutor");

                await RefundPayment(order.Total, (int)order.LearnerId!);

                return Ok("Booking rejected and payment refunded.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-orders-by-time-period")]
        public async Task<IActionResult> GetOrdersByTimePeriod([FromQuery] DateTime startTime, [FromQuery] DateTime endTime, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                IQueryable<LearnerOrder> query = _context.LearnerOrders.Where(o => o.OrderStatus == "Accepted");
                query = query.Where(o => o.OrderDate >= startTime && o.OrderDate <= endTime);
                query = query.OrderBy(o => o.OrderDate);
                var totalCount = await query.CountAsync();
                var orders = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
                if (orders == null || orders.Count == 0)
                {
                    return NotFound("Not found orders in this time period.");
                }

                var response = new PaginatedResponse<LearnerOrder>
                {
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    Items = orders,
                };
                var numOfPages = totalCount / pageSize;
                if (totalCount % pageSize != 0)
                {
                    numOfPages++;
                }
                return Ok(new { Response = response, NumOfPages = numOfPages });
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
                    .FirstOrDefaultAsync(a => a.Id == learnerId);

                if (learner != null)
                {
                    var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == learnerId);
                    wallet!.Balance += amount;
                    _context.Wallets.Update(wallet);

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

        private async Task NotifyTutorAboutCancelRequest(int? tutorId, int orderId, string status)
        {
            var order = await _context.LearnerOrders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                throw new Exception("Not found order.");
            }

            var notification = new UserNotification
            {
                Content = $"Your cancel request for order having curriculum {order.Curriculum!.CurriculumType} has been {status} by the learner.",
                NotificateDay = DateTime.Now,
                AccountId = (int)tutorId!,
                NotiStatus = "NEW",
            };

            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
            if (tutor != null)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == tutorId);
                string subject = "Booking Status Update";
                string message = $"Dear {account!.FirstName}, \n\nYour cancel request for order having curriculum {order.Curriculum!.CurriculumType} has been {status} by the learner. Please login to your dashboard for more details.";

                await _emailService.SendMailAsync(tutor.TutorEmail, subject, message);
            }
            else
            {
                throw new Exception("Not found learner");
            }

            _context.UserNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        private async Task NotifyTutorAboutBookingStatus(int? tutorId, int orderId, string status)
        {
            var order = await _context.LearnerOrders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                throw new Exception("Not found order.");
            }

            var notification = new UserNotification
            {
                Content = $"Your booking request for curriculum {order.Curriculum!.CurriculumType} has been {status}.",
                NotificateDay = DateTime.Now,
                AccountId = (int)tutorId!,
                NotiStatus = "NEW",
            };

            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
            if (tutor != null)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == tutorId);
                string subject = "Booking Status Update";
                string message = $"Dear {account!.FirstName}, \n\nYour booking request for curriculum {order.Curriculum!.CurriculumType} has been {status}. Please login to your dashboard for more details.";

                await _emailService.SendMailAsync(tutor.TutorEmail, subject, message);
            }
            else
            {
                throw new Exception("Not found learner");
            }

            _context.UserNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        private async Task NotifyLearnerAboutBookingStatus(int? learnerId, int orderId, string status)
        {
            var order = await _context.LearnerOrders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                throw new Exception("Not found order.");
            }

            var notification = new UserNotification
            {
                Content = $"Your booking request for curriculum {order.Curriculum!.CurriculumType} has been {status}.",
                NotificateDay = DateTime.Now,
                AccountId = (int)learnerId!,
                NotiStatus = "NEW",
            };

            var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerId);
            if (learner != null)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == learnerId);
                string subject = "Booking Status Update";
                string message = $"Dear {account!.FirstName}, \n\nYour booking request for curriculum {order.Curriculum!.CurriculumType} has been {status}. Please login to your dashboard for more details.";

                await _emailService.SendMailAsync(learner.LearnerEmail, subject, message);
            }
            else
            {
                throw new Exception("Not found learner");
            }

            _context.UserNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        private async Task NotifyTutorAboutBooking(int? tutorId, int orderId)
        {
            var order = await _context.LearnerOrders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                throw new Exception("Not found order");
            }
            var tutor = await _context.Tutors.FirstOrDefaultAsync(t => t.TutorId == tutorId);
            if (tutor == null)
            {
                throw new Exception("Not found tutor.");
            }
            var notification = new UserNotification
            {
                Content = $"You have received a new booking request for curriculum {order.Curriculum!.CurriculumType}.",
                NotificateDay = DateTime.Now,
                AccountId = (int)tutorId!,
                NotiStatus = "NEW",
            };
            if (tutor != null && tutor.TutorNavigation != null)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == tutorId);
                string subject = "New Booking Request";
                string message = $"Dear {account!.FirstName}, \n\nYou have received a new booking request for curriculum {order.Curriculum!.CurriculumType}. Please login to your dhashboard for more details.";

                await _emailService.SendMailAsync(tutor.TutorEmail, subject, message);
            }

            _context.UserNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        private async Task NotifyLearnerAboutBooking(int? learnerId, int orderId)
        {
            var order = await _context.LearnerOrders.FirstOrDefaultAsync(o => o.OrderId == orderId);
            if (order == null)
            {
                throw new Exception("Not found order");
            }
            var learner = await _context.Learners.FirstOrDefaultAsync(l => l.LearnerId == learnerId);
            var notification = new UserNotification
            {
                Content = $"Your booking request for curriculum {order.Curriculum!.CurriculumType} has been sent to the tutor.",
                NotificateDay = DateTime.Now,
                AccountId = (int)learnerId!,
                NotiStatus = "NEW",
            };
            if (learner != null && learner.LearnerNavigation != null)
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == learnerId);
                string subject = "Booking Sent";
                string message = $"Dear {account!.FirstName}, \n\nYour booking request for curriculum {order.Curriculum!.CurriculumType} has been sent to the tutor.";

                await _emailService.SendMailAsync(learner.LearnerEmail, subject, message);
            }
            else
            {
                throw new Exception("Not found learner");
            }

            _context.UserNotifications.Add(notification);
            await _context.SaveChangesAsync();
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
                    Description = "Top-up wallet",
                },
                Mode = "payment",
                SuccessUrl = $"https://localhost:3000",
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
                return Ok(new { SessionId = session.Id, url = session.Url });
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
                                Name = "Top-up remain",
                            },
                        },
                        Quantity = 1,
                    },
                },
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Description = "Top-up remain to complete booking payment",
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
        // GET: api/OrderHistory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LearnerOrder>>> GetOrderHistory()
        {
            return await _context.LearnerOrders.ToListAsync();
        }

        // GET: api/OrderHistory/Learner/5
        [HttpGet("Learner/{learnerId}")]
        public async Task<ActionResult<IEnumerable<LearnerOrder>>> GetOrdersByLearnerId([FromRoute] int learnerId)
        {
            var orders = await _context.LearnerOrders
                                       .Where(order => order.LearnerId == learnerId)
                                       .ToListAsync();

            if (orders == null || !orders.Any())
            {
                return NotFound();
            }

            return orders;
        }
    }
}
