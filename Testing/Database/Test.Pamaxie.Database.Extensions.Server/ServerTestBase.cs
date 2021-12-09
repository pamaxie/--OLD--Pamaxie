using System.Net;
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
        /// Database Context
        /// </summary>
        protected string Context { get; }

        /// <summary>
        /// Database Service
        /// </summary>
        protected DatabaseService Service { get; }

        protected ServerBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            IConfigurationSection dbConfigSection = Configuration.GetSection("RedisData");
            string connectionString = dbConfigSection.GetValue<string>("ConnectionString");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                Service = new DatabaseService(null)
                {
                    Service = MockIConnectionMultiplexer.Mock()
                };
            }
            else
            {
                Context = connectionString;
                Service = new DatabaseService(ConfigurationOptions.Parse(connectionString));
            }
        }
    }
}