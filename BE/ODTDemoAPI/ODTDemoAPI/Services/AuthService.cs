using Microsoft.IdentityModel.Tokens;
using ODTDemoAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ODTDemoAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;

        private const string Key = "your-256-bit-secret-your-256-bit-secret";
        private const string Issuer = "yourIssuer";
        private const string Audience = "yourAudience";

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                new Claim(JwtRegisteredClaimNames.Jti, account.Password),
                new Claim(ClaimTypes.Role, account.RoleId)
            };

            var token = new JwtSecurityToken
                (
                    issuer: Issuer,
                    audience: Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
