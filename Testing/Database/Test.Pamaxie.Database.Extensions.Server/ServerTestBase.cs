using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Extensions.Server;
using StackExchange.Redis;
using Test.Base;
using Xunit.Abstractions;

namespace Test.Database.Extensions.Server_Test
{
    /// <summary>
    /// Base testing class for Database.Server
    /// </summary>
    public class ServerBase : TestBase
    {
        /// <summary>
        /// Database Service
        /// </summary>
        protected DatabaseService Service { get; }

        protected ServerBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            IConfigurationSection dbConfigSection = Configuration.GetSection("RedisData");
            string connectionString = dbConfigSection.GetValue<string>("ConnectionString");

            if (string.IsNullOrEmpty(connectionString))
            {
                Service = new DatabaseService(null)
                {
                    Service = MockIConnectionMultiplexer.Mock()
                };
            }
            else
            {
                Service = new DatabaseService(ConfigurationOptions.Parse(connectionString));
            }
        }
    }
}