using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public TransactionController(OnDemandTutorContext context)
        {
            _context = context;
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
    }
}
