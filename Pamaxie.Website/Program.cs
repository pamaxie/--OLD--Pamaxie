using System;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pamaxie.Database.Extensions;

namespace Pamaxie.Website
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseKestrel()
                        .UseUrls("http://0.0.0.0");
                });
    }
}
