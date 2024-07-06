using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;
using ODTDemoAPI.EntityViewModels;

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

        //all transaction
        [HttpGet("get-all-transactions")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetAllTransactions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            IQueryable<Transaction> query = _context.Transactions.OrderByDescending(t => t.TransactionDate);
            var totalCount = await query.CountAsync();
            var transactions = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            if (transactions == null || transactions.Count == 0)
            {
                return NotFound("No transaction was found.");
            }
            var response = new PaginatedResponse<Transaction>
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Items = transactions,
            };

            int numOfPages = totalCount / pageSize;
            if (totalCount % pageSize != 0)
            {
                numOfPages += 1;
            }
            return Ok(new { Response = response, NumOfPages = numOfPages });
        }

        //sao kê
        [HttpGet("get-all-transactions/{accountId}")]
        public async Task<IActionResult> GetAllTransactionsById(int accountId)
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

        //[HttpPost("receive-wage")]
        //[Authorize(Roles = "TUTOR")]
        //public async Task<IActionResult> ReceiveWage([FromForm] int amount, int tutorId)
        //{
        //    try
        //    {
        //        var tutor = await _context.Tutors.Include(t => t.TutorNavigation).ThenInclude(a => a.Wallet).FirstOrDefaultAsync(t => t.TutorId == tutorId);
        //        if(tutor == null)
        //        {
        //            return NotFound("Tutor not found");
        //        }

        //        if(amount == 0)
        //        {
        //            return BadRequest(new { message = "Invalid amount." });
        //        }

        //        var accountWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == tutorId);
        //        if (tutor.TutorNavigation.Wallet == null)
        //        {
        //            if(accountWallet == null)
        //            {
        //                var wallet = new Wallet
        //                {
        //                    WalletId = tutorId,
        //                    Balance = 0,
        //                };
        //                _context.Wallets.Add(wallet);

        //                await _context.SaveChangesAsync();
        //            }
        //            else
        //            {
        //                decimal currBalance = accountWallet.Balance;
        //                _context.Wallets.Remove(accountWallet);
        //                await _context.SaveChangesAsync();

        //                var wallet = new Wallet
        //                {
        //                    WalletId = tutorId,
        //                    Balance = currBalance,
        //                };
        //                _context.Wallets.Add(wallet);
        //                await _context.SaveChangesAsync();
        //            }
        //        }

        //        var totalEarnings = await _context.LearnerOrders
        //                                .Where(o => o.Curriculum!.TutorId == tutorId && o.OrderStatus == "Accepted" && o.IsCompleted == true)
        //                                .SumAsync(o => o.Total);

        //        var totalWithdrawn = await _context.Transactions
        //                                .Where(t => t.AccountId == tutorId && t.TransactionType == "Receive Wage")
        //                                .SumAsync(t => t.Amount);

        //        var availableAmount = totalEarnings - totalWithdrawn;

        //        if(availableAmount == 0)
        //        {
        //            return BadRequest(new { message = "Your funds has reached limit. No more withdrawal until your next booking." });
        //        }

        //        if(amount > availableAmount)
        //        {
        //            return BadRequest(new { message = "Your withdrawal exceeds the available funds." });
        //        }

        //        var tutorWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == tutorId);
        //        tutorWallet!.Balance += amount;
        //        _context.Wallets.Update(tutorWallet);

        //        var transaction = new Transaction
        //        {
        //            AccountId = tutorId,
        //            Amount = amount,
        //            TransactionDate = DateTime.Now,
        //            TransactionType = "Receive Wage",
        //        };

        //        _context.Transactions.Add(transaction);
        //        await _context.SaveChangesAsync();

        //        return Ok(new 
        //        { 
        //            balance = tutorWallet.Balance, 
        //            message1 = "Reeive successfully!", 
        //            message2 = totalEarnings == totalWithdrawn ? "Your funds has reached limit. No more withdrawal until your next booking." : null, 
        //            message3 = $"Available funds: {totalEarnings - totalWithdrawn}" 
        //        });
        //    }
        //    catch(Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet("revenue-monthly")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetMonthlyRevenue(int year, int month)
        {
            var revenue = await _context.Transactions
                .Where(t => t.TransactionDate.Year == year 
                            && t.TransactionDate.Month == month 
                            &&( t.TransactionType == "Membership SILVER" || t.TransactionType == "Membership PRENIUM"))
                .SumAsync(t => t.Amount);
            return Ok(new {Year = year, Month = month, Revenue = revenue});
        }

        [HttpGet("revenue-yearly")]
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> GetYearlyRevenue(int year)
        {
            var revenue = await _context.Transactions
                .Where(t => t.TransactionDate.Year == year
                            && (t.TransactionType == "Membership SILVER" || t.TransactionType == "Membership PRENIUM"))
                .SumAsync(t => t.Amount);
            return Ok(new { Year = year, Revenue = revenue });
        }
    }
}
