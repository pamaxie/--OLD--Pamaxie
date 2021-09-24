using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Jwt;
using Spectre.Console;
using StackExchange.Redis;

namespace Pamaxie.Helpers
{
    public static class SettingsGeneration
    {
        /// <summary>
        /// Generates the configuration for the api
        /// </summary>
        /// <returns><see cref="bool"/> if the generation succeeded</returns>
        public static bool GenerateConfig(out dynamic jsonString)
        {
            Rule ruler = new Rule()
            {
                Alignment = Justify.Left,
                Title = "[yellow]Configuration Setup[/]"
            };
            AnsiConsole.Render(ruler);

            dynamic obj = new JObject();

            //TODO use a flag system to say how much should be generated instead of asking the user

            //Authentication Setup
            if (AnsiConsole.Confirm("Would you like to generate a section for authentication?"))
            {
                GenerateAuthenticationSettings(ruler, obj);
            }

            //Redis Setup
            if (AnsiConsole.Confirm("Would you like to generate a section for Redis database?"))
            {
                GenerateRedisDatabaseSettings(ruler, obj);
            }

            //Logging Setup
            dynamic objLogging = new JObject();
            dynamic objLogLevel = new JObject();
            objLogLevel.Default = "Debug";
            objLogLevel.System = "Information";
            objLogLevel.Microsoft = "Information";
            objLogging.LogLevel = objLogLevel;
            obj.Logging = objLogging;

            //Finishing touches
            ruler.Title = "[yellow]Finishing touches[/]";
            AnsiConsole.Render(ruler);
            jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
            AnsiConsole.WriteLine("The generated configuration looks like this:");
            AnsiConsole.Render(new Markup($"[yellow]{jsonString}[/]\n"));
            return AnsiConsole.Confirm(
                "Are you happy with this configuration? (If not we'll guide you through it again from the start)",
                false);
        }

        /// <summary>
        /// Generates the authentication section for configuration file
        /// </summary>
        /// <param name="ruler">Reusable Horizontal ruler for Ansi</param>
        /// <param name="obj"><see cref="JObject"/> containing all sections</param>
        private static void GenerateAuthenticationSettings(Rule ruler, dynamic obj)
        {
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
            else
            {
                secret = TokenGenerator.GenerateSecret();
            }

            if (AnsiConsole.Confirm(
                "Show the set token now? [yellow]It will also be put into the configuration file so u can always see it.[/]",
                false))
            {
                AnsiConsole.WriteLine(secret);
            }

            int timeout = AnsiConsole.Ask(
                "How long in minutes should the timeout for the Jwt bearer be? \n" +
                "[yellow]We usually recommend anywhere between 5 - 15 minutes lifespan[/]", 10);
            dynamic authObj = new JObject();
            authObj.Secret = secret;
            authObj.ExpiresInMinutes = timeout;
            obj.AuthData = authObj;
        }

        /// <summary>
        /// Generates the Redis database section for configuration file
        /// </summary>
        /// <param name="ruler">Reusable Horizontal ruler for Ansi</param>
        /// <param name="obj"><see cref="JObject"/> containing all sections</param>
        private static void GenerateRedisDatabaseSettings(Rule ruler, dynamic obj)
        {
            ruler.Title = "[yellow]Redis setup[/]";
                AnsiConsole.Render(ruler);
                AnsiConsole.WriteLine(
                    "For using this service you require a redis database to be configured and installed on the current system.\n" +
                    "Usually with the provided installer for this software you were instructed to install redis on another server or this one.\n" +
                    "In the following steps we will help you configure these redis databases.\n" +
                    "Please ensure this configuration is correct otherwise we are unable to handle any requests.");
                string setupType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                    .Title("--Please select the type of database you are using--")
                    .PageSize(10)
                    .AddChoices("Local Instance", "Remote Instance"));

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
                    int instances = AnsiConsole.Ask("How many redis servers [red](NOT INSTANCES)[/] do you have?", 1);

                    for (int i = 0; i < instances; i++)
                    {
                        string instanceName = AnsiConsole.Ask<string>("Please tell us your instance address");
                        GenerateServerOptions(instanceName);
                    }

                    AnsiConsole.Render(ruler);
                }
                obj.RedisData = redisObj;
        }

        /// <summary>
        /// Generates the server options for connecting to the redis database
        /// </summary>
        /// <param name="serverAddress">the instance address / name</param>
        /// <returns>The server options</returns>
        private static string GenerateServerOptions(string serverAddress = "redis")
        {
            ConfigurationOptions options = new ConfigurationOptions();
            int port = AnsiConsole.Ask(
                "Whats the port number of the local redis instance?", 6379);

            if (AnsiConsole.Ask("Do you want to use a password for the database (this is HIGHLY recommended)",
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
                            {
                                return ValidationResult.Error(
                                    "[red]The entered password was too short, please ensure its at least 8 Characters.[/]");
                            }

                            return ValidationResult.Success();
                        }));
            }

            int instanceCount = AnsiConsole.Ask("How many redis instances are running locally?", 16);
            int[] instances = new int[instanceCount];

            for (int i = 0; i < instanceCount; i++)
            {
                instances[i] = i;
            }

            List<int> selectedInstances = AnsiConsole.Prompt(
                new MultiSelectionPrompt<int>()
                    .Title("Please select the Redis instances we should use")
                    .Required()
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down to display more instances)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle an instance, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(instances));
            string reconnectionPolicy = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("--Please select the reconnection policy that should be used if the connection is ever lost--")
                .PageSize(10)
                .AddChoices(new[]
                {
                    "Linear", "Exponential"
                }));
            int reconnectionAttempts = AnsiConsole.Ask("How many reconnection attempts should we do?", 5000);

            if (reconnectionPolicy == "Linear")
            {
                options.ReconnectRetryPolicy = new LinearRetry(reconnectionAttempts);
            }
            else
            {
                options.ReconnectRetryPolicy = new ExponentialRetry(reconnectionAttempts);
            }

            for (int i = 0; i < selectedInstances.Count; i++)
            {
                options.EndPoints.Add(serverAddress + selectedInstances[i], port);
            }

            AnsiConsole.WriteLine("The generated redis configuration looks like this:");
            AnsiConsole.Render(new Markup($"[yellow]{options}[/]\n"));
            return options.ToString();
        }
    }
}