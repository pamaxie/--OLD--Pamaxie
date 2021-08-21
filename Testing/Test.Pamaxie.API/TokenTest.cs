using System.IO;
using Microsoft.Extensions.Configuration;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.API_UnitTesting
{
    /// <summary>
    /// Testing class for AuthController
    /// </summary>
    public class TokenTest : Base.Test
    {
        public TokenTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        /// <summary>
        /// Testing for succeeding in generating a new token from a id
        /// </summary>
        [Theory]
        [InlineData("mc2cc3_1x9132cx_3474nv")]
        [InlineData("c43354_c5c3412c_2c423t")]
        public void GenerateToken_Succeed(string id)
        {
            //appsettings.test.json needs to have a secret and expiration time, as they will be used in TokenGenerator
            IConfiguration configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.test.json").Build();
            TokenGenerator tokenGenerator = new(configuration);
            AuthToken result = tokenGenerator.CreateToken(id);
            Assert.NotNull(result);
            Assert.NotEmpty(result.Token);
        }
    }
}
