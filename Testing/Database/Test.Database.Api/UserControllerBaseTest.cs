using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Controllers;
using Pamaxie.Api.Security;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;
using Test.TestBase;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Api
{
    /// <summary>
    /// Testing class for <see cref="UserController"/>
    /// </summary>
    public class UserControllerBaseTest : ApiBaseTest<UserController>
    {
        private readonly PamaxieDataContext _context = new("", "");
        
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;
        
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUnverifiedUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers => MemberData.AllUnverifiedUsers;
        
        public UserControllerBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            //Instantiate the controller and add a default HttpContext
            Controller = new UserController(new TokenGenerator(Configuration), Context)
            {
                ControllerContext = {HttpContext = new DefaultHttpContext()}
            };
        }

        /// <summary>
        /// Test for getting a user through <see cref="UserController.GetTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Get(string userKey)
        {
            //Parse the user to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(userKey);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieUser> result = Controller.GetTask();
            
            //Check if user is not null
            IPamaxieUser user = ((ObjectResult)result.Result).Value as IPamaxieUser;
            Assert.NotNull(user);
        }
        
        /// <summary>
        /// Test for creating a user <see cref="UserController.CreateTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Create(string userKey)
        {
            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;
            
            //Call controller and get result
            ActionResult<IPamaxieUser> result = Controller.CreateTask();
            Assert.IsType<OkObjectResult>(result.Result);
            
            //Check if user is created
            IPamaxieUser createdUser = ((ObjectResult)result.Result).Value as IPamaxieUser;
            Assert.NotNull(createdUser);
        }
        
        /// <summary>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void TryCreate(string userKey)
        {
            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;
            
            //Call controller and get result
            ActionResult<IPamaxieUser> result = Controller.TryCreateTask();
            Assert.IsType<OkObjectResult>(result.Result);
            
            //Check if user is created
            IPamaxieUser createdUser = ((ObjectResult)result.Result).Value as IPamaxieUser;
            Assert.NotNull(createdUser);
        }
        
        /// <summary>
        /// Test for updating a user through <see cref="UserController.UpdateTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Update(string userKey)
        {
            const string newEmail = "UpdatedEmail@testmail.com";
            
            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            
            //Update User
            user.EmailAddress = newEmail;
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieUser> result = Controller.UpdateTask();
            Assert.IsType<OkObjectResult>(result.Result);
            
            //Check if user is updated
            IPamaxieUser updatedUser = ((ObjectResult)result.Result).Value as IPamaxieUser;
            Assert.NotNull(updatedUser);
            Assert.Equal(newEmail, updatedUser.EmailAddress);
        }
        
        /// <summary>
        /// Test for trying to update a user through <see cref="UserController.UpdateTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void TryUpdate(string userKey)
        {
            const string newEmail = "UpdatedEmail@testmail.com";
            
            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            
            //Update User
            user.EmailAddress = newEmail;
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieUser> result = Controller.TryUpdateTask();
            Assert.IsType<OkObjectResult>(result.Result);
            
            //Check if user is updated
            IPamaxieUser updatedUser = ((ObjectResult)result.Result).Value as IPamaxieUser;
            Assert.NotNull(updatedUser);
            Assert.Equal(newEmail, updatedUser.EmailAddress);
        }
        
        /// <summary>
        /// Test for creating a user through <see cref="UserController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void UpdateOrCreate_Create(string userKey)
        {
            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieUser> result = Controller.UpdateOrCreateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if user is updated or created
            IPamaxieUser createdUser = ((ObjectResult)result.Result).Value as IPamaxieUser;
            Assert.NotNull(createdUser);
        }
        
        /// <summary>
        /// Test for updating a user through <see cref="UserController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void UpdateOrCreate_Update(string userKey)
        {
            const string newEmail = "UpdatedEmail@testmail.com";
            
            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            
            //Update User
            user.EmailAddress = newEmail;

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IPamaxieUser> result = Controller.UpdateOrCreateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if user is updated or created
            IPamaxieUser updatedUser = ((ObjectResult)result.Result).Value as IPamaxieUser;
            Assert.NotNull(updatedUser);
            Assert.Equal(newEmail, updatedUser.EmailAddress);
        }
        
        /// <summary>
        /// Test for deleting a user through <see cref="UserController.DeleteTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Delete(string userKey)
        {
            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and check if user is deleted
            ActionResult<bool> result = Controller.DeleteTask();
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)((ObjectResult)result.Result).Value);
        }
        
        /// <summary>
        /// Test for getting all applications from a user through <see cref="UserController.GetAllApplicationsTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void GetAllApplications(string userKey)
        {
            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IEnumerable<IPamaxieApplication>> result = Controller.GetAllApplicationsTask();
            Assert.IsType<OkObjectResult>(result.Result);
            
            //Check if user is updated or created
            IEnumerable<IPamaxieApplication> applications = ((ObjectResult)result.Result).Value as IEnumerable<IPamaxieApplication>;
            Assert.NotNull(applications);
            foreach (IPamaxieApplication application in applications)
            {
                string applicationStr = JsonConvert.SerializeObject(application);
                TestOutputHelper.WriteLine(applicationStr);
            }
        }

        /// <summary>
        /// Test for verify a user through <see cref="UserController.VerifyEmailTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void VerifyEmail(string userKey)
        {
            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            
            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and check if user is verified
            ActionResult<bool> result = Controller.VerifyEmailTask();
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)((ObjectResult)result.Result).Value);
        }
    }
}