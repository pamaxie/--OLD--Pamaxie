using System;

#pragma warning disable 8618

namespace Pamaxie.Jwt
{
    /// <summary>
    /// Authentication Token
    /// </summary>
    public class AuthToken
    {
        /// <summary>
        /// The token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// The expiration date of the token
        /// </summary>
        public DateTime ExpirationUtc { get; set; }
    }
}