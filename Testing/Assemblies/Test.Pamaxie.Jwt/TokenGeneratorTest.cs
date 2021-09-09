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
    public sealed class TokenGeneratorTest : TestBase
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
            //Test the CreateToken method to see if it creates a valid token
            TokenGenerator generator = new TokenGenerator(Configuration);
            AuthToken authToken = generator.CreateToken(userKey);
            Assert.NotNull(authToken);
            Assert.NotEmpty(authToken.Token);
            TestOutputHelper.WriteLine("Token: {0}", authToken.Token);
            TestOutputHelper.WriteLine("Expires at {0:R}", authToken.ExpirationUtc.ToLocalTime());
        }

        /// <summary>
        /// Test for decrypting a JWT bearer token through <see cref="TokenGenerator.GetUserKey"/>
        /// </summary>
        /// <param name="expectedUserKey">The expected user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void GetUserKey(string expectedUserKey)
        {
            //Create a auth token from the user key
            TokenGenerator generator = new TokenGenerator(Configuration);
            AuthToken authToken = generator.CreateToken(expectedUserKey);
            Assert.NotNull(authToken);
            //Test the GetUserKey method to see if it returns the user's key
            string userKey = TokenGenerator.GetUserKey(authToken.Token);
            Assert.False(string.IsNullOrEmpty(userKey));
            TestOutputHelper.WriteLine("Actual user key {0}", userKey);
            TestOutputHelper.WriteLine("Expected user key {0}", expectedUserKey);
            Assert.Equal(expectedUserKey, userKey);
        }
    }
}