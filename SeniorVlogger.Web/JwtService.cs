using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace SeniorVlogger.Web
{
    internal class JwtService
    {
        private readonly string _secret;
        private readonly string _expirationTime;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtService(IConfiguration configuration)
        {
            _secret = configuration.GetSection("Jwt")["Secret"];
            _expirationTime = configuration.GetSection("Jwt")["ExpirationInMinutes"];
            _issuer = configuration.GetSection("Jwt")["Issuer"];
            _audience = configuration.GetSection("Jwt")["Audience"];
        }

        public string GenerateSecurityToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(JwtRegisteredClaimNames.Sub, email), 
                }),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expirationTime)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
                Issuer = _issuer,
                Audience = _audience,
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
