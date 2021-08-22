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
        public TokenTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        /// <summary>
        /// Testing for succeeding in generating a new token from a id
        /// TODO makes it use testing data from <see cref="MemberData"/>
        /// </summary>
        [Theory]
        [InlineData("mc2cc3_1x9132cx_3474nv")]
        [InlineData("c43354_c5c3412c_2c423t")]
        public void GenerateToken_Succeed(string id)
        {
            TokenGenerator tokenGenerator = new(Configuration);
            AuthToken result = tokenGenerator.CreateToken(id);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
        }
    }
}
