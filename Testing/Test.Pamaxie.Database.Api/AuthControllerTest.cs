using Microsoft.AspNetCore.Http;
using Pamaxie.Api.Controllers;
using Pamaxie.Database.Extensions.Server;
using Pamaxie.Jwt;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Database.Api_Test
{
    /// <summary>
    /// Testing class for <see cref="AuthController"/>
    /// </summary>
    public class AuthControllerTest : ApiTestBase<AuthController>
    {
        public AuthControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Context = new PamaxieDataContext(Instance, Password);
            Service = new DatabaseService(Context);
            //Instantiate the controller and add a default HttpContext
            Controller = new AuthController(new TokenGenerator(Configuration), Service as DatabaseService)
            {
                ControllerContext = { HttpContext = new DefaultHttpContext() }
            };
        }

        /// <summary>
        /// Test for getting a application through <see cref="AuthController.LoginTask"/>
        /// </summary>
        [Fact] //TODO Needs testing data
        public void Login()
        {
            //TODO Not yet implemented
        }

        /// <summary>
        /// Test for getting a application through <see cref="AuthController.CreateUserTask"/>
        /// </summary>
        [Fact] //TODO Needs testing data
        public void CreateUser()
        {
            //TODO Not yet implemented
        }

        /// <summary>
        /// Test for getting a application through <see cref="AuthController.RefreshTask"/>
        /// </summary>
        [Fact] //TODO Needs testing data
        public void Refresh()
        {
            //TODO Not yet implemented
        }
    }
}