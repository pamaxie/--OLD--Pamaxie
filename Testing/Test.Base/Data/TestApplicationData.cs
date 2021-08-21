using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Pamaxie.Data;

namespace Test.Base
{
    public class TestApplicationData
    {
        /// <summary>
        /// List of Users for the SqlDbContext.
        /// </summary>
        public static readonly List<Application> ListOfApplications = new()
        {
            new Application()
            {
                ApplicationId = 2,
                AppToken = "Apple",
                UserId = 2,
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
            },
            new Application()
            {
                ApplicationId = 3,
                AppToken = "Pie",
                UserId = 2,
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
            },
            new Application()
            {
                ApplicationId = 4,
                AppToken = "Orange",
                UserId = 4,
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
            },
            new Application()
            {
                ApplicationId = 5,
                AppToken = "Pear",
                UserId = 4,
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
            },
            new Application()
            {
                ApplicationId = 6,
                AppToken = "Cake",
                UserId = 4,
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
            }
        };

        static TestApplicationData()
        {
            //Check if configuration file exists before adding personal testing data
            if (File.Exists("appsettings.test.json"))
            {
                IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
                if (configuration.GetChildren().Any(_ => _.Key == "ApplicationData"))
                {
                    IConfigurationSection configurationSection = configuration.GetSection("ApplicationData");

                    //Check if a email is given
                    string appToken = configurationSection.GetValue<string>("AppToken");

                    if (!string.IsNullOrEmpty(appToken))
                    {
                        //Add the user to the list of users
                        Application application = new()
                        {
                            ApplicationId = 1,
                            AppToken = appToken,
                            UserId = 1,
                            ApplicationName = "PersonalApplication",
                            LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                            RateLimited = false,
                            Disabled = false,
                            Deleted = false
                        };
                        ListOfApplications.Add(application);
                        return;
                    }
                }
            }
            ListOfApplications.Add(new Application()
            {
                ApplicationId = 1,
                AppToken = "qwerty",
                UserId = 1,
                ApplicationName = "PersonalApplication",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = true
            });
        }
    }
}