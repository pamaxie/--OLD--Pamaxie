<<<<<<< HEAD
﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Net.Mime;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Api.Controllers;
using Pamaxie.Database.Extensions.Server;
using Pamaxie.Jwt;
using Spectre.Console;
using StackExchange.Redis;

namespace Pamaxie.Api
{
    /// <summary>
    /// Startup class, usually gets called by <see cref="Program"/>
    /// </summary>
    public sealed class Startup
    {
        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="configuration">Configuration to use</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// Validates that the settings exist and are correct
        /// </summary>
        /// <returns></returns>
        private bool ValidateSettings(out string additionalIssue)
        {
            additionalIssue = string.Empty;
            if (!File.Exists("app.configuration"))
            {
                if (!AnsiConsole.Confirm(
                    "[red]No app.config could be found. This is required to start the application.[/] Create one now?"))
                {
                    AnsiConsole.WriteLine(
                        "[red]No configuration exists, please manually or automatically create a configuration before continuing[/]");
                    return false;
                }

                //TODO: Save the settings after this :derp:
                do
                {
                    Console.Clear();
                } while (!GenerateConfig());

                System.Environment.Exit(-501);
            }

            //TODO: Validate Redis connection and that all instances can be connected to
            IConfigurationSection authSection = Configuration.GetSection("AuthData");
            if (authSection == null)
            {
                additionalIssue = "The authentication section ";
                return false;
            }


            return true;
        }

        /// <summary>
        /// Generates the configuration for the api
        /// </summary>
        /// <returns></returns>
        private bool GenerateConfig()
        {
            var ruler = new Rule("[yellow]Configuration Setup[/]");
            ruler.Alignment = Justify.Left;
            AnsiConsole.Render(ruler);
            ruler.Title = "[yellow]Jwt Bearer Setup[/]";
            AnsiConsole.Render(ruler);
            string secret;

            if (!AnsiConsole.Confirm("Do u want to automatically generate a secret for the JWT bearer creation?"))
            {
                secret = AnsiConsole.Prompt(
                    new TextPrompt<string>("Please enter the applications JWT bearer secret. \n" +
                                           "Please ensure its at least 16 Characters in length [yellow](preferably 64)[/]")
                        .Secret()
                        .PromptStyle("red")
                        .Validate(token =>
                        {
                            if (token.Length < 16)
                                return ValidationResult.Error(
                                    "[red]The entered token was too short, please ensure its at least 16 Characters.[/]");
                            return ValidationResult.Success();
                        }));
            }

            secret = TokenGenerator.GenerateSecret();

            if (AnsiConsole.Confirm(
                "Show the set token now? [yellow]It will also be put into the configuration file so u can always see it.[/]",
                false))
                AnsiConsole.WriteLine(secret);

            var timeout = AnsiConsole.Ask<int>(
                "How long in minutes should the timeout for the Jwt bearer be? \n" +
                "[yellow]We usually recommend anywhere between 5 - 15 minutes lifespan[/]", 10);


            dynamic authObj = new JObject();
            authObj.Secret = secret;
            authObj.ExpiresInMinutes = timeout;
            ruler.Title = "[yellow]Redis setup[/]";
            AnsiConsole.Render(ruler);
            AnsiConsole.WriteLine(
                "For using this service you require a redis database to be configured and installed on the current system.\n" +
                "Usually with the provided installer for this software you were instructed to install redis on another server or this one.\n" +
                "In the following steps we will help you configure these redis databases.\n" +
                "Please ensure this configuration is correct otherwise we are unable to handle any requests.");
            var setupType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("--Please select the type of database you are using--")
                .PageSize(10)
                .AddChoices(new[]
                {
                        "Local Instance", "Remote Instance"
                }));

            dynamic redisObj = new JObject();
            if (setupType == "Local Instance")
            {
                ruler.Title = "[yellow]Local Instance setup[/]";
                AnsiConsole.Render(ruler);
                redisObj = GenerateServerOptions();
            }
            else
            {
                //TODO: Finish generating configurations for multiple servers (have a peek at the documentation if you wanna know how to do this)
                ruler.Title = "[yellow]Remote Instance setup[/]";
                int instances = AnsiConsole.Ask<int>("How many redis servers [red](NOT INSTANCES)[/] do you have?", 1);
                for (int i = 0; i < instances; i++)
                {
                    string instanceName = AnsiConsole.Ask<string>("Please tell us your instance address");
                    GenerateServerOptions(instanceName);
                }

                AnsiConsole.Render(ruler);
            }


            ruler.Title = "[yellow]Finishing touches[/]";
            AnsiConsole.Render(ruler);
            dynamic obj = new JObject();
            obj.AuthData = authObj;
            obj.RedisData = redisObj;
            var jsonString = Newtonsoft.Json.JsonConvert.SerializeObject(obj, Formatting.Indented);
            AnsiConsole.WriteLine("The generated configuration looks like this:");
            AnsiConsole.Render(new Markup($"[yellow]{jsonString}[/]\n"));
            return AnsiConsole.Ask(
                "Are you happy with this configuration? (If not we'll guide you through it again from the start)", false);
        }

        /// <summary>
        /// Generates the server options for connecting to the redis database
        /// </summary>
        /// <param name="serverAddress">the instance address / name</param>
        /// <returns></returns>
        private string GenerateServerOptions(string serverAddress = "redis")
        {
            ConfigurationOptions options = new ConfigurationOptions();
            
            var port = AnsiConsole.Ask<int>(
                "Whats the port number of the local redis instance?", 6379);

            if (AnsiConsole.Ask<bool>("Do you want to use a password for the database (this is HIGHLY recommended)",
                true))
            {
                options.Password = AnsiConsole.Prompt(
                    new TextPrompt<string>("Please enter your database password. " +
                                           "[yellow]We do [red]NOT[/] accept passwords with a length of shorter than 8 characters[/], these can be too easily brute forced with redis.")
                        .Secret()
                        .PromptStyle("red")
                        .Validate(token =>
                        {
                            if (token.Length < 8)
                                return ValidationResult.Error(
                                    "[red]The entered password was too short, please ensure its at least 8 Characters.[/]");
                            return ValidationResult.Success();
                        }));
            }

            var instanceCount = AnsiConsole.Ask<int>("How many redis instances are running locally?", 16);
            int[] instances = new int[instanceCount];

            for (int i = 0; i < instanceCount; i++)
            {
                instances[i] = i;
            }

            var selectedInstances = AnsiConsole.Prompt(
                new MultiSelectionPrompt<int>()
                    .Title("Please select the Redis instances we should use")
                    .Required()
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to display more instances)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle an instance, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(instances));
            var reconnectionPolicy = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("--Please select the reconnection policy that should be used if the connection is ever lost--")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Linear", "Exponential"
                }));
            var reconnectionAttempts = AnsiConsole.Ask<int>("How many reconnection attempts should we do?", 5000);

            if (reconnectionPolicy == "Linear")
            {
                options.ReconnectRetryPolicy = new LinearRetry(reconnectionAttempts);
            }
            else
            {
                options.ReconnectRetryPolicy = new ExponentialRetry(reconnectionAttempts);
            }

            foreach (var instance in selectedInstances)
            {
                options.EndPoints.Add(serverAddress + instance, port);
            }

            AnsiConsole.WriteLine("The generated redis configuration looks like this:");
            AnsiConsole.Render(new Markup($"[yellow]{options}[/]\n"));
            return options.ToString();
        }

        /// <summary>
        /// Gets called by the runtime to add Services to the container
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add</param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (!ValidateSettings(out var issue))
            {
                Console.WriteLine(
                    "The applications configuration was in an incorrect or unaccepted format. The detailed problem was: \n" +
                    issue);
                //TODO: add status code 501 to the list of status codes as "wrong configuration"
                System.Environment.Exit(-501);
            }

            services.AddControllers();
            IConfigurationSection authSection = Configuration.GetSection("AuthData");
            byte[] key = Encoding.ASCII.GetBytes(authSection.GetValue<string>("Secret"));
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            IConfigurationSection dbSection = Configuration.GetSection("Redis");
            PamaxieDataContext dataContext = new PamaxieDataContext(
                dbSection.GetValue<string>("Instances"),
                dbSection.GetValue<string>("Password"),
                dbSection.GetValue<int>("ReconAttempts"));

            services.AddSingleton(new DatabaseService(dataContext));
            services.AddTransient<TokenGenerator>();
        }

        /// <summary>
        /// Gets called by the runtime to configure HTTP Services
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> Builder</param>
        /// <param name="env">Hosting Environment</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
=======
﻿using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Pamaxie.Database.Extensions.Server;
using Pamaxie.Helpers;
using Pamaxie.Jwt;

namespace Pamaxie.Api
{
    /// <summary>
    /// Startup class, usually gets called by <see cref="Program"/>
    /// </summary>
    public sealed class Startup
    {
        /// <summary>
        /// Initializer
        /// </summary>
        /// <param name="configuration">Configuration to use</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        /// <summary>
        /// Gets called by the runtime to add Services to the container
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> to add</param>
        public void ConfigureServices(IServiceCollection services)
        {
            string issue = string.Empty;
            DatabaseService databaseService = null;
            if (!SettingsValidation.ValidateSettings() ||
                !SettingsValidation.ValidateAuthenticationSettings(Configuration, out issue) ||
                !SettingsValidation.ValidateRedisSettings(Configuration, out issue, out databaseService))
            {
                Console.WriteLine(
                    "The applications configuration was in an incorrect or unaccepted format. The detailed problem was: \n" +
                    issue);
                //TODO: add status code 501 to the list of status codes as "wrong configuration"
                Environment.Exit(-501);
            }

            services.AddControllers();
            IConfigurationSection authSection = Configuration.GetSection("AuthData");
            byte[] key = Encoding.ASCII.GetBytes(authSection.GetValue<string>("Secret"));
            services
                .AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    };
                });

            services.AddSingleton(databaseService);
            services.AddTransient<TokenGenerator>();
        }

        /// <summary>
        /// Gets called by the runtime to configure HTTP Services
        /// </summary>
        /// <param name="app"><see cref="IApplicationBuilder"/> Builder</param>
        /// <param name="env">Hosting Environment</param>
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
>>>>>>> database-rework
}