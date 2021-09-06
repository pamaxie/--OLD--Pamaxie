using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pamaxie.Api;
using Pamaxie.Database.Extensions.Server;

namespace Test.Database.Extensions.Client
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DatabaseApiFactory : WebApplicationFactory<Startup>
    {
        private IConfiguration Configuration { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder
                .UseTestServer()
                .ConfigureServices(services =>
                {
                    services.Configure<TestServer>(options => options.AllowSynchronousIO = true);
                })
                //.ConfigureTestServices(services =>
                //{
                //    services.AddMvc(options => options.Filters.Add(new AllowAnonymousFilter()));
                //})
                .ConfigureAppConfiguration(config =>
                {
                    Configuration = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
                    config.AddConfiguration(Configuration);
                });
        }
    }
}