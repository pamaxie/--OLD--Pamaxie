using System.Collections.Generic;
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
        /// <inheritdoc cref="MemberData.AllUserKeys"/>
        public static IEnumerable<object[]> AllUserKeys => MemberData.AllUserKeys;

        /// <inheritdoc cref="MemberData.AllUsers"/>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;

        /// <inheritdoc cref="MemberData.AllUnverifiedUsers"/>
        public static IEnumerable<object[]> AllUnverifiedUsers => MemberData.AllUnverifiedUsers;

        /// <inheritdoc cref="MemberData.RandomUsers"/>
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
        /// Test for getting a <see cref="PamaxieUser"/> through <see cref="UserController.GetTask"/>
        /// </summary>
        /// <param name="userKey">The <see cref="PamaxieUser"/> key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUserKeys))]
        public void Get(string userKey)
        {
            //Act
            ActionResult<PamaxieUser> result = Controller.GetTask(userKey);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieUser user = GetObjectResultContent(result);
            Assert.NotNull(user);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(user));
        }

        /// <summary>
        /// Test for failing <see cref="UserController.GetTask"/> with no data
        /// </summary>
        [Fact]
        public void Get_Failure_BadRequest()
        {
            //Act
            ActionResult<PamaxieUser> result = Controller.GetTask(null);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, GetObjectResultStatusCode(result));
            PamaxieUser user = GetObjectResultContent(result);
            Assert.Null(user);
        }

        /// <summary>
        /// Test for creating a <see cref="PamaxieUser"/> <see cref="UserController.CreateTask"/>
        /// </summary>
        /// <param name="user">Random test <see cref="PamaxieUser"/></param>
        [Theory]
        [MemberData(nameof(RandomUsers))]
        public void Create(PamaxieUser user)
        {
            //Act
            ActionResult<PamaxieUser> result = Controller.CreateTask(user);

            //Assert
            Assert.Equal(StatusCodes.Status201Created, GetObjectResultStatusCode(result));
            PamaxieUser createdUser = GetObjectResultContent(result);
            Assert.NotNull(createdUser);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(createdUser));
        }

        /// <summary>
        /// Test for creating a <see cref="PamaxieUser"/> <see cref="UserController.TryCreateTask"/>
        /// </summary>
        /// <param name="user">Random test <see cref="PamaxieUser"/></param>
        [Theory]
        [MemberData(nameof(RandomUsers))]
        public void TryCreate(PamaxieUser user)
        {
            //Act
            ActionResult<PamaxieUser> result = Controller.TryCreateTask(user);

            //Assert
            Assert.Equal(StatusCodes.Status201Created, GetObjectResultStatusCode(result));
            PamaxieUser createdUser = GetObjectResultContent(result);
            Assert.NotNull(createdUser);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(createdUser));
        }

        /// <summary>
        /// Test for updating a <see cref="PamaxieUser"/> through <see cref="UserController.UpdateTask"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Update(PamaxieUser user)
        {
            //Arrange
            const string newEmail = "UpdatedEmail@testmail.com";

            //Act
            user.EmailAddress = newEmail;
            ActionResult<PamaxieUser> result = Controller.UpdateTask(user);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieUser updatedUser = GetObjectResultContent(result);
            Assert.NotNull(updatedUser);
            Assert.Equal(newEmail, updatedUser.EmailAddress);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(updatedUser));
        }

        /// <summary>
        /// Test for trying to update a <see cref="PamaxieUser"/> through <see cref="UserController.UpdateTask"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void TryUpdate(PamaxieUser user)
        {
            //Arrange
            const string newEmail = "UpdatedEmail@testmail.com";

            //Act
            user.EmailAddress = newEmail;
            ActionResult<PamaxieUser> result = Controller.TryUpdateTask(user);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieUser updatedUser = GetObjectResultContent(result);
            Assert.NotNull(updatedUser);
            Assert.Equal(newEmail, updatedUser.EmailAddress);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(updatedUser));
        }

        /// <summary>
        /// Test for creating a <see cref="PamaxieUser"/> through <see cref="UserController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="user">Random test <see cref="PamaxieUser"/></param>
        [Theory]
        [MemberData(nameof(RandomUsers))]
        public void UpdateOrCreate_Create(PamaxieUser user)
        {
            //Act
            ActionResult<PamaxieUser> result = Controller.UpdateOrCreateTask(user);

            //Assert
            Assert.Equal(StatusCodes.Status201Created, GetObjectResultStatusCode(result));
            PamaxieUser createdUser = GetObjectResultContent(result);
            Assert.NotNull(createdUser);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(createdUser));
        }

        /// <summary>
        /// Test for updating a <see cref="PamaxieUser"/> through <see cref="UserController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void UpdateOrCreate_Update(PamaxieUser user)
        {
            //Arrange
            const string newEmail = "UpdatedEmail@testmail.com";

            //Act
            user.EmailAddress = newEmail;
            ActionResult<PamaxieUser> result = Controller.UpdateOrCreateTask(user);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieUser updatedUser = GetObjectResultContent(result);
            Assert.NotNull(updatedUser);
            Assert.Equal(newEmail, updatedUser.EmailAddress);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(updatedUser));
        }

        /// <summary>
        /// Test for checking if a <see cref="PamaxieUser"/> exists in the database through <see cref="UserController.ExistsTask"/>
        /// </summary>
        /// <param name="userKey">The <see cref="PamaxieUser"/> key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUserKeys))]
        public void Exists(string userKey)
        {
            //Act
            ActionResult<bool> result = Controller.ExistsTask(userKey);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            Assert.True(GetObjectResultContent(result));
        }

        /// <summary>
        /// Test for deleting a <see cref="PamaxieUser"/> through <see cref="UserController.DeleteTask"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Delete(PamaxieUser user)
        {
            //Act
            ActionResult<bool> result = Controller.DeleteTask(user);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            Assert.True(GetObjectResultContent(result));

            //Add it back, so it will not fail other tests
            Controller.CreateTask(user);
        }

        /// <summary>
        /// Test for getting all <see cref="PamaxieApplication"/>s from a <see cref="PamaxieUser"/>
        /// through <see cref="UserController.GetAllApplicationsTask"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void GetAllApplications(PamaxieUser user)
        {
            //Act
            ActionResult<IEnumerable<PamaxieApplication>> result = Controller.GetAllApplicationsTask(user);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            IEnumerable<PamaxieApplication> applications = GetObjectResultContent(result);
            Assert.NotNull(applications);

            foreach (PamaxieApplication application in applications)
            {
                string applicationStr = JsonConvert.SerializeObject(application);
                TestOutputHelper.WriteLine(applicationStr);
            }
        }

        /// <summary>
        /// Test for verify a <see cref="PamaxieUser"/> through <see cref="UserController.VerifyEmailTask"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void VerifyEmail(PamaxieUser user)
        {
            //Act
            Assert.NotNull(user);
            ActionResult<bool> result = Controller.VerifyEmailTask(user);

            //Assert
            Assert.IsType<OkObjectResult>(result.Result);
            Assert.True((bool)((ObjectResult)result.Result).Value);
        }
    }
}