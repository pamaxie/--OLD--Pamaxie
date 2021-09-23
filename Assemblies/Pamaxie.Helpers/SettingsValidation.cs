using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Extensions.Server;
using Spectre.Console;

namespace Pamaxie.Helpers
{
    public static class SettingsValidation
    {
        /// <summary>
        /// Validates that the settings exist and are correct
        /// </summary>
        /// <returns><see cref="bool"/> if the settings was successfully validated</returns>
        public static bool ValidateSettings()
        {
            if (!UnitTest.IsRunningFromUnitTests)
            {
                if (!File.Exists("app.configuration"))
                {
                    if (!Console.IsInputRedirected)
                    {
                        if (!AnsiConsole.Confirm(
                            "[red]No app.config could be found. This is required to start the application.[/] Create one now?"))
                        {
                            AnsiConsole.WriteLine(
                                "[red]No configuration exists, please manually or automatically create a configuration before continuing[/]");
                            return false;
                        }
                    }

                    //TODO: Save the settings after this
                    do
                    {
                        Console.Clear();
                    } while (!SettingsGeneration.GenerateConfig());

                    Environment.Exit(-501);
                }
            }

            return true;
        }

        /// <summary>
        /// Validates that the settings for authentication are correct
        /// </summary>
        /// <param name="configuration">Application's configurations</param>
        /// <param name="issue">Issue of the validation if failed</param>
        /// <returns><see cref="bool"/> if the settings was successfully validated</returns>
        public static bool ValidateAuthenticationSettings(IConfiguration configuration, out string issue)
        {
            issue = string.Empty;
            IConfigurationSection authSection = configuration.GetSection("AuthData");

            if (authSection == null)
            {
                issue = "The authentication section ";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates that the settings for the Redis database are correct
        /// </summary>
        /// <param name="configuation">Application's configurations</param>
        /// <param name="databaseService">Newly instantiated <see cref="DatabaseService"/> if succeeded</param>
        /// <returns><see cref="bool"/> if the settings was successfully validated</returns>
        public static bool ValidateRedisSettings(IConfiguration configuation, out DatabaseService databaseService)
        {
            IConfigurationSection dbSection = configuation.GetSection("Redis");
            PamaxieDataContext dataContext = new PamaxieDataContext(
                dbSection.GetValue<string>("Instances"),
                dbSection.GetValue<string>("Password"),
                dbSection.GetValue<int>("ReconAttempts"));
            databaseService = new DatabaseService(dataContext);

            if (!UnitTest.IsRunningFromUnitTests)
            {
                if (databaseService.Connect())
                {
                    AnsiConsole.WriteLine("[red]Cannot connect to the Redis Database[/]");
                    return false;
                }
            }

            return true;
        }
    }
}