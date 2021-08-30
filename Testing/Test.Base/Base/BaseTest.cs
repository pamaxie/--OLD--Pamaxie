using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Test.TestBase
{
    /// <summary>
    /// Base testing class
    /// </summary>
    public class BaseTest
    {
        protected ITestOutputHelper TestOutputHelper { get; }

        protected static IConfiguration Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();

        protected BaseTest(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }
    }
}