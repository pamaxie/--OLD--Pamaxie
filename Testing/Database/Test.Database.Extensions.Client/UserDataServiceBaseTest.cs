using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Test.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Extensions.Client
{
    /// <summary>
    /// Testing class for <see cref="UserDataService"/>
    /// </summary>
    public class UserDataServiceBaseTest : BaseTest
    {
        public UserDataServiceBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
    }
}