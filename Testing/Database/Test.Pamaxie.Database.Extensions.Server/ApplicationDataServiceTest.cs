using System.Collections.Generic;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Extensions.Server_Test
{
    /// <summary>
    /// Testing class for <see cref="ApplicationDataService"/>
    /// </summary>
    public sealed class ApplicationDataServiceTest : ServerBase
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllApplicationKeys"/>
        /// </summary>
        public static IEnumerable<object[]> AllApplicationKeys => MemberData.AllApplicationKeys;

        /// <summary>
        /// <inheritdoc cref="MemberData.AllApplications"/>
        /// </summary>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;

        /// <summary>
        /// <inheritdoc cref="MemberData.RandomApplications"/>
        /// </summary>
        public static IEnumerable<object[]> RandomApplications => MemberData.RandomApplications;

        public ApplicationDataServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Get a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="applicationKey">The key of the <see cref="PamaxieApplication"/></param>
        [Theory]
        [MemberData(nameof(AllApplicationKeys))]
        public void Get(string applicationKey)
        {
            //Act
            PamaxieApplication application = Service.Applications.Get(applicationKey);

            //Assert
            Assert.NotNull(application);
            string str = JsonConvert.SerializeObject(application);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Create a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to create</param>
        [Theory]
        [MemberData(nameof(RandomApplications))]
        public void Create(PamaxieApplication application)
        {
            //Act
            PamaxieApplication createdApplication = Service.Applications.Create(application);

            //Assert
            Assert.NotNull(createdApplication);
            Assert.False(string.IsNullOrWhiteSpace(createdApplication.UniqueKey));
            string str = JsonConvert.SerializeObject(createdApplication);
            TestOutputHelper.WriteLine(str);

            //Delete after being created or it will slow down other tests
            Service.Applications.Delete(application);
        }

        /// <summary>
        /// Tries to create a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to create</param>
        [Theory]
        [MemberData(nameof(RandomApplications))]
        public void TryCreate(PamaxieApplication application)
        {
            //Act
            bool created = Service.Applications.TryCreate(application, out PamaxieApplication createdApplication);

            //Assert
            Assert.True(created);
            Assert.NotNull(createdApplication);
            Assert.False(string.IsNullOrWhiteSpace(createdApplication.UniqueKey));
            string str = JsonConvert.SerializeObject(createdApplication);
            TestOutputHelper.WriteLine(str);

            //Delete after being created or it will slow down other tests
            Service.Applications.Delete(application);
        }

        /// <summary>
        /// Updates a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to update</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Update(PamaxieApplication application)
        {
            //Arrange
            string oldName = application.ApplicationName;
            application.ApplicationName = RandomService.GenerateRandomName();

            //Act
            PamaxieApplication updatedApplication = Service.Applications.Update(application);

            //Assert
            Assert.NotNull(updatedApplication);
            Assert.NotEqual(oldName, updatedApplication.ApplicationName);
            Assert.Equal(application.ApplicationName, updatedApplication.ApplicationName);
            string str = JsonConvert.SerializeObject(updatedApplication);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            application.ApplicationName = oldName;
            Service.Applications.Update(application);
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to update</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void TryUpdate(PamaxieApplication application)
        {
            //Arrange
            string oldName = application.ApplicationName;
            application.ApplicationName = RandomService.GenerateRandomName();

            //Act
            bool updated = Service.Applications.TryUpdate(application, out PamaxieApplication updatedApplication);

            //Assert
            Assert.True(updated);
            Assert.NotNull(updatedApplication);
            Assert.NotEqual(oldName, updatedApplication.ApplicationName);
            Assert.Equal(application.ApplicationName, updatedApplication.ApplicationName);
            string str = JsonConvert.SerializeObject(updatedApplication);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            application.ApplicationName = oldName;
            Service.Applications.Update(application);
        }

        /// <summary>
        /// Tries to create a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to create</param>
        [Theory]
        [MemberData(nameof(RandomApplications))]
        public void UpdateOrCreate_Create(PamaxieApplication application)
        {
            //Act
            bool created = Service.Applications.TryCreate(application, out PamaxieApplication createdApplication);

            //Assert
            Assert.True(created);
            Assert.NotNull(createdApplication);
            Assert.False(string.IsNullOrWhiteSpace(createdApplication.UniqueKey));
            string str = JsonConvert.SerializeObject(createdApplication);
            TestOutputHelper.WriteLine(str);

            //Delete after being created or it will slow down other tests
            Service.Applications.Delete(application);
        }

        /// <summary>
        /// Tries to updates a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to update</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void UpdateOrCreate_Update(PamaxieApplication application)
        {
            //Arrange
            string oldName = application.ApplicationName;
            application.ApplicationName = RandomService.GenerateRandomName();

            //Act
            bool updated = Service.Applications.TryUpdate(application, out PamaxieApplication updatedApplication);

            //Assert
            Assert.True(updated);
            Assert.NotNull(updatedApplication);
            Assert.NotEqual(oldName, updatedApplication.ApplicationName);
            Assert.Equal(application.ApplicationName, updatedApplication.ApplicationName);
            string str = JsonConvert.SerializeObject(updatedApplication);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            application.ApplicationName = oldName;
            Service.Applications.Update(application);
        }

        /// <summary>
        /// Deletes a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to delete</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Delete(PamaxieApplication application)
        {
            //Act
            bool deleted = Service.Applications.Delete(application);

            //Assert
            Assert.True(deleted);
            TestOutputHelper.WriteLine("Deleted {0}", true);

            //Add it back, so it will not fail other tests
            Service.Applications.Create(application);
        }

        /// <summary>
        /// Get the owner of a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to get the owner of</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void GetOwner(PamaxieApplication application)
        {
            //Act
            PamaxieUser user = Service.Applications.GetOwner(application);

            //Assert
            Assert.NotNull(user);
            string str = JsonConvert.SerializeObject(user);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Enable or disable a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to enable or disable</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void EnableOrDisable(PamaxieApplication application)
        {
            //Arrange
            bool disabled = application.Disabled;

            //Act
            PamaxieApplication enabledOrDisabledApplication = Service.Applications.EnableOrDisable(application);

            //Assert
            Assert.NotEqual(disabled, enabledOrDisabledApplication.Disabled);
            TestOutputHelper.WriteLine(enabledOrDisabledApplication.Disabled ? "Enabled" : "Disabled");

            //Change it back before to avoid conflicts between other tests
            Service.Applications.EnableOrDisable(application);
        }

        /// <summary>
        /// Verifies a <see cref="PamaxieApplication"/>'s credentials
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to verify</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void VerifyAuthentication(PamaxieApplication application)
        {
            //Act
            bool verified = Service.Applications.VerifyAuthentication(application);

            //Assert
            Assert.True(verified);
            TestOutputHelper.WriteLine("Verified {0}", true);
        }
    }
}