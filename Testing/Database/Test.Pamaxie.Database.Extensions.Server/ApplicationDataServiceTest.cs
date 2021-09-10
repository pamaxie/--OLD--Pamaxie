using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Get a application
        /// </summary>
        /// <param name="applicationKey">The key of the application</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Get(string applicationKey)
        {
            PamaxieApplication application = Service.Applications.Get(applicationKey);
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
        [MemberData(nameof(RandomApplications))]
        public void Create(string ownerKey, string applicationName, string authorizationToken)
        {
            PamaxieApplication application = new PamaxieApplication
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
            PamaxieApplication createdApplication = Service.Applications.Create(application);
            Assert.NotNull(createdApplication);
            Assert.NotEmpty(createdApplication.Key);
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == createdApplication.OwnerKey);
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
        [MemberData(nameof(RandomApplications))]
        public void TryCreate(string ownerKey, string applicationName, string authorizationToken)
        {
            PamaxieApplication application = new PamaxieApplication
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
            bool created = Service.Applications.TryCreate(application, out PamaxieApplication createdApplication);
            Assert.True(created);
            Assert.NotNull(createdApplication);
            Assert.NotEmpty(createdApplication.Key);
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == createdApplication.OwnerKey);
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
            PamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            string oldName = application.ApplicationName;
            application.ApplicationName = RandomService.GenerateRandomName();
            PamaxieApplication updatedApplication = Service.Applications.Update(application);
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
            PamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            string oldName = application.ApplicationName;
            application.ApplicationName = RandomService.GenerateRandomName();
            bool updated = Service.Applications.TryUpdate(application, out PamaxieApplication updatedApplication);
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
        [MemberData(nameof(RandomApplications))]
        public void UpdateOrCreate_Create(string ownerKey, string applicationName, string authorizationToken)
        {
            PamaxieApplication application = new PamaxieApplication
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
            bool created = Service.Applications.TryCreate(application, out PamaxieApplication createdApplication);
            Assert.True(created);
            Assert.NotNull(createdApplication);
            Assert.NotEmpty(createdApplication.Key);
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == createdApplication.OwnerKey);
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
            PamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            string oldName = application.ApplicationName;
            application.ApplicationName = RandomService.GenerateRandomName();
            bool updated = Service.Applications.TryUpdate(application, out PamaxieApplication updatedApplication);
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
            PamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            bool deleted = Service.Applications.Delete(application);
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
            PamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            PamaxieUser user = Service.Applications.GetOwner(application);
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
            PamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            bool disabled = application.Disabled;
            PamaxieApplication enabledOrDisabledApplication = Service.Applications.EnableOrDisable(application);
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
            PamaxieApplication application =
                TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == applicationKey);
            Assert.NotNull(application);
            bool verified = Service.Applications.VerifyAuthentication(application);
            Assert.True(verified);
            TestOutputHelper.WriteLine("Verified {0}", true);
        }
    }
}