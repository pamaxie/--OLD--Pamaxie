using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pamaxie.Api.Controllers;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
using Pamaxie.Data;
using Test.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.API_UnitTesting
{
    /// <summary>
    /// Testing class for <see cref="AuthController"/>
    /// </summary>
    public class AuthControllerBaseTest : BaseTest
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllApplications"/>
        /// </summary>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;

        public AuthControllerBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Test for login of a application
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Login_Succeed(string applicationKey)
        {
            //Get application
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Mock ApplicationDataService, for ApplicationDataServiceExtension
            MockApplicationDataService.Mock();

            //Instantiate the controller and add a default HttpContext
            AuthController authController = new(new TokenGenerator(Configuration))
            {
                ControllerContext = { HttpContext = new DefaultHttpContext() }
            };

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            authController.Request.Body = body;

            ActionResult<AuthToken> result = authController.LoginTask();
            Assert.IsType<OkObjectResult>(result.Result);
        }
    }
}