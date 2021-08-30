using System.Collections.Generic;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
using Test.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Api
{
    /// <summary>
    /// Testing class for <see cref="TokenGenerator"/>
    /// </summary>
    public class TokenGeneratorBaseTest : BaseTest
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;
        
        public TokenGeneratorBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        /// <summary>
        /// Test for creating a token through <see cref="TokenGenerator.CreateToken"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void CreateToken(string userKey)
        {
            TokenGenerator generator = new(Configuration);
            AuthToken authToken = generator.CreateToken(userKey);
            Assert.NotNull(authToken);
            Assert.NotEmpty(authToken.Token);
            TestOutputHelper.WriteLine("Token: {0}", authToken.Token);
        }
    }
}