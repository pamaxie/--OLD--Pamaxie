using System;

namespace PamaxieML.Api.Data
{
    public class AuthToken
    {
        public string Token { get; set; }
        public DateTime ExpirationUtc { get; set; }
    }
}