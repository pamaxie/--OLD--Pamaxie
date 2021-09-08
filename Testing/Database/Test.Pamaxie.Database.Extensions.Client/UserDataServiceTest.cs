using Microsoft.Extensions.Configuration;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// Testing class for <see cref="UserDataService"/>
    /// </summary>
    public class UserDataServiceTest : TestBase, IClassFixture<DatabaseApiFactory>
    {
        public UserDataServiceTest(DatabaseApiFactory fixture, ITestOutputHelper testOutputHelper) : base(
            testOutputHelper)
        {
            PamaxieDataContext context = new("", Configuration.GetSection("AuthData").GetValue<string>("Token"));
            DatabaseService service = new(context)
            {
                Service = fixture.CreateClient()
            };
            //_service.Service.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("IntegrationTest");
            service.Service.DefaultRequestHeaders.Authorization = null;
        }

        [Fact]
        public void Get()
        {
            IPamaxieUser user = UserDataServiceExtension.Get("123");
            Assert.NotNull(user);
        }

        [Fact]
        public void Create()
        {
            IPamaxieUser user = new PamaxieUser()
            {
                Key = "123"
            };
            IPamaxieUser createdUser = user.Create();
            Assert.NotNull(createdUser);
        }
    }
}