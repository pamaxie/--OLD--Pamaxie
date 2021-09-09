using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Design;
using StackExchange.Redis;
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
        /// Database Context
        /// </summary>
        protected IPamaxieDataContext Context { get; init; }

        /// <summary>
        /// Database Service
        /// </summary>
        protected IDatabaseService<IConnectionMultiplexer> Service { get; init; }

        /// <summary>
        /// Database Instance
        /// </summary>
        protected string Instance { get; }

        /// <summary>
        /// Database Password
        /// </summary>
        protected string Password { get; }

        /// <summary>
        /// The Api controller that will be tested against
        /// </summary>
        protected T Controller { get; init; }

        protected ApiTestBase(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            //TODO Change these section name, and value names when the appsettings.json is done for Database.Api
            IConfigurationSection dbConfigSection = Configuration.GetSection("DbConfig");
            Instance = dbConfigSection.GetValue<string>("Instances");
            Password = dbConfigSection.GetValue<string>("Password");
        }
    }
}