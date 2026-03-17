using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MarketAPI.Entities;
using MarketAPI.Options;
using MarketAPI.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace MarketAPI.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly JwtOptions _options;

        public JwtTokenService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        public string GenerateToken(User user, string provider)
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new(JwtRegisteredClaimNames.Name, user.Name),
                new("provider", provider)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_options.ExpiresMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
