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

        /// <summary>
        /// Database Instance
        /// </summary>
        private string Instance { get; }

        /// <summary>
        /// Database Password
        /// </summary>
        private string Password { get; }

        protected ServerBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            //TODO Change these section name, and value names when the appsettings.json is done for Database.Api
            IConfigurationSection dbConfigSection = Configuration.GetSection("DbConfig");
            Instance = dbConfigSection.GetValue<string>("Instances");
            Password = dbConfigSection.GetValue<string>("Password");

            if (string.IsNullOrEmpty(Instance) || string.IsNullOrEmpty(Password))
            {
                Service = new DatabaseService(null)
                {
                    Service = MockIConnectionMultiplexer.Mock()
                };
            }
            else
            {
                Context = new PamaxieDataContext(Instance, Password);
                Service = new DatabaseService(Context);
            }
        }
    }
}