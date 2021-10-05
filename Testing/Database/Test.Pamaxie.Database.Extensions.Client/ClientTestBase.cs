using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Pamaxie.Database.Extensions.Client;
using Pamaxie.Jwt;
using Test.Base;
using Xunit.Abstractions;

namespace Test.Pamaxie.Database.Extensions.Client_Test
{
    /// <summary>
    /// Base testing class for Database.Client
    /// </summary>
    public class ClientTestBase : TestBase
    {
        protected ClientTestBase(ITestOutputHelper testOutputHelper) : base(
            testOutputHelper)
        {
            IConfigurationSection apiDataSection = Configuration.GetSection("ApiData");
            string instance = apiDataSection.GetValue<string>("Instance");
            TokenGenerator tokenGenerator = new TokenGenerator(Configuration);
            AuthToken token = tokenGenerator.CreateToken("101963629560135630792");
            DatabaseService service = new DatabaseService(new PamaxieDataContext(instance, token))
            {
                Service = DatabaseApiFactory.CreateFactory(Configuration).CreateClient()
            };
            service.Service.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Token);
        }
    }
}