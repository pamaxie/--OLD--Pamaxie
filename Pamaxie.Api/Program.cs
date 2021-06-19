using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using PamaxieML.Model;
using System;
using System.Collections.Generic;

namespace PamaxieML.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //Testing the prediction engine. We test this very early to ensure everything is working properly.
            try
            {
                Console.WriteLine("We are testing if the neural network works. This may take a minute.");

                var image = ImageProcessing.DownloadFile("https://www.google.com/images/branding/googlelogo/1x/googlelogo_color_272x92dp.png");
                // Add input data
                var input = new ModelInput
                {
                    ImageSource = image.FullName
                };


                // Load model and predict output of sample data
                ConsumeModel.Predict(input, out var labelResult);
                image.Delete();

                Console.WriteLine("Tested neural network successfully. Starting now!");
            }catch(Exception ex)
            {
                Console.WriteLine("Hit an error while testing the Neural Network. Exeting...");
                Console.WriteLine(ex.Message);
                Environment.Exit(502);
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .UseConfiguration(configuration)
                        .UseKestrel()
                        .UseUrls("http://0.0.0.0:80")
                        .UseIISIntegration();
                });
        }
    }
}