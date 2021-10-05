using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pamaxie.Api;
using Pamaxie.Database.Extensions.Server;

namespace Test.Base
{
    /// <summary>
    /// Class for creating the ApiFactory
    /// </summary>
    internal static class DatabaseApiFactory
    {
        /// <summary>
        /// Creates the Api Factory using the Database.Api.Startup
        /// </summary>
        /// <param name="configuration">Configuration file to use for the Database.Api</param>
        /// <returns>Created <see cref="WebApplicationFactory{TEntryPoint}"/></returns>
        internal static WebApplicationFactory<Startup> CreateFactory(IConfiguration configuration)
        {
            WebApplicationFactory<Startup> factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.UseTestServer()
                    .ConfigureServices(services =>
                    {
<<<<<<< Updated upstream:Testing/Database/Test.Pamaxie.Database.Extensions.Client/DatabaseApiFactory.cs
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
=======
                        Service = MockIConnectionMultiplexer.Mock(),
                        IsConnected = true
                    });
                })
                .ConfigureAppConfiguration(config =>
                {
                    Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
                    config.AddConfiguration(Configuration);
                });
>>>>>>> Stashed changes:Testing/Database/Test.Pamaxie.Database.Extensions.Client/Factory/DatabaseApiFactory.cs
        }
    }
}