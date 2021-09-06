using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Pamaxie.Api.Data;

namespace Pamaxie.Api.Security
{
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration.GetSection("AuthData");
        }

        public AuthToken CreateToken(string userKey)
        {
            // authentication successful so generate jwt token
            JwtSecurityTokenHandler tokenHandler = new ();
            byte[] key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Secret"));
            DateTime expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("ExpiresInMinutes"));
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, userKey)
                }),
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthToken {ExpirationUtc = expires, Token = tokenHandler.WriteToken(token)};
        }

        /// <summary>
        /// Decrypts a JWT bearer token
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public string GetUserKey(string authToken)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(authToken);
            var tokenS = jsonToken as JwtSecurityToken;
            return tokenS?.Id;
        }
    }
}