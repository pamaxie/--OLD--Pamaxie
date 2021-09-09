using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Xunit;
using Xunit.Abstractions;

namespace Test.Base
{
    /// <summary>
    /// Testing class to test the custom ApplicationDataService Client interaction used when testing towards other projects
    /// </summary>
    public class TestApplicationDataService : TestBase
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllApplications"/>
        /// </summary>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;

        /// <summary>
        /// <inheritdoc cref="MemberData.RandomApplications"/>
        /// </summary>
        public static IEnumerable<object[]> RandomApplicationData => MemberData.RandomApplications;

        public TestApplicationDataService(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            //Mock ApplicationDataService, for ApplicationDataServiceExtension
            MockApplicationDataService.Mock();
        }

        /// <summary>
        /// Get a application
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Get(string applicationKey)
        {
            IPamaxieApplication application = ApplicationDataServiceExtension.Get(applicationKey);
            Assert.NotNull(application);
            string str = JsonConvert.SerializeObject(application);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Create a application
        /// </summary>
        /// /// <param name="ownerKey">The key of the application owner</param>
        /// /// <param name="applicationName">The application name</param>
        /// /// <param name="authorizationToken">The authorization token</param>
        [Theory]
        [MemberData(nameof(RandomApplicationData))]
        public void Create(string ownerKey, string applicationName, string authorizationToken)
        {
            IPamaxieApplication application = new PamaxieApplication
            {
                TTL = DateTime.Now,
                Credentials = new AppAuthCredentials
                {
                    AuthorizationToken = authorizationToken,
                    AuthorizationTokenCipher = "",
                    LastAuth = DateTime.Now
                },
                OwnerKey = ownerKey,
                ApplicationName = applicationName,
                LastAuth = DateTime.Now,
                RateLimited = false,
                Disabled = false,
                Deleted = false
            };
            IPamaxieApplication createdApplication = application.Create();
            Assert.NotNull(createdApplication);
            Assert.NotEmpty(createdApplication.Key);
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == createdApplication.OwnerKey);
            Assert.NotNull(user);
            string str = JsonConvert.SerializeObject(createdApplication);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to create a application
        /// </summary>
        /// /// <param name="ownerKey">The key of the application owner</param>
        /// /// <param name="applicationName">The application name</param>
        /// /// <param name="authorizationToken">The authorization token</param>
        [Theory]
        [MemberData(nameof(RandomApplicationData))]
        public void TryCreate(string ownerKey, string applicationName, string authorizationToken)
        {
            IPamaxieApplication application = new PamaxieApplication
            {
                TTL = DateTime.Now,
                Credentials = new AppAuthCredentials
                {
                    AuthorizationToken = authorizationToken,
                    AuthorizationTokenCipher = "",
                    LastAuth = DateTime.Now
                },
                OwnerKey = ownerKey,
                ApplicationName = applicationName,
                LastAuth = DateTime.Now,
                RateLimited = false,
                Disabled = false,
                Deleted = false
            };
            bool created = application.TryCreate(out IPamaxieApplication createdApplication);
            Assert.True(created);
            Assert.NotNull(createdApplication);
            Assert.NotEmpty(createdApplication.Key);
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == createdApplication.OwnerKey);
            Assert.NotNull(user);
            string str = JsonConvert.SerializeObject(createdApplication);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Updates a application
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Update(string applicationKey)
        {
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            string oldName = application.ApplicationName;
            application.ApplicationName = RandomService.GenerateRandomName();
            IPamaxieApplication updatedApplication = application.Update();
            Assert.NotNull(updatedApplication);
            Assert.NotEqual(oldName, updatedApplication.ApplicationName);
            Assert.Equal(application.ApplicationName, updatedApplication.ApplicationName);
            string str = JsonConvert.SerializeObject(updatedApplication);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to updates a application
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void TryUpdate(string applicationKey)
        {
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            string oldName = application.ApplicationName;
            application.ApplicationName = RandomService.GenerateRandomName();
            bool updated = application.TryUpdate(out IPamaxieApplication updatedApplication);
            Assert.True(updated);
            Assert.NotNull(updatedApplication);
            Assert.NotEqual(oldName, updatedApplication.ApplicationName);
            Assert.Equal(application.ApplicationName, updatedApplication.ApplicationName);
            string str = JsonConvert.SerializeObject(updatedApplication);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to create a application
        /// </summary>
        /// /// <param name="ownerKey">The key of the application owner</param>
        /// /// <param name="applicationName">The application name</param>
        /// /// <param name="authorizationToken">The authorization token</param>
        [Theory]
        [MemberData(nameof(RandomApplicationData))]
        public void UpdateOrCreate_Create(string ownerKey, string applicationName, string authorizationToken)
        {
            IPamaxieApplication application = new PamaxieApplication
            {
                TTL = DateTime.Now,
                Credentials = new AppAuthCredentials
                {
                    AuthorizationToken = authorizationToken,
                    AuthorizationTokenCipher = "",
                    LastAuth = DateTime.Now
                },
                OwnerKey = ownerKey,
                ApplicationName = applicationName,
                LastAuth = DateTime.Now,
                RateLimited = false,
                Disabled = false,
                Deleted = false
            };
            bool created = application.TryCreate(out IPamaxieApplication createdApplication);
            Assert.True(created);
            Assert.NotNull(createdApplication);
            Assert.NotEmpty(createdApplication.Key);
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == createdApplication.OwnerKey);
            Assert.NotNull(user);
            string str = JsonConvert.SerializeObject(createdApplication);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to updates a application
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void UpdateOrCreate_Update(string applicationKey)
        {
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            string oldName = application.ApplicationName;
            application.ApplicationName = RandomService.GenerateRandomName();
            bool updated = application.TryUpdate(out IPamaxieApplication updatedApplication);
            Assert.True(updated);
            Assert.NotNull(updatedApplication);
            Assert.NotEqual(oldName, updatedApplication.ApplicationName);
            Assert.Equal(application.ApplicationName, updatedApplication.ApplicationName);
            string str = JsonConvert.SerializeObject(updatedApplication);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Deletes a application
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Delete(string applicationKey)
        {
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            bool deleted = application.Delete();
            Assert.True(deleted);
            TestOutputHelper.WriteLine("Deleted {0}", true);

            //Add it back, so it will not fail other tests
            TestApplicationData.ListOfApplications.Add(application);
        }

        /// <summary>
        /// Enable or disable a application
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void GetOwner(string applicationKey)
        {
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            IPamaxieUser user = application.GetOwner();
            Assert.NotNull(user);
            string str = JsonConvert.SerializeObject(user);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Enable or disable a application
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void EnableOrDisable(string applicationKey)
        {
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            bool disabled = application.Disabled;
            IPamaxieApplication enabledOrDisabledApplication = application.EnableOrDisable();
            Assert.NotEqual(disabled, enabledOrDisabledApplication.Disabled);
            TestOutputHelper.WriteLine(enabledOrDisabledApplication.Disabled ? "Enabled" : "Disabled");
        }

        /// <summary>
        /// Verifies a application's credentials
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void VerifyAuthentication(string applicationKey)
        {
            IPamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            //bool verified = application.VerifyAuthentication();
            //Assert.True(verified);
            //TestOutputHelper.WriteLine("Verified {0}", true);
        }
    }
}