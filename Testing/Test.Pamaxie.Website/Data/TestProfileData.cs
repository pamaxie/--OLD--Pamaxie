using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Extensions.Sql.Data;

namespace Test.Pamaxie.Website
{
    internal static class TestProfileData
    {
        private static IConfiguration Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        
        internal static ProfileData Profile { get; } = new()
        {
            Id = 1,
            GoogleClaimUserId = "101963629560135630792",
            EmailAddress = Configuration.GetSection("ProfileData").GetValue<string>("Email"),
            UserName = "PersonalUser",
            ProfilePictureAddress = "https://lh3.googleusercontent.com/-K6jEW8D8F4E/YRy0Zw8i-OI/AAAAAAAAAMQ/pJE0bflfklI1iGnB5zqUspjINcPo1yJ3wCMICGAYYCw/s96-c",
            Deleted = false
        };
    }
}