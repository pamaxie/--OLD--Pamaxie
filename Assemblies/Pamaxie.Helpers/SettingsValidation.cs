using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Extensions.Server;
using Spectre.Console;
using StackExchange.Redis;

namespace Pamaxie.Helpers
{
    public static class SettingsValidation
    {
        private const string ConfigurationFileName = "app.configuration.json"; //TODO Do we wanna change our configuration file name to this, or appsettings.json?

        /// <summary>
        /// Validates that the settings exist and are correct
        /// </summary>
        /// <returns><see cref="bool"/> if the settings was successfully validated</returns>
        public static bool ValidateSettings()
        {
            if (!UnitTest.IsRunningFromUnitTests)
            {
                if (!File.Exists(ConfigurationFileName))
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

                    dynamic jsonString;

                    do
                    {
                        Console.Clear();
                    } while (!SettingsGeneration.GenerateConfig(out jsonString));

                    File.WriteAllText(ConfigurationFileName, jsonString);

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
                issue = "The authentication section is missing";
                return false;
            }

            if (string.IsNullOrEmpty(authSection.GetValue<string>("Secret")))
            {
                issue = "No secret provided";
                return false;
            }

            if (string.IsNullOrEmpty(authSection.GetValue<string>("ExpiresInMinutes")))
            {
                issue = "No expiration time provided";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validates that the settings for the Redis database are correct
        /// </summary>
        /// <param name="configuation">Application's configurations</param>
        /// <param name="issue">Issue of the validation if failed</param>
        /// <param name="databaseService">Newly instantiated <see cref="DatabaseService"/> if succeeded</param>
        /// <returns><see cref="bool"/> if the settings was successfully validated</returns>
        public static bool ValidateRedisSettings(IConfiguration configuation, out string issue, out DatabaseService databaseService)
        {
            databaseService = null;
            issue = string.Empty;
            IConfigurationSection dbSection = configuation.GetSection("Redis");
            string connectionString = dbSection.GetValue<string>("ConnectionString");

            if (dbSection == null)
            {
                issue = "The redis database section is missing";
                return false;
            }

            databaseService = new DatabaseService(ConfigurationOptions.Parse(connectionString));

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