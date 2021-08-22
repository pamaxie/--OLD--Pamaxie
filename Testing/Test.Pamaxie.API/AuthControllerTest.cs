using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Controllers;
using Pamaxie.Api.Data;
using Pamaxie.Api.Security;
using Pamaxie.Data;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.API_UnitTesting
{
    /// <summary>
    /// Testing class for <see cref="AuthController"/>
    /// </summary>
    public class AuthControllerTest : Base.Test
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllApplications"/>
        /// </summary>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;
        
        public AuthControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Login_Succeed(string applicationKey)
        {
            //Get application
            PamaxieApplication application = TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            
            //Mock Database
            SqlDbContext sqlDbContext = MockSqlDbContext.Mock();
            AuthenticationExtensions.DbContext = sqlDbContext;
            
            //Instantiate the controller and add a default HttpContext
            AuthController authController = new(new TokenGenerator(Configuration))
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };

            //Parse the application to a request body and send it to the controller
            Stream body = CreateStream(application);
            authController.Request.Body = body;
            
            ActionResult<AuthToken> result = authController.LoginTask();
            TestOutputHelper.WriteLine(result.Result.ToString());
            //Assert.Equal(, result.Result);
        }
        
        private static Stream CreateStream(object body)
        {            
            MemoryStream ms = new();
            StreamWriter sw = new(ms);
 
            string json = JsonConvert.SerializeObject(body);
 
            sw.Write(json);
            sw.Flush();
 
            ms.Position = 0;
            
            return ms;
        }
    }
}
