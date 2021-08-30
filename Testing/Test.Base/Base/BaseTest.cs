using Microsoft.Extensions.Configuration;
using Xunit.Abstractions;

namespace Test.TestBase
{
    /// <summary>
    /// Base testing class
    /// </summary>
    public class BaseTest
    {
        /// <summary>
        /// Provides test output
        /// </summary>
        protected ITestOutputHelper TestOutputHelper { get; }

        /// <summary>
        /// Configurations for testing methods
        /// </summary>
        protected IConfiguration Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();

        protected BaseTest(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }
    }
}