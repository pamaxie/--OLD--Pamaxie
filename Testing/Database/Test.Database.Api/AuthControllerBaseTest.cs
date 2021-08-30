using Microsoft.AspNetCore.Http;
using Pamaxie.Api.Controllers;
using Pamaxie.Api.Security;
using Test.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Api
{
    /// <summary>
    /// Testing class for <see cref="AuthController"/>
    /// </summary>
    public class AuthControllerBaseTest : ApiBaseTest<AuthController>
    {
        public AuthControllerBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            //Instantiate the controller and add a default HttpContext
            Controller = new AuthController(new TokenGenerator(Configuration))
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
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