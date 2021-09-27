using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Controllers;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
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
            DatabaseService.ApplicationService = MockApplicationDataService.Mock();
            //Instantiate the controller and add a default HttpContext
            Controller = new AuthController(new TokenGenerator(Configuration))
            {
                ControllerContext = { HttpContext = new DefaultHttpContext() }
            };
        }

        /// <summary>
        /// Test for login of a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Login(PamaxieApplication application)
        {
            //Act
            ActionResult<AuthToken> result = Controller.LoginTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            AuthToken token = GetObjectResultContent(result);
            Assert.NotNull(token);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(token));
        }

        /// <summary>
        /// Test refreshing an exiting <see cref="AuthToken"/> through <see cref="AuthController.RefreshTask"/>
        /// </summary>
        [Fact]
        public void Refresh()
        {
            throw new NotImplementedException();
        }
    }
}