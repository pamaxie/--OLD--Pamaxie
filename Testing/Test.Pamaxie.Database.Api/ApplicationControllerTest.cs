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
    /// Testing class for <see cref="ApplicationController"/>
    /// </summary>
    public sealed class ApplicationControllerTest : ApiTestBase<ApplicationController>
    {
        /// <inheritdoc cref="MemberData.AllApplicationKeys"/>
        public static IEnumerable<object[]> AllApplicationKeys => MemberData.AllApplicationKeys;

        /// <inheritdoc cref="MemberData.AllApplications"/>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;

        /// <inheritdoc cref="MemberData.RandomApplications"/>
        public static IEnumerable<object[]> RandomApplications => MemberData.RandomApplications;

        public ApplicationControllerTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            //Instantiate the controller and add a default HttpContext
            Controller = new ApplicationController(Service)
            {
                ControllerContext = { HttpContext = new DefaultHttpContext() }
            };
        }

        /// <summary>
        /// Test for getting a <see cref="PamaxieApplication"/> through <see cref="ApplicationController.GetTask"/>
        /// </summary>
        /// <param name="applicationKey">The <see cref="PamaxieApplication"/> key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplicationKeys))]
        public void Get_Succeed(string applicationKey)
        {
            //Act
            ActionResult<PamaxieApplication> result = Controller.GetTask(applicationKey);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieApplication application = GetObjectResultContent(result);
            Assert.NotNull(application);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(application));
        }

        /// <summary>
        /// Test for failing <see cref="ApplicationController.GetTask"/> with no data
        /// </summary>
        [Fact]
        public void Get_Failure_BadRequest()
        {
            //Act
            ActionResult<PamaxieApplication> result = Controller.GetTask(null);

            //Assert
            Assert.Equal(StatusCodes.Status400BadRequest, GetObjectResultStatusCode(result));
            PamaxieApplication application = GetObjectResultContent(result);
            Assert.Null(application);
        }

        /// <summary>
        /// Test for creating a <see cref="PamaxieApplication"/> <see cref="ApplicationController.CreateTask"/>
        /// </summary>
        /// <param name="application">Random test <see cref="PamaxieApplication"/></param>
        [Theory]
        [MemberData(nameof(RandomApplications))]
        public void Create(PamaxieApplication application)
        {
            //Act
            ActionResult<PamaxieApplication> result = Controller.CreateTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status201Created, GetObjectResultStatusCode(result));
            PamaxieApplication createdApplication = GetObjectResultContent(result);
            Assert.NotNull(createdApplication);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(createdApplication));
        }

        /// <summary>
        /// Test for trying to create a <see cref="PamaxieApplication"/> <see cref="ApplicationController.CreateTask"/>
        /// </summary>
        /// <param name="application">Random test <see cref="PamaxieApplication"/></param>
        [Theory]
        [MemberData(nameof(RandomApplications))]
        public void TryCreate(PamaxieApplication application)
        {
            //Act
            ActionResult<PamaxieApplication> result = Controller.TryCreateTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status201Created, GetObjectResultStatusCode(result));
            PamaxieApplication createdApplication = GetObjectResultContent(result);
            Assert.NotNull(createdApplication);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(createdApplication));
        }

        /// <summary>
        /// Test for updating a <see cref="PamaxieApplication"/> through <see cref="ApplicationController.UpdateTask"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Update(PamaxieApplication application)
        {
            //Arrange
            const string newName = "UpdatedName";

            //Act
            application.ApplicationName = newName;
            ActionResult<PamaxieApplication> result = Controller.UpdateTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieApplication updatedApplication = GetObjectResultContent(result);
            Assert.NotNull(updatedApplication);
            Assert.Equal(newName, updatedApplication.ApplicationName);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(updatedApplication));
        }

        /// <summary>
        /// Test for trying to update a <see cref="PamaxieApplication"/> through <see cref="ApplicationController.UpdateTask"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void TryUpdate(PamaxieApplication application)
        {
            //Arrange
            const string newName = "UpdatedName";

            //Act
            application.ApplicationName = newName;
            ActionResult<PamaxieApplication> result = Controller.TryUpdateTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieApplication updatedApplication = GetObjectResultContent(result);
            Assert.NotNull(updatedApplication);
            Assert.Equal(newName, updatedApplication.ApplicationName);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(updatedApplication));
        }

        /// <summary>
        /// Test for creating a <see cref="PamaxieApplication"/> through <see cref="ApplicationController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="application">Random test <see cref="PamaxieApplication"/></param>
        [Theory]
        [MemberData(nameof(RandomApplications))]
        public void UpdateOrCreate_Create(PamaxieApplication application)
        {
            //Act
            ActionResult<PamaxieApplication> result = Controller.UpdateOrCreateTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status201Created, GetObjectResultStatusCode(result));
            PamaxieApplication createdApplication = GetObjectResultContent(result);
            Assert.NotNull(createdApplication);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(createdApplication));
        }

        /// <summary>
        /// Test for updating a <see cref="PamaxieApplication"/> through <see cref="ApplicationController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void UpdateOrCreate_Update(PamaxieApplication application)
        {
            //Arrange
            const string newName = "UpdatedName";

            //Act
            application.ApplicationName = newName;
            ActionResult<PamaxieApplication> result = Controller.UpdateOrCreateTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieApplication updatedApplication = GetObjectResultContent(result);
            Assert.NotNull(updatedApplication);
            Assert.Equal(newName, updatedApplication.ApplicationName);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(updatedApplication));
        }

        /// <summary>
        /// Test for checking if a <see cref="PamaxieApplication"/> exists in the database through <see cref="ApplicationController.ExistsTask"/>
        /// </summary>
        /// <param name="applicationKey">The <see cref="PamaxieApplication"/> key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplicationKeys))]
        public void Exists(string applicationKey)
        {
            //Act
            ActionResult<bool> result = Controller.ExistsTask(applicationKey);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            Assert.True(GetObjectResultContent(result));
        }

        /// <summary>
        /// Test for deleting a <see cref="PamaxieApplication"/> through <see cref="ApplicationController.DeleteTask"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Delete(PamaxieApplication application)
        {
            //Act
            ActionResult<bool> result = Controller.DeleteTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            Assert.True(GetObjectResultContent(result));

            //Add it back, so it will not fail other tests
            Controller.CreateTask(application);
        }

        /// <summary>
        /// Test for getting the owner from a <see cref="PamaxieApplication"/> through <see cref="ApplicationController.GetOwner"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void GetOwner(PamaxieApplication application)
        {
            //Act
            ActionResult<PamaxieUser> result = Controller.GetOwner(application);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieUser owner = GetObjectResultContent(result);
            Assert.NotNull(owner);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(owner));
        }

        /// <summary>
        /// Test for enabling or disabling a <see cref="PamaxieApplication"/> through <see cref="ApplicationController.EnableOrDisableTask"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void EnableOrDisable(PamaxieApplication application)
        {
            //Arrange
            bool disabled = application.Disabled;

            //Act
            ActionResult<PamaxieApplication> result = Controller.EnableOrDisableTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            PamaxieApplication disabledOrEnabledApplication = GetObjectResultContent(result);
            Assert.NotEqual(disabled, disabledOrEnabledApplication.Disabled);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(disabledOrEnabledApplication));
        }

        /// <summary>
        /// Test for verify a <see cref="PamaxieApplication"/> through <see cref="ApplicationController.VerifyAuthenticationTask"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void VerifyAuthentication(PamaxieApplication application)
        {
            //Act
            ActionResult<bool> result = Controller.VerifyAuthenticationTask(application);

            //Assert
            Assert.Equal(StatusCodes.Status200OK, GetObjectResultStatusCode(result));
            Assert.True(GetObjectResultContent(result));
        }
    }
}