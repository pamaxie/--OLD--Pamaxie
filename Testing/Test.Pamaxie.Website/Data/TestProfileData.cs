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
            Id = Configuration.GetSection("ProfileData").GetValue<int>("Id"),
            GoogleClaimUserId = Configuration.GetSection("ProfileData").GetValue<string>("GoogleClaimUserId"),
            EmailAddress = Configuration.GetSection("ProfileData").GetValue<string>("Email"),
            UserName = "Hello",
            ProfilePictureAddress = "",
            Deleted = false
        };
    }
}