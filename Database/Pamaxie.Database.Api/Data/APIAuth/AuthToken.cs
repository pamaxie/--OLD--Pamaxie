using System;

namespace Pamaxie.Api.Data
{
    /// <summary>
    /// Data returned by the Authentication
    /// </summary>
    public class AuthToken
    {
        public string Token { get; set; }
        public DateTime ExpirationUtc { get; set; }
    }
}