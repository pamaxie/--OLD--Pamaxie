using Pamaxie.Database.Extensions.Server;
using Xunit.Abstractions;

namespace Test.TestBase
{
    /// <summary>
    /// Base testing class for Api
    /// </summary>
    /// <typeparam name="T">The Api controller that will be tested against</typeparam>
    public class ApiBaseTest<T> : BaseTest
    {
        /// <summary>
        /// Database Context
        /// </summary>
        protected PamaxieDataContext Context { get; }

        /// <summary>
        /// The Api controller that will be tested against
        /// </summary>
        protected T Controller { get; init; }

        protected ApiBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Context = new PamaxieDataContext("", "");
        }
    }
}