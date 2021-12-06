using Microsoft.AspNetCore.Mvc;
using Test.Base;
using Xunit.Abstractions;

namespace Test.Pamaxie.API_Test
{
    /// <summary>
    /// Base testing class for Api
    /// </summary>
    /// <typeparam name="T">The Api controller that will be tested against</typeparam>
    public class ApiTestBase<T> : TestBase
    {
        /// <summary>
        /// The Api controller that will be tested against
        /// </summary>
        protected T Controller { get; init; }

        protected ApiTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
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