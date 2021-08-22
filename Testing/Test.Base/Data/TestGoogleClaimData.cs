using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace Test.Base
{
    /// <summary>
    /// Contains randomly generated Google Claims Data
    /// </summary>
    public static class TestGoogleClaimData
    {
        /// <summary>
        /// List of Google user principle claims.
        /// Used to act as a user being logged into the website.
        /// </summary>
        public static readonly List<Claim[]> ListOfGoogleUserPrincipleClaims = new()
        {
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "108533958726952849891", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Name, "Xystosie", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.GivenName, "Santina", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Surname, "Xystos", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Email, "Saxy@fakemail.com", ClaimValueTypes.String, "Google"),
                new("urn:google:image",
                    "https://lh3.googleusercontent.com/-KHm9C7OfwHE/YRygoiMAYaI/AAAAAAAAAIU/EtHpaiw9rqQiXwbtloPiN-hhxuRRJmJFwCMICGAYYCw/s96-c"
                    , ClaimValueTypes.String, "Google")
            },
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "102617494281791801620", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Name, "Indrakitten", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.GivenName, "Indra", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Surname, "Aronne", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Email, "Inar@fakemail.com", ClaimValueTypes.String, "Google"),
                new("urn:google:image",
                    "https://lh3.googleusercontent.com/-gN0jNA4zEEc/YRyhZ7pvf4I/AAAAAAAAAKo/a0Zi5AZgM4Umg4hWhtGB0bMz8RAt8bKHgCMICGAYYCw/s96-c"
                    , ClaimValueTypes.String, "Google")
            },
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "103932469084294046511", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Name, "Maxster", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.GivenName, "Oshrat", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Surname, "Max", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Email, "Osma@fakemail.com", ClaimValueTypes.String, "Google"),
                new("urn:google:image",
                    "https://lh3.googleusercontent.com/-ZoqFDMxHwW8/YRyhOXy4usI/AAAAAAAAAKI/aYF2Yf-OkgA1A5Q4h2H5Kl1uyEpH3FwsgCMICGAYYCw/s96-c"
                    , ClaimValueTypes.String, "Google")
            },
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "104669818103955818761", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Name, "Paulo", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.GivenName, "Paulus", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Surname, "Ferdinand", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Email, "Pafe@fakemail.com", ClaimValueTypes.String, "Google"),
                new("urn:google:image",
                    "https://lh3.googleusercontent.com/-sWl6t08Q35E/YRyhBB8AlkI/AAAAAAAAAJQ/BO8AYPZlG90gQEND_C_W63fGEnRBvLnhQCMICGAYYCw/s96-c"
                    , ClaimValueTypes.String, "Google")
            },
            new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "101321258707856828644", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Name, "Mana", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.GivenName, "Manuela", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Surname, "Jéssica", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Email, "Maje@fakemail.com", ClaimValueTypes.String, "Google"),
                new("urn:google:image",
                    "https://lh3.googleusercontent.com/-njVNd76BfGQ/YRyhHMcQWRI/AAAAAAAAAJw/dLEOLMKV7Tk8-NrNNRyh5r-lAGd1DPmqwCMICGAYYCw/s96-c"
                    , ClaimValueTypes.String, "Google")
            }
        };

        static TestGoogleClaimData()
        {
            //Check if configuration file exists before adding personal testing data
            if (!File.Exists("appsettings.test.json")) return;
            IConfiguration configuration = new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
            if (configuration.GetChildren().All(_ => _.Key != "UserData")) return;
            IConfigurationSection configurationSection = configuration.GetSection("UserData");

            //Check if the two most important claims exists
            string email = configurationSection.GetValue<string>("Email");

            if (string.IsNullOrEmpty(email)) return;

            //Add the claims to the list of principle claims
            Claim[] claims =
            {
                new(ClaimTypes.NameIdentifier, "101963629560135630792", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Name, "PersonalUser", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.GivenName, "Test", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Surname, "The Tester", ClaimValueTypes.String, "Google"),
                new(ClaimTypes.Email, email, ClaimValueTypes.String, "Google"),
                new("urn:google:image",
                    "https://lh3.googleusercontent.com/-K6jEW8D8F4E/YRy0Zw8i-OI/AAAAAAAAAMQ/pJE0bflfklI1iGnB5zqUspjINcPo1yJ3wCMICGAYYCw/s96-c"
                    , ClaimValueTypes.String, "Google")
            };
            ListOfGoogleUserPrincipleClaims.Add(claims);
        }
    }
}