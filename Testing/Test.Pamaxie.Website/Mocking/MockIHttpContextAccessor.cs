using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;

namespace Test.Pamaxie.Website
{
    internal static class MockIHttpContextAccessor
    {
        internal static IHttpContextAccessor Mock(IConfiguration configuration)
        {
            ClaimsPrincipal user = new(new ClaimsIdentity(TestGoogleClaimData.GoogleUserPrincipleClaims));

            DefaultHttpContext context = new()
            {
                User = user
            };
            
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            return mockHttpContextAccessor.Object;
        }
    }
}