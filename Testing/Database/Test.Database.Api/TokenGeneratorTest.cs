using Pamaxie.Api.Security;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Api
{
    /// <summary>
    /// Testing class for <see cref="TokenGenerator"/>
    /// </summary>
    public class TokenGeneratorTest : Base.Test
    {
        public TokenGeneratorTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        /// <summary>
        /// Test for creating a token through <see cref="TokenGenerator.CreateToken"/>
        /// </summary>
        [Fact] //TODO Needs testing data
        public void CreateToken()
        {
            //TODO Not yet implemented
        }
    }
}