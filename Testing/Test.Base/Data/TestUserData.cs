using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Pamaxie.Data;

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
        public static readonly List<User> ListOfUsers = new()
        {
            new User
            {
                GoogleUserId = "108533958726952849891",
                Id = 2,
                Username = "Xystosie",
                Email = "Saxy@fakemail.com",
                EmailVerified = false,
                DeletedAccount = false
            },
            new User
            {
                GoogleUserId = "102617494281791801620",
                Id = 3,
                Username = "Indrakitten",
                Email = "Inar@fakemail.com",
                EmailVerified = true,
                DeletedAccount = false
            },
            new User
            {
                GoogleUserId = "103932469084294046511",
                Id = 4,
                Username = "Maxster",
                Email = "Osma@fakemail.com",
                EmailVerified = false,
                DeletedAccount = false
            },
            new User
            {
                GoogleUserId = "104669818103955818761",
                Id = 5,
                Username = "Paulo",
                Email = "Pafe@fakemail.com",
                EmailVerified = true,
                DeletedAccount = false
            },
            new User
            {
                GoogleUserId = "101321258707856828644",
                Id = 6,
                Username = "Mana",
                Email = "Maje@fakemail.com",
                EmailVerified = false,
                DeletedAccount = false
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
                        User user = new()
                        {
                            GoogleUserId = "101963629560135630792",
                            Id = 1,
                            Username = "PersonalUser",
                            Email = email,
                            EmailVerified = false,
                            DeletedAccount = false
                        };
                        ListOfUsers.Add(user);
                        return;
                    }
                }
            }
            ListOfUsers.Add(new User()
            {
                GoogleUserId = "101963629560135630792",
                Id = 1,
                Username = "",
                Email = "",
                EmailVerified = false,
                DeletedAccount = true
            });
        }
    }
}