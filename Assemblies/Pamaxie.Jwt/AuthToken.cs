using System;

namespace Pamaxie.Jwt
{
    /// <summary>
    /// Authentication Token
    /// </summary>
    public sealed class AuthToken
    {
        /// <summary>
        /// The token
        /// </summary>
        public string Token { get; init; }

        /// <summary>
        /// The expiration date of the token
        /// </summary>
        public DateTime ExpiresAtUTC { get; init; }
    }
}