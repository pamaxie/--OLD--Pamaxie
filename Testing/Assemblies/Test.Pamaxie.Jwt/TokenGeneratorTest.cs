using System.Collections.Generic;
using Pamaxie.Jwt;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Jwt_Test
{
    /// <summary>
    /// Testing class for <see cref="TokenGenerator"/>
    /// </summary>
    public class TokenGeneratorTest : TestBase
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;

        public TokenGeneratorTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
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

        /// <summary>
        /// Test for decrypting a JWT bearer token through <see cref="TokenGenerator.GetUserKey"/>
        /// </summary>
        /// <param name="authToken">The auth token to decrypt</param>
        [Theory]
        [InlineData("")] //TODO Needs testing data
        public void GetUserKey(string authToken)
        {
            //TODO
        }
    }
}