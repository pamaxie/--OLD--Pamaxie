using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Pamaxie.Data;

namespace Test.Pamaxie.Website
{
    internal static class TestUserData
    {
        private static IConfiguration Configuration { get; } =
            new ConfigurationBuilder().AddJsonFile("appsettings.test.json").Build();

        internal static readonly IQueryable<User> Users = new List<User>
        {
            new()
            {
                GoogleUserId = Configuration.GetSection("ProfileData").GetValue<string>("GoogleClaimUserId"),
                Id = Configuration.GetSection("ProfileData").GetValue<int>("Id"),
                Username = "TestUser",
                Email = Configuration.GetSection("ProfileData").GetValue<string>("Email"),
                EmailVerified = false,
                DeletedAccount = false
            }
        }.AsQueryable();
    }
}