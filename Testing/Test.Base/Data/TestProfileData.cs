using Microsoft.Extensions.Configuration;
using Pamaxie.Data;
using Pamaxie.Data.Interfaces;

namespace Test.Pamaxie.Website
{
    public static class TestProfileData
    {
        private static IConfiguration Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();
        
        public static IPamaxieUser PamaxieUser { get; } = new PamaxieUser()
        {
            Key = "101963629560135630792",
            EmailAddress = Configuration.GetSection("UserData").GetValue<string>("Email"),
            UserName = "PersonalUser",
            ProfilePictureAddress = "https://lh3.googleusercontent.com/-K6jEW8D8F4E/YRy0Zw8i-OI/AAAAAAAAAMQ/pJE0bflfklI1iGnB5zqUspjINcPo1yJ3wCMICGAYYCw/s96-c",
            Deleted = false
        };
    }
}