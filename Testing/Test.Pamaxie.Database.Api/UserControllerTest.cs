using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pamaxie.Api.Controllers;
using Pamaxie.Data;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Database.Api_Test
{
    /// <summary>
    /// Testing class for <see cref="UserController"/>
    /// </summary>
    public sealed class UserControllerTest : ApiTestBase<UserController>
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;

        /// <summary>
        /// <inheritdoc cref="MemberData.AllUnverifiedUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers => MemberData.AllUnverifiedUsers;

        /// <summary>
        /// <inheritdoc cref="MemberData.RandomUsers"/>
        /// </summary>
        public static IEnumerable<object[]> RandomUsers => MemberData.RandomUsers;

        public UserControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            //Instantiate the controller and add a default HttpContext
            Controller = new UserController(Service)
            {
                ControllerContext = { HttpContext = new DefaultHttpContext() }
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
            ActionResult<PamaxieUser> result = Controller.GetTask();

            //Check if user is not null
            PamaxieUser user = ((ObjectResult)result.Result).Value as PamaxieUser;
            Assert.NotNull(user);
        }

        /// <summary>
        /// Test for creating a user <see cref="UserController.CreateTask"/>
        /// </summary>
        /// <param name="userName">The username of the user</param>
        /// <param name="firstName">The firstname of the user</param>
        /// <param name="lastName">The lastname of the user</param>
        /// <param name="emailAddress">The email address of the user</param>
        [Theory]
        [MemberData(nameof(RandomUsers))]
        public void Create(string userName, string firstName, string lastName, string emailAddress)
        {
            PamaxieUser user = new PamaxieUser
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                EmailVerified = false,
                ProfilePictureAddress =
                    "https://lh3.googleusercontent.com/--uodKwFP09o/YTBmgn0JnUI/AAAAAAAAAOw/vPRY_cexRuQnj8du8aFuuqJWn1fZAPW3gCMICGAYYCw/s96-c",
                Disabled = false,
                Deleted = false
            };

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<PamaxieUser> result = Controller.CreateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if user is created
            PamaxieUser createdUser = ((ObjectResult)result.Result).Value as PamaxieUser;
            Assert.NotNull(createdUser);
        }

        /// <summary>
        /// Test for creating a user <see cref="UserController.TryCreateTask"/>
        /// </summary>
        /// <param name="userName">The username of the user</param>
        /// <param name="firstName">The firstname of the user</param>
        /// <param name="lastName">The lastname of the user</param>
        /// <param name="emailAddress">The email address of the user</param>
        [Theory]
        [MemberData(nameof(RandomUsers))]
        public void TryCreate(string userName, string firstName, string lastName, string emailAddress)
        {
            PamaxieUser user = new PamaxieUser
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                EmailVerified = false,
                ProfilePictureAddress =
                    "https://lh3.googleusercontent.com/--uodKwFP09o/YTBmgn0JnUI/AAAAAAAAAOw/vPRY_cexRuQnj8du8aFuuqJWn1fZAPW3gCMICGAYYCw/s96-c",
                Disabled = false,
                Deleted = false
            };

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<PamaxieUser> result = Controller.TryCreateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if user is created
            PamaxieUser createdUser = ((ObjectResult)result.Result).Value as PamaxieUser;
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
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            //Update User
            user.EmailAddress = newEmail;

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<PamaxieUser> result = Controller.UpdateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if user is updated
            PamaxieUser updatedUser = ((ObjectResult)result.Result).Value as PamaxieUser;
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
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            //Update User
            user.EmailAddress = newEmail;

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<PamaxieUser> result = Controller.TryUpdateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if user is updated
            PamaxieUser updatedUser = ((ObjectResult)result.Result).Value as PamaxieUser;
            Assert.NotNull(updatedUser);
            Assert.Equal(newEmail, updatedUser.EmailAddress);
        }

        /// <summary>
        /// Test for creating a user through <see cref="UserController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="userName">The username of the user</param>
        /// <param name="firstName">The firstname of the user</param>
        /// <param name="lastName">The lastname of the user</param>
        /// <param name="emailAddress">The email address of the user</param>
        [Theory]
        [MemberData(nameof(RandomUsers))]
        public void UpdateOrCreate_Create(string userName, string firstName, string lastName, string emailAddress)
        {
            PamaxieUser user = new PamaxieUser
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                EmailVerified = false,
                ProfilePictureAddress =
                    "https://lh3.googleusercontent.com/--uodKwFP09o/YTBmgn0JnUI/AAAAAAAAAOw/vPRY_cexRuQnj8du8aFuuqJWn1fZAPW3gCMICGAYYCw/s96-c",
                Disabled = false,
                Deleted = false
            };

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<PamaxieUser> result = Controller.UpdateOrCreateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if user is updated or created
            PamaxieUser createdUser = ((ObjectResult)result.Result).Value as PamaxieUser;
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
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            //Update User
            user.EmailAddress = newEmail;

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<PamaxieUser> result = Controller.UpdateOrCreateTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if user is updated or created
            PamaxieUser updatedUser = ((ObjectResult)result.Result).Value as PamaxieUser;
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
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
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
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            //Parse the application to a request body and send it to the controller
            Stream body = ControllerService.CreateStream(user);
            Controller.Request.Body = body;

            //Call controller and get result
            ActionResult<IEnumerable<PamaxieApplication>> result = Controller.GetAllApplicationsTask();
            Assert.IsType<OkObjectResult>(result.Result);

            //Check if user is updated or created
            IEnumerable<PamaxieApplication> applications =
                ((ObjectResult)result.Result).Value as IEnumerable<PamaxieApplication>;
            Assert.NotNull(applications);
            foreach (PamaxieApplication application in applications)
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
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
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