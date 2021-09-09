using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Pamaxie.Jwt
{
    /// <summary>
    /// Authentication token generator
    /// </summary>
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Creates a token object through JWT encoding
        /// </summary>
        /// <param name="userId">User id of the user that will be contained in the token</param>
        /// <returns>A authentication token object</returns>
        public AuthToken CreateToken(string userId)
        {
            //Authentication successful so generate JWT Token
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

            return new AuthToken { ExpirationUtc = expires, Token = tokenHandler.WriteToken(token) };
        }

        /// <summary>
        /// Decrypts a JWT bearer token
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public static string GetUserKey(string authToken)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.ReadJwtToken(authToken);
            return token?.Claims.First(claim => claim.Type == "unique_name").Value;
        }
    }
}