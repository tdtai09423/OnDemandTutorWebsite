using ODTDemoAPI.Entities;

namespace ODTDemoAPI.Services
{
    public interface IAuthService
    {
        string GenerateToken(Account account);
    }
}
