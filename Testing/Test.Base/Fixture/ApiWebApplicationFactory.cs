using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace Test.Base
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// Creates the Api in memory with the test configuration file
    /// </summary>
    /// <typeparam name="T">The Startup class from the API you want to test</typeparam>
    public class ApiWebApplicationFactory<T> : WebApplicationFactory<T> where T : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(_ =>
                _.AddConfiguration(new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build()));
            //builder.UseConfiguration(new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build());
        }
    }
}