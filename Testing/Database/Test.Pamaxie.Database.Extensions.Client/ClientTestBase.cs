using System.Net.Http.Headers;
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
        protected ClientTestBase(DatabaseApiFactory fixture, ITestOutputHelper testOutputHelper) : base(
            testOutputHelper)
        {
            TokenGenerator tokenGenerator = new TokenGenerator(Configuration);
            AuthToken token = tokenGenerator.CreateToken("101963629560135630792");
            DatabaseService service = new DatabaseService(new PamaxieDataContext("http://localhost/", token))
            {
                Service = fixture.CreateClient()
            };
            service.Service.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Token);
        }
    }
}