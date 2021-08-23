using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Pamaxie.Website
{
    /// <summary>
    /// Class for the Main entry point
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main entry point for the application
        /// </summary>
        /// <param name="args">Launch Parameters</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseKestrel()
                        .UseUrls("http://0.0.0.0");
                });
    }
}
