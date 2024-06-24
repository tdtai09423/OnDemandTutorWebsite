using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ODTDemoAPI.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ODTDemoAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSetting _jwtSetting;

        public AuthService(IOptionsMonitor<JwtSetting> optionsMonitor)
        {
            _jwtSetting = optionsMonitor.CurrentValue;
        }

        public string GenerateToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                new Claim(JwtRegisteredClaimNames.Jti, account.Password),
                new Claim(ClaimTypes.Role, account.RoleId)
            };

            var token = new JwtSecurityToken
                (
                    issuer: _jwtSetting.Issuer,
                    audience: _jwtSetting.Audience,
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
