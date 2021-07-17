using System;
#pragma warning disable 8618

namespace Pamaxie.Api.Data
{
    public class AuthToken
    {
        public string Token { get; set; }
        public DateTime ExpirationUtc { get; set; }
    }
}