using Microsoft.Extensions.Configuration;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Database.Extensions.Client_Test
{
    /// <summary>
    /// Testing class for <see cref="UserDataService"/>
    /// </summary>
    public sealed class UserDataServiceTest : TestBase, IClassFixture<DatabaseApiFactory>
    {
        public UserDataServiceTest(DatabaseApiFactory fixture, ITestOutputHelper testOutputHelper) : base(
            testOutputHelper)
        {
            PamaxieDataContext context =
                new PamaxieDataContext("", Configuration.GetSection("AuthData").GetValue<string>("Token"));
            DatabaseService service = new DatabaseService(context)
            {
                Service = fixture.CreateClient()
            };
            //_service.Service.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("IntegrationTest");
            service.Service.DefaultRequestHeaders.Authorization = null;
        }

        // TODO This is only a short test until I find a way to get through the authorization on the fixture Api
        [Fact]
        public void Get()
        {
            PamaxieUser user = UserDataServiceExtension.Get("123");
            Assert.NotNull(user);
        }

        // TODO This is only a short test until I find a way to get through the authorization on the fixture Api
        [Fact]
        public void Create()
        {
            PamaxieUser user = new PamaxieUser()
            {
                Key = "123"
            };
            PamaxieUser createdUser = user.Create();
            Assert.NotNull(createdUser);
        }
    }
}