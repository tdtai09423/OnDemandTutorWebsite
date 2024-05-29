using ODTDemoAPI.Entities;

namespace ODTDemoAPI.AuthOperation
{
    public interface IAuthService
    {
        string GenerateToken(Account account);
    }
}
