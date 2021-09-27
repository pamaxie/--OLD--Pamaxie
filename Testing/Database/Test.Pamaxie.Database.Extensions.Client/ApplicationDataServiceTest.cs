using System.Collections.Generic;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Database.Extensions.Client_Test
{
    /// <summary>
    /// Testing class for <see cref="ApplicationDataService"/>
    /// </summary>
    public sealed class ApplicationDataServiceTest : ClientTestBase
    {
        /// <inheritdoc cref="MemberData.AllApplicationKeys"/>
        public static IEnumerable<object[]> AllApplicationKeys => MemberData.AllApplicationKeys;

        /// <inheritdoc cref="MemberData.AllApplications"/>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;

        /// <inheritdoc cref="MemberData.RandomApplications"/>
        public static IEnumerable<object[]> RandomApplications => MemberData.RandomApplications;

        public ApplicationDataServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Get a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplicationKeys))]
        public void Get(string applicationKey)
        {
            //Act
            PamaxieApplication application = ApplicationDataServiceExtension.Get(applicationKey);

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
            PamaxieApplication createdApplication = application.Create();

            //Assert
            Assert.NotNull(createdApplication);
            Assert.False(string.IsNullOrEmpty(createdApplication.Key));
            string str = JsonConvert.SerializeObject(createdApplication);
            TestOutputHelper.WriteLine(str);
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
            bool created = application.TryCreate(out PamaxieApplication createdApplication);

            //Assert
            Assert.True(created);
            Assert.NotNull(createdApplication);
            Assert.False(string.IsNullOrEmpty(createdApplication.Key));
            string str = JsonConvert.SerializeObject(createdApplication);
            TestOutputHelper.WriteLine(str);
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
            PamaxieApplication updatedApplication = application.Update();

            //Assert
            Assert.NotNull(updatedApplication);
            Assert.NotEqual(oldName, updatedApplication.ApplicationName);
            Assert.Equal(application.ApplicationName, updatedApplication.ApplicationName);
            string str = JsonConvert.SerializeObject(updatedApplication);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            application.ApplicationName = oldName;
            application.Update();
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
            bool updated = application.TryUpdate(out PamaxieApplication updatedApplication);

            //Assert
            Assert.True(updated);
            Assert.NotNull(updatedApplication);
            Assert.NotEqual(oldName, updatedApplication.ApplicationName);
            Assert.Equal(application.ApplicationName, updatedApplication.ApplicationName);
            string str = JsonConvert.SerializeObject(updatedApplication);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            application.ApplicationName = oldName;
            application.Update();
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
            bool created = application.UpdateOrCreate(out PamaxieApplication createdApplication);

            //Assert
            Assert.True(created);
            Assert.NotNull(createdApplication);
            Assert.False(string.IsNullOrEmpty(createdApplication.Key));
            string str = JsonConvert.SerializeObject(createdApplication);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieApplication"/>
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
            bool created = application.UpdateOrCreate(out PamaxieApplication updatedApplication);

            //Assert
            Assert.False(created);
            Assert.NotNull(updatedApplication);
            Assert.NotEqual(oldName, updatedApplication.ApplicationName);
            Assert.Equal(application.ApplicationName, updatedApplication.ApplicationName);
            string str = JsonConvert.SerializeObject(updatedApplication);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            application.ApplicationName = oldName;
            application.Update();
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
            bool deleted = application.Delete();

            //Assert
            Assert.True(deleted);
            TestOutputHelper.WriteLine("Deleted {0}", true);

            //Add it back, so it will not fail other tests
            application.Create();
        }

        /// <summary>
        /// Enable or disable a <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="application">The <see cref="PamaxieApplication"/> to get owner from</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void GetOwner(PamaxieApplication application)
        {
            //Act
            PamaxieUser user = application.GetOwner();

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
            PamaxieApplication enabledOrDisabledApplication = application.EnableOrDisable();

            //Assert
            Assert.NotEqual(disabled, enabledOrDisabledApplication.Disabled);
            TestOutputHelper.WriteLine(enabledOrDisabledApplication.Disabled ? "Enabled" : "Disabled");

            //Change it back before to avoid conflicts between other tests
            application.EnableOrDisable();
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
            bool verified = application.VerifyAuthentication();

            //Assert
            Assert.True(verified);
            TestOutputHelper.WriteLine("Verified {0}", true);
        }
    }
}