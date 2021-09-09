using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Moq;

namespace Test.Base
{
    /// <summary>
    /// Class containing method for mocking <see cref="IHttpContextAccessor"/>.
    /// </summary>
    public static class MockIHttpContextAccessor
    {
        /// <summary>
        /// Mocks the <see cref="IHttpContextAccessor"/> for testing usage
        /// </summary>
        /// <param name="userClaims">Claims of the user who is logged in</param>
        /// <returns>Mocked IHttpContextAccessor</returns>
        public static IHttpContextAccessor Mock(IEnumerable<Claim> userClaims)
        {
            ClaimsPrincipal user = new ClaimsPrincipal(new ClaimsIdentity(userClaims));

            DefaultHttpContext context = new DefaultHttpContext
            {
                User = user
            };

            Mock<IHttpContextAccessor> mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            return mockHttpContextAccessor.Object;
        }
    }
}