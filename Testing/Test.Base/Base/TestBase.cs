using Microsoft.Extensions.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Test.Base
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
    public class TestBase
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

        protected TestBase(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }
    }
}