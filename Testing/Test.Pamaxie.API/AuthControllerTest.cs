using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pamaxie.Api.Controllers;
using Pamaxie.Data;
using Pamaxie.Jwt;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.API_Test
{
    /// <summary>
    /// Testing class for <see cref="AuthController"/>
    /// </summary>
    public sealed class AuthControllerTest : ApiTestBase<AuthController>
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllApplications"/>
        /// </summary>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;

        public AuthControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            //Mock ApplicationDataService, for ApplicationDataServiceExtension
            MockApplicationDataService.Mock();
            //Instantiate the controller and add a default HttpContext
            Controller = new AuthController(new TokenGenerator(Configuration))
            {
                ControllerContext = { HttpContext = new DefaultHttpContext() }
            };
        }

        /// <summary>
        /// Test for login of a application
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Login(string applicationKey)
        {
            //Get application
            PamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            ActionResult<AuthToken> result = Controller.LoginTask();
            Assert.IsType<OkObjectResult>(result.Result);
        }

        /// <summary>
        /// Test refreshing an exiting oAuth Token through <see cref="AuthController.RefreshTask"/>
        /// </summary>
        [Fact]
        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}