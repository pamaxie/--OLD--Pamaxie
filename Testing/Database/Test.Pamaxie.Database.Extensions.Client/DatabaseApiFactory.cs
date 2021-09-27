using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pamaxie.Api;
using Pamaxie.Database.Extensions.Server;

namespace Test.Base
{
    /// <summary>
    /// 
    /// </summary>
    internal static class DatabaseApiFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        /// <returns></returns>
        internal static WebApplicationFactory<Startup> CreateFactory(IConfiguration configuration)
        {
            WebApplicationFactory<Startup> factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.UseTestServer()
                    .ConfigureServices(services =>
                    {
                        services.Configure<TestServer>(options => options.AllowSynchronousIO = true);

                        services.AddSingleton(new DatabaseService(null)
                        {
                            Service = MockIConnectionMultiplexer.Mock(),
                            ConnectionSuccess = true
                        });
                    })
                    .ConfigureAppConfiguration((_, config) => { config.AddConfiguration(configuration); });
            });
            return factory;
        }
    }
}