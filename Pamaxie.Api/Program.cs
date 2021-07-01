using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PamaxieML.Model;

namespace Pamaxie.Api
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            //Testing the prediction engine. We test this very early to ensure everything is working properly.
            try
            {
                Console.WriteLine("We are testing if the neural network works. This may take a minute.");

                var image = ImageProcessing.ImageProcessing.DownloadFile("https://cdn.discordapp.com/attachments/777425987914039296/858793909945368616/snes9x-x64.exe");
                // Add input data
                ModelInput input = new()
                {
                    ImageSource = image?.FullName
                };

                // Load model and predict output of sample data
                ConsumeModel.Predict(input, out OutputProperties labelResult);
                image?.Delete();

                Console.WriteLine("Tested neural network successfully. Starting now!");
            }catch(Exception ex)
            {
                Console.WriteLine("Hit an error while testing the Neural Network. Exiting...");
                Console.WriteLine(ex.Message);
                Environment.Exit(502);
            }

            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseConfiguration(configuration)
                        .UseKestrel()
                        .UseUrls("http://0.0.0.0:8080")
                        .UseIISIntegration();
                });
        }
    }
}