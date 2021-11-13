using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Database.Extensions;
using Pamaxie.Jwt;
using Spectre.Console;
using System;
using System.IO;
using System.Linq;

namespace Pamaxie.Database.Api
{
    /// <summary>
    /// Configuration setup for the Database Api
    /// </summary>
    public class ApiApplicationConfiguration
    {
        /// <summary>
        /// Validates that the settings exist and are correct
        /// </summary>
        /// <returns></returns>
        public static bool ValidateConfiguration(IConfiguration configuration, out string additionalIssue)

        {
            var ruler = new Rule("[orange]Pamaxie Database API[/]");
            ruler.Alignment = Justify.Left;
            AnsiConsole.Render(ruler);

            ruler = new Rule("Settings validation");
            AnsiConsole.Render(ruler);

            additionalIssue = string.Empty;
            if (!File.Exists("app.configuration"))
            {
                if (!AnsiConsole.Confirm("[orange]No app.config could be found. This is required to start the application.[/] Create one now?"))
                {
                    AnsiConsole.WriteLine(
                        "[red]No configuration exists, please manually or automatically create a configuration before continuing. Startup failed.[/]");
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
            IConfigurationSection authSection = configuration.GetSection("AuthData");
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
        private static bool GenerateConfig()
        {
            Console.Clear();
            string jwtBearerConfig = GenerateJwtConfig();
            string databaseConnectionString = GenerateDatabaseConfig();
            Console.Clear();
            var ruler = new Rule("[yellow]Configuration Setup[/]");
            ruler.Alignment = Justify.Left;
            AnsiConsole.Render(ruler);
            ruler.Title = "[yellow]Finishing touches[/]";
            AnsiConsole.Render(ruler);
            dynamic obj = new JObject();
            obj.AuthData = jwtBearerConfig;
            obj.Database = databaseConnectionString;
            var jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
            AnsiConsole.WriteLine("The generated configuration looks like this:");
            AnsiConsole.Render(new Markup($"[yellow]{jsonString}[/]\n"));
            return AnsiConsole.Ask(
                "Are you happy with this configuration? (If not we'll guide you through it again from the start)", false);
        }

        /// <summary>
        /// This generates the Jwt Bearer configuration that the API should use
        /// </summary>
        /// <returns></returns>
        private static string GenerateJwtConfig()
        {
            var ruler = new Rule("[yellow]Configuration Setup[/]");
            ruler.Alignment = Justify.Left;
            AnsiConsole.Render(ruler);
            ruler.Title = "[yellow]Jwt Bearer Setup[/]";
            AnsiConsole.Render(ruler);
            string secret;

            if (!AnsiConsole.Confirm("Do you want to automatically generate a secret for the JWT bearer creation?"))
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

            if (AnsiConsole.Confirm("Show the set token now? [yellow]It will also be put into the configuration file so u can always see it.[/]", 
                                    false))
            {
                AnsiConsole.WriteLine(secret);
            }

            var timeout = AnsiConsole.Ask<int>(
                "How long in minutes should the timeout for the Jwt bearer be? \n" +
                "[yellow]We usually recommend anywhere between 5 - 15 minutes lifespan[/]", 10);

            dynamic authObj = new JObject();
            authObj.Secret = secret;
            authObj.ExpiresInMinutes = timeout;

            return JsonConvert.SerializeObject(authObj);
        }

        /// <summary>
        /// This does some magic to automatically detect the different database drivers available and then tries to generate a config for them.
        /// </summary>
        /// <returns></returns>
        private static string GenerateDatabaseConfig()
        {
            var ruler = new Rule("[yellow]Configuration Setup[/]");
            ruler.Alignment = Justify.Left;
            AnsiConsole.Render(ruler);
            ruler.Title = "[blue]Setting up the database[/]";
            AnsiConsole.Render(ruler);

            var drivers = DbDriverManager.LoadDatabaseDrivers();
            var driverNames = drivers.Select(x => $"{x.DatabaseTypeName} ;; \0{x.DatabaseTypeGuid}").ToArray();

            var selectedDatabaseDriver = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Please select which database you want to use")
                            .PageSize(10)
                            .MoreChoicesText("[grey](Move up and down to show more database drivers)[/]")
                            .AddChoices(driverNames));

            var guid = new Guid(selectedDatabaseDriver.Split('\0').LastOrDefault());
            var driver = drivers.FirstOrDefault(x => x.DatabaseTypeGuid == guid);
            return driver.Configuration.GenerateConfig();
        }
    }
}
