using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Pamaxie.Api
{
    /// <summary>
    /// BUG: This is just here for future proofing. This is a concept idea to have the data flow over this api instead of the ML Apis
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Main Function
        /// </summary>
        /// <param name="args">Args to be passed</param>
        [SuppressMessage("NDepend", "ND2500:DontCreateThreadsExplicitly", Justification = "TODO")]
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create our Host builder
        /// </summary>
        /// <param name="args">Startup Args</param>
        /// <returns><see cref="IHostBuilder"/> for the API</returns>
        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().AddCommandLine(args).Build();
            string hostUrl = configuration["hosturl"];
            if (string.IsNullOrEmpty(hostUrl)) hostUrl = "http://0.0.0.0:6000";
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseConfiguration(configuration)
                        .UseKestrel()
                        .UseUrls("http://0.0.0.0:5003", "http://localhost:5004")
                        .UseIISIntegration();
                });
        }
    }
}