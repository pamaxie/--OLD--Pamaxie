using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pamaxie.Api.Controllers;
using Pamaxie.Api.Security;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Api.DirectControllerCall
{
    /// <summary>
    /// Testing class for <see cref="ApplicationController"/>
    /// </summary>
    public class ApplicationControllerTest : Base.Test
    {
        private readonly PamaxieDataContext _context = new("", "");
        
        /// <summary>
        /// <inheritdoc cref="MemberData.AllApplications"/>
        /// </summary>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;
        
        public ApplicationControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Test for getting a application through <see cref="ApplicationController.GetTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Get(string applicationKey)
        {
            //Instantiate the controller and add a default HttpContext
            ApplicationController applicationController = new(new TokenGenerator(Configuration), _context)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(applicationKey);
            applicationController.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = applicationController.GetTask();
            
            //Check if application is not null
            IPamaxieApplication application = ((ObjectResult)result.Result).Value as IPamaxieApplication;
            Assert.NotNull(application);
        }
        
        /// <summary>
        /// Test for creating a application <see cref="ApplicationController.CreateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Create(string applicationKey)
        {
            //Get application
            IPamaxieApplication application = TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            
            //Instantiate the controller and add a default HttpContext
            ApplicationController applicationController = new(new TokenGenerator(Configuration), _context)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            applicationController.Request.Body = body;
            
            //Call controller and get result
            ActionResult<IPamaxieApplication> result = applicationController.CreateTask();
            Assert.IsType<OkObjectResult>(result.Result);
            
            //Check if application is created
            IPamaxieApplication createdApplication = ((ObjectResult)result.Result).Value as IPamaxieApplication;
            Assert.NotNull(createdApplication);
        }
        
        /// <summary>
        /// Test for updating a application through <see cref="ApplicationController.UpdateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Update(string applicationKey)
        {
            const string newName = "UpdatedName";
            
            //Get application
            IPamaxieApplication application = TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            
            //Update application
            application.ApplicationName = newName;
            
            //Instantiate the controller and add a default HttpContext
            ApplicationController applicationController = new(new TokenGenerator(Configuration), _context)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            applicationController.Request.Body = body;
            
            //Call controller and get result
            ActionResult<IPamaxieApplication> result = applicationController.UpdateTask();
            Assert.IsType<OkObjectResult>(result.Result);
            
            //Check if application is updated
            IPamaxieApplication updatedApplication = ((ObjectResult)result.Result).Value as IPamaxieApplication;
            Assert.NotNull(updatedApplication);
            Assert.Equal(newName, updatedApplication.ApplicationName);
        }
        
        /// <summary>
        /// Test for creating a application through <see cref="ApplicationController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void UpdateOrCreate_Create(string applicationKey)
        {
            //Get application
            IPamaxieApplication application = TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Instantiate the controller and add a default HttpContext
            ApplicationController applicationController = new(new TokenGenerator(Configuration), _context)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            applicationController.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = applicationController.UpdateOrCreateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if application is updated or created
            IPamaxieApplication createdApplication = ((ObjectResult)result.Result).Value as IPamaxieApplication;
            Assert.NotNull(createdApplication);
        }
        
        /// <summary>
        /// Test for updating a application through <see cref="ApplicationController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void UpdateOrCreate_Update(string applicationKey)
        {
            const string newName = "UpdatedName";
            
            //Get application
            IPamaxieApplication application = TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            
            //Update application
            application.ApplicationName = newName;

            //Instantiate the controller and add a default HttpContext
            ApplicationController applicationController = new(new TokenGenerator(Configuration), _context)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            applicationController.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = applicationController.UpdateOrCreateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if application is updated or created
            IPamaxieApplication updatedApplication = ((ObjectResult)result.Result).Value as IPamaxieApplication;
            Assert.NotNull(updatedApplication);
            Assert.Equal(newName, updatedApplication.ApplicationName);
        }
        
        /// <summary>
        /// Test for deleting a application through <see cref="ApplicationController.DeleteTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Delete(string applicationKey)
        {
            //Get application
            IPamaxieApplication application = TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Instantiate the controller and add a default HttpContext
            ApplicationController applicationController = new(new TokenGenerator(Configuration), _context)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            applicationController.Request.Body = body;

            //Call controller and check if applications is deleted
            ActionResult<IPamaxieApplication> result = applicationController.DeleteTask();
            Assert.IsType<OkObjectResult>(result.Result);
        }
        
        /// <summary>
        /// Test for getting all applications from a application through <see cref="ApplicationController.EnableOrDisableTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void EnableOrDisable(string applicationKey)
        {
            //Get application
            IPamaxieApplication application = TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Instantiate the controller and add a default HttpContext
            ApplicationController applicationController = new(new TokenGenerator(Configuration), _context)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            applicationController.Request.Body = body;

            //Call controller and check if application is enabled or disabled
            ActionResult<IPamaxieApplication> result = applicationController.EnableOrDisableTask();
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(!application.Disabled, ((IPamaxieApplication)((ObjectResult)result.Result).Value).Disabled);
        }

        /// <summary>
        /// Test for verify a application through <see cref="ApplicationController.VerifyAuthenticationTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void VerifyAuthentication(string applicationKey)
        {
            //Get application
            IPamaxieApplication application = TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            
            //Instantiate the controller and add a default HttpContext
            ApplicationController applicationController = new(new TokenGenerator(Configuration), _context)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            applicationController.Request.Body = body;

            //Call controller and check if application is verified
            ActionResult<bool> result = applicationController.VerifyAuthenticationTask();
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)((ObjectResult)result.Result).Value);
        }
    }
}