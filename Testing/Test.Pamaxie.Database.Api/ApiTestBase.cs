using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Extensions.Server;
using StackExchange.Redis;
using Test.Base;
using Xunit.Abstractions;

namespace Test.Pamaxie.Database.Api_Test
{
    /// <summary>
    /// Base testing class for Database.Api
    /// </summary>
    /// <typeparam name="T">The Api controller that will be tested against</typeparam>
    public class ApiTestBase<T> : TestBase
    {
        /// <summary>
        /// Database Service
        /// </summary>
        protected DatabaseService Service { get; }

        /// <summary>
        /// The Api controller that will be tested against
        /// </summary>
        protected T Controller { get; init; }

        protected ApiTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            IConfigurationSection dbConfigSection = Configuration.GetSection("RedisData");
            string connectionString = dbConfigSection.GetValue<string>("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                Service = new DatabaseService(null)
                {
                    Service = MockIConnectionMultiplexer.Mock(),
                    ConnectionSuccess = true
                };
            }
            else
            {
                Service = new DatabaseService(ConfigurationOptions.Parse(connectionString));
            }
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