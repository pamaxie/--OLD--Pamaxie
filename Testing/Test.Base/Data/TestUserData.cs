using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Pamaxie.Data;
using Pamaxie.Data.Interfaces;

namespace Test.Base
{
    /// <summary>
    /// Contains randomly generated User Data
    /// </summary>
    public static class TestUserData
    {
        /// <summary>
        /// List of Users for the SqlDbContext.
        /// </summary>
        public static readonly List<IPamaxieUser> ListOfUsers = new()
        {
            new PamaxieUser
            {
                Key = "108533958726952849891",
                UserName = "Xystosie",
                EmailAddress = "Saxy@fakemail.com",
                EmailVerified = false,
                Deleted = false
            },
            new PamaxieUser
            {
                Key = "102617494281791801620",
                UserName = "Indrakitten",
                EmailAddress = "Inar@fakemail.com",
                EmailVerified = true,
                Deleted = false
            },
            new PamaxieUser
            {
                Key = "103932469084294046511",
                UserName = "Maxster",
                EmailAddress = "Osma@fakemail.com",
                EmailVerified = false,
                Deleted = false
            },
            new PamaxieUser
            {
                Key = "104669818103955818761",
                UserName = "Paulo",
                EmailAddress = "Pafe@fakemail.com",
                EmailVerified = true,
                Deleted = false
            },
            new PamaxieUser
            {
                Key = "101321258707856828644",
                UserName = "Mana",
                EmailAddress = "Maje@fakemail.com",
                EmailVerified = false,
                Deleted = false
            }
        };

        static TestUserData()
        {
            //Check if configuration file exists before adding personal testing data
            if (File.Exists("appsettings.test.json"))
            {
                IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
                if (configuration.GetChildren().Any(_ => _.Key == "UserData"))
                {
                    IConfigurationSection configurationSection = configuration.GetSection("UserData");

                    //Check if a email is given
                    string email = configurationSection.GetValue<string>("Email");

                    if (!string.IsNullOrEmpty(email))
                    {
                        //Add the user to the list of users
                        PamaxieUser pamaxieUser = new()
                        {
                            Key = "101963629560135630792",
                            UserName = "PersonalUser",
                            EmailAddress = email,
                            EmailVerified = false,
                            Deleted = false
                        };
                        ListOfUsers.Add(pamaxieUser);
                        return;
                    }
                }
            }
            ListOfUsers.Add(new PamaxieUser()
            {
                Key = "101963629560135630792",
                UserName = "",
                EmailAddress = "",
                EmailVerified = false,
                Deleted = true
            });
        }
    }
}