using Pamaxie.Api.Controllers;
using Pamaxie.Database.Extensions.Server;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Api.DirectControllerCall
{
    /// <summary>
    /// Testing class for <see cref="AuthController"/>
    /// </summary>
    public class AuthControllerTest : Base.Test
    {
        private readonly PamaxieDataContext _context = new("", "");
        
        public AuthControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
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