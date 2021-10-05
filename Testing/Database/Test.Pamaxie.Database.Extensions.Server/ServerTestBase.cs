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
<<<<<<< HEAD:Testing/Test.Base/Base/ServerBase.cs
        /// Database Context
        /// </summary>
        protected PamaxieDataContext Context { get; }

        /// <summary>
=======
>>>>>>> database-rework:Testing/Database/Test.Pamaxie.Database.Extensions.Server/ServerTestBase.cs
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
<<<<<<< HEAD:Testing/Test.Base/Base/ServerBase.cs
                Context = new PamaxieDataContext(instance, new NetworkCredential(string.Empty, password), reconAttempts);
                Service = new DatabaseService(Context);
=======
                Service = new DatabaseService(ConfigurationOptions.Parse(connectionString));
>>>>>>> database-rework:Testing/Database/Test.Pamaxie.Database.Extensions.Server/ServerTestBase.cs
            }
        }
    }
}