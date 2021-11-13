using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Database.Design;
using Spectre.Console;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Database.Server
{
    internal class PamaxieDatabaseConfig : IPamaxieDatabaseConfiguration
    {
        PamaxieDatabaseDriver owner;
        ConfigurationOptions configurationOptions;


        /// <summary>
        /// Creates a new instance of the Pamaxie Database config for Redis
        /// </summary>
        /// <param name="owner"></param>
        internal PamaxieDatabaseConfig(PamaxieDatabaseDriver owner)
        {
            this.owner = owner;
        }

        /// <inheritdoc/>
        public Guid DatabaseDriverGuid { get => owner.DatabaseTypeGuid; }

        /// <inheritdoc/>
        public string GenerateConfig()
        {
            var ruler = new Rule(@"[yellow]Redis setup[/]");
            ruler.Alignment = Justify.Left;
            AnsiConsole.Render(ruler);
            AnsiConsole.WriteLine(
                        @"For using this service you require a redis database to be configured and installed on the current system.
                        Usually with the provided installer for this software you were instructed to install redis on another server or this one.
                        In the following steps we will help you configure these redis databases.
                        Please ensure this configuration is correct otherwise we are unable to handle any requests.");

            var setupType = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title(@"--Please select the database enviorement that you are using--")
                .AddChoices(new[]{@"Local Instance", @"Remote Instance"}));

            dynamic redisObj = new JObject();

            string selectedRedisConfig = string.Empty;
            if (setupType == @"Local Instance")
            {
                ruler.Title = @"[yellow]Local Instance setup[/]";
                AnsiConsole.Render(ruler);
                selectedRedisConfig = GenerateServerOptions();
            }
            else
            {
                //TODO: Finish generating configurations for multiple servers (have a peek at the documentation if you wanna know how to do this)
                ruler.Title = @"[yellow]Remote Instance setup[/]";
                int instances = AnsiConsole.Ask<int>(@"How many redis servers [red](NOT INSTANCES)[/] do you have?", 1);
                for (int i = 0; i < instances; i++)
                {
                    string instanceName = AnsiConsole.Ask<string>(@"Please tell us your instance address");
                    GenerateServerOptions(instanceName);
                    //TODO: we still need to save the server options here somehow.
                }

                AnsiConsole.Render(ruler);
            }

            //PLEASE MAKE SURE TO SET THESE EXACTLY LIKE THIS IF YOU BUILD YOUR OWN DRIVER
            redisObj.DatabaseDriverGuid = DatabaseDriverGuid;
            redisObj.Settings = selectedRedisConfig;
            return JsonConvert.SerializeObject(redisObj);
        }

        /// <inheritdoc/>
        public void LoadConfig(string config)
        {
            configurationOptions = ConfigurationOptions.Parse(config);
        }

        /// <summary>
        /// Using the ToString to return this items string representation.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            dynamic options = new JObject();
            options.Guid = this.DatabaseDriverGuid;
            options.DatabaseConfig = configurationOptions.ToString();

            return JsonConvert.SerializeObject(options, Formatting.Indented);
        }


        /// <summary>
        /// Generates the server options for connecting to the redis database
        /// </summary>
        /// <param name="serverAddress">the instance address / name</param>
        /// <returns></returns>
        private static string GenerateServerOptions(string serverAddress = "redis")
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
    }
}
