using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace Pamaxie.Jwt
{
    /// <summary>
    /// Authentication token generator
    /// </summary>
    public sealed class TokenGenerator
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
        public AuthToken CreateToken(string userId, string authTokenSettings = null)
        {
            //TODO: something is wrong with how this token is generated here. We need to fix this for gathering user information about a token to refresh it for example

           //Authentication successful so generate JWT Token
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            byte[] key = null;
            DateTime? expires = null;
            if (string.IsNullOrWhiteSpace(authTokenSettings))
            {
                IConfigurationSection section = _configuration.GetSection("AuthData");
                key = Encoding.ASCII.GetBytes(section.GetValue<string>("Secret"));
                expires = DateTime.UtcNow.AddMinutes(section.GetValue<int>("ExpiresInMinutes"));
            }
            else
            {
                //TODO: this is only a temporary fix until we figured out how to use our own configuration via dependency injection.
                var settings = JsonConvert.DeserializeObject<AuthSettings>(authTokenSettings);
                key = Encoding.ASCII.GetBytes(settings.Secret);
                expires = DateTime.UtcNow.AddMinutes(settings.ExpiresInMinutes);
            }

            if (expires == null || key == null)
            {
                throw new InvalidOperationException("We hit an unexpected problem while generating the token");
            }
            
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
            return new AuthToken { ExpiresAtUTC = (DateTime)expires, Token = tokenHandler.WriteToken(token) };
        }

        /// <summary>
        /// Decrypts a JWT bearer token
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public static string GetUserKey(string authToken)
        {
            var jwtToken = new JwtSecurityToken(authToken);
            return jwtToken.Subject;
        }

        public static string GenerateSecret()
        {
            using RNGCryptoServiceProvider cryptRng = new RNGCryptoServiceProvider();
            byte[] tokenBuffer = new byte[64];
            cryptRng.GetBytes(tokenBuffer);
            return Convert.ToBase64String(tokenBuffer);
        }
    }
}