using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Pamaxie.Api;
using Pamaxie.Api.Controllers;
using Pamaxie.Database.Extensions.Server;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Api.HttpClientControllerCall
{
    /// <summary>
    /// Testing class for <see cref="AuthController"/>
    /// </summary>
    public class AuthControllerTest : Base.Test, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly PamaxieDataContext _context;
        private readonly HttpClient _client;
        
        public AuthControllerTest(ITestOutputHelper testOutputHelper, WebApplicationFactory<Startup> fixture) : base(testOutputHelper)
        {
            _context = new PamaxieDataContext("", ""); //TODO get instance and password from Configuration
            _client = fixture.CreateClient();
        }
        
        /// <summary>
        /// Test for getting a application through <see cref="AuthController.LoginTask"/>
        /// </summary>
        [Fact]
        public void Login()
        {
            //TODO Not yet implemented
        }

        /// <summary>
        /// Test for getting a application through <see cref="AuthController.CreateUserTask"/>
        /// </summary>
        [Fact]
        public void CreateUser()
        {
            //TODO Not yet implemented
        }

        /// <summary>
        /// Test for getting a application through <see cref="AuthController.RefreshTask"/>
        /// </summary>
        [Fact]
        public void Refresh()
        {
            //TODO Not yet implemented
        }
    }
}