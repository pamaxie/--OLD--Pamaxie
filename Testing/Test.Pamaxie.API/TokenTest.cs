using System.Collections.Generic;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.API_UnitTesting
{
    /// <summary>
    /// Testing class for <see cref="TokenGenerator"/>
    /// </summary>
    public class TokenTest : Base.Test
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;

        public TokenTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        /// <summary>
        /// Testing for succeeding in generating a new token from a user key
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void GenerateToken_Succeed(string userKey)
        {
            TokenGenerator tokenGenerator = new(Configuration);
            AuthToken result = tokenGenerator.CreateToken(userKey);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
        }
    }
}
