using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Extensions.Server;
using Xunit.Abstractions;

namespace Test.Base
{
    /// <summary>
    /// Base testing class for Database.Server
    /// </summary>
    public class ServerBase : TestBase
    {
        /// <summary>
        /// Database Context
        /// </summary>
        private PamaxieDataContext Context { get; }

        /// <summary>
        /// Database Service
        /// </summary>
        protected DatabaseService Service { get; }

        protected ServerBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            IConfigurationSection dbConfigSection = Configuration.GetSection("RedisData");
            string instance = dbConfigSection.GetValue<string>("Instances");
            string password = dbConfigSection.GetValue<string>("Password");
            int reconAttempts = dbConfigSection.GetValue<int>("ReconAttempts");

            if (string.IsNullOrEmpty(instance) || string.IsNullOrEmpty(password) || reconAttempts == default)
            {
                Service = new DatabaseService(null)
                {
                    Service = MockIConnectionMultiplexer.Mock()
                };
            }
            else
            {
                Context = new PamaxieDataContext(instance, password, reconAttempts);
                Service = new DatabaseService(Context);
            }
        }
    }
}