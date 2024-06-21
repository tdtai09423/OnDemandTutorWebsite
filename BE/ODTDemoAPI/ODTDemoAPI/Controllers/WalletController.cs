using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly OnDemandTutorContext _context;

        public WalletController(OnDemandTutorContext context)
        {
            _context = context;
        }

        [HttpGet("get-wallet")]
        public async Task<IActionResult> GetWallet([FromQuery] int accountId)
        {
            try
            {
                var account = await _context.Accounts.FirstOrDefaultAsync(a => a.Id == accountId);
                if(account == null)
                {
                    return NotFound("Account not found!");
                }

                var accountWallet = await _context.Wallets.FirstOrDefaultAsync(w => w.WalletId == accountId);
                if(account.Wallet == null)
                {
                    if(accountWallet == null)
                    {
                        var wallet = new Wallet
                        {
                            WalletId = accountId,
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
                            WalletId = accountId,
                            Balance = currBalance,
                        };
                        _context.Wallets.Add(wallet);
                        await _context.SaveChangesAsync();
                    }

                    account.Wallet = accountWallet;
                }

                var accountView = await _context.Accounts.Include(a => a.Wallet).FirstOrDefaultAsync(a => a.Id == accountId);
                return Ok(accountView);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
