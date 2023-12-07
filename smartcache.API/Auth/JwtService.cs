using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace smartcache.API.Auth
{
    public class JwtService
    {
        private readonly string? _secretKey;
        private readonly string? _issuer;
        private readonly string? _audience;

        public JwtService(IConfiguration configuration)
        {
            _secretKey = configuration.GetSection("JWT-secret").Value;
            _issuer = configuration.GetSection("JWT-issuer").Value;
            _audience = configuration.GetSection("JWT-audience").Value;
        }

        public string GenerateToken(string username, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role),
            };

            var token = new JwtSecurityToken(
                _issuer,
                _audience,
                claims,
                expires: DateTime.UtcNow.AddHours(24),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
