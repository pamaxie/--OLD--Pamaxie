using System;

namespace Pamaxie.Api.Data
{
    public class AuthToken
    {
        public string Token { get; set; }
        public DateTime ExpirationUtc { get; set; }
    }
}