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

        public JwtService(IConfiguration configuration)
        {
            _secret = configuration.GetSection("Jwt")["secret"];
            _expirationTime = configuration.GetSection("Jwt")["expirationInMinutes"];
        }

        public string GenerateSecurityToken(string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {new Claim(ClaimTypes.Email, email)}),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(_expirationTime)),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
