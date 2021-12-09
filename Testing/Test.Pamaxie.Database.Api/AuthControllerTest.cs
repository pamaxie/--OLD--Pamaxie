//using Microsoft.AspNetCore.Http;
//using Pamaxie.Api.Controllers;

//using Xunit;
//using Xunit.Abstractions;

//namespace Test.Pamaxie.Database.Api_Test
//{
//    /// <summary>
//    /// Testing class for <see cref="AuthController"/>
//    /// </summary>
//    public sealed class AuthControllerTest : ApiTestBase<AuthController>
//    {
//        public AuthControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
//        {
//            //Instantiate the controller and add a default HttpContext
//            Controller = new AuthController(new TokenGenerator(Configuration), Service)
//            {
//                ControllerContext = { HttpContext = new DefaultHttpContext() }
//            };
//        }

//        /// <summary>
//        /// Test for <see cref="AuthController.LoginTask"/>
//        /// </summary>
//        [Fact] //TODO Needs testing data
//        public void Login()
//        {
//            //TODO Not yet implemented
//        }

//        /// <summary>
//        /// Test for <see cref="AuthController.CreateUserTask"/>
//        /// </summary>
//        [Fact] //TODO Needs testing data
//        public void CreateUser()
//        {
//            //TODO Not yet implemented
//        }

//        /// <summary>
//        /// Test for <see cref="AuthController.RefreshTask"/>
//        /// </summary>
//        [Fact] //TODO Needs testing data
//        public void Refresh()
//        {
//            //TODO Not yet implemented
//        }
//    }
//}