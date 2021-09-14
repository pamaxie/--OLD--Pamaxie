using Microsoft.AspNetCore.Mvc;
using Pamaxie.Database.Extensions.Server;
using Xunit.Abstractions;

namespace Test.Base
{
    /// <summary>
    /// Base testing class for Api
    /// </summary>
    /// <typeparam name="T">The Api controller that will be tested against</typeparam>
    public class ApiTestBase<T> : TestBase
    {
        /// <summary>
        /// Database Service
        /// </summary>
        protected DatabaseService Service { get; init; }

        /// <summary>
        /// The Api controller that will be tested against
        /// </summary>
        protected T Controller { get; init; }

        protected ApiTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Service = new DatabaseService(null)
            {
                Service = MockIConnectionMultiplexer.Mock(),
                ConnectionSuccess = true
            };
        }

        protected static TR GetObjectResultContent<TR>(ActionResult<TR> result)
        {
            return result.Result switch
            {
                ObjectResult objectResult => (TR)objectResult.Value,
                _ => default
            };
        }

        protected static int? GetObjectResultStatusCode<TS>(ActionResult<TS> result)
        {
            return result.Result switch
            {
                ObjectResult objectResult => objectResult.StatusCode,
                StatusCodeResult codeResult => codeResult.StatusCode,
                _ => 0
            };
        }
    }
}