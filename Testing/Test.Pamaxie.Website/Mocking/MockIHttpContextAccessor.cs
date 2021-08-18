using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Class containing method for mocking IHttpContextAccessor.
    /// </summary>
    internal static class MockIHttpContextAccessor
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userClaims">Claims of the user who is logged in</param>
        /// <returns>Mocked IHttpContextAccessor</returns>
        internal static IHttpContextAccessor Mock(IEnumerable<Claim> userClaims)
        {
            ClaimsPrincipal user = new(new ClaimsIdentity(userClaims));

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