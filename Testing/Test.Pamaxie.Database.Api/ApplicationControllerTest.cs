using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Controllers;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Database.Api_Test
{
    /// <summary>
    /// Testing class for <see cref="ApplicationController"/>
    /// </summary>
    public sealed class ApplicationControllerTest : ApiTestBase<ApplicationController>
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllApplications"/>
        /// </summary>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;

        public ApplicationControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Context = new PamaxieDataContext(Instance, Password);
            Service = new DatabaseService(Context);
            //Instantiate the controller and add a default HttpContext
            Controller = new ApplicationController(Service as DatabaseService)
            {
                ControllerContext = { HttpContext = new DefaultHttpContext() }
            };
        }

        /// <summary>
        /// Test for getting a application through <see cref="ApplicationController.GetTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Get(string applicationKey)
        {
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(applicationKey);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = Controller.GetTask();

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
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = Controller.CreateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if application is created
            IPamaxieApplication createdApplication = ((ObjectResult)result.Result).Value as IPamaxieApplication;
            Assert.NotNull(createdApplication);
        }

        /// <summary>
        /// Test for trying to create a application <see cref="ApplicationController.CreateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void TryCreate(string applicationKey)
        {
            //Get application
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = Controller.TryCreateTask();
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
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Update application
            application.ApplicationName = newName;

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = Controller.UpdateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if application is updated
            IPamaxieApplication updatedApplication = ((ObjectResult)result.Result).Value as IPamaxieApplication;
            Assert.NotNull(updatedApplication);
            Assert.Equal(newName, updatedApplication.ApplicationName);
        }

        /// <summary>
        /// Test for trying to update a application through <see cref="ApplicationController.UpdateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void TryUpdate(string applicationKey)
        {
            const string newName = "UpdatedName";

            //Get application
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Update application
            application.ApplicationName = newName;

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = Controller.TryUpdateTask();
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
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = Controller.UpdateOrCreateTask();
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
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Update application
            application.ApplicationName = newName;

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieApplication> result = Controller.UpdateOrCreateTask();
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
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and check if applications is deleted
            ActionResult<bool> result = Controller.DeleteTask();
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)((ObjectResult)result.Result).Value);
        }

        /// <summary>
        /// Test for getting the owner from a application through <see cref="ApplicationController.GetOwner"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void GetOwner(string applicationKey)
        {
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and get the owner of the application
            ActionResult<IPamaxieUser> result = Controller.GetOwner();
            Assert.IsType<OkObjectResult>(result.Result);
            IPamaxieUser owner = ((ObjectResult)result.Result).Value as IPamaxieUser;
            Assert.NotNull(owner);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(owner));
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
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and check if application is enabled or disabled
            ActionResult<IPamaxieApplication> result = Controller.EnableOrDisableTask();
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
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(application);
            Controller.Request.Body = body;

            //Call controller and check if application is verified
            ActionResult<bool> result = Controller.VerifyAuthenticationTask();
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)((ObjectResult)result.Result).Value);
        }
    }
}