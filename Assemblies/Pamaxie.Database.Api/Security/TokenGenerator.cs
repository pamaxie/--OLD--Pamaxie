using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Pamaxie.Api.Data;

namespace Pamaxie.Api.Security
{
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AuthToken CreateToken(string userId)
        {
            // authentication successful so generate jwt token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            IConfigurationSection section = _configuration.GetSection("AuthData");
            byte[] key = Encoding.ASCII.GetBytes(section.GetValue<string>("Secret"));
            DateTime expires = DateTime.UtcNow.AddMinutes(section.GetValue<int>("ExpiresInMinutes"));
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userId)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthToken {ExpirationUtc = expires, Token = tokenHandler.WriteToken(token)};
        }
    }
}