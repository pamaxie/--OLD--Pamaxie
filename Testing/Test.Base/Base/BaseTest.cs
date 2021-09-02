using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Test.TestBase
{
    /// <summary>
    /// Disable tests from running parallel, as this can fail tests using the same collection
    /// </summary>
    [CollectionDefinition(nameof(TestCollectionDefinition), DisableParallelization = true)]
    public class TestCollectionDefinition
    {
    }

    /// <summary>
    /// Base testing class
    /// </summary>
    [Collection(nameof(TestCollectionDefinition))]
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