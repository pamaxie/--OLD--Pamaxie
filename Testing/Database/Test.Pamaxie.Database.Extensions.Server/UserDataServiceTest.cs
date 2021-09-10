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
    /// Testing class for <see cref="UserDataService"/>
    /// </summary>
    public sealed class UserDataServiceTest : ServerBase
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;

        /// <summary>
        /// <inheritdoc cref="MemberData.RandomUsers"/>
        /// </summary>
        public static IEnumerable<object[]> RandomUserData => MemberData.RandomUsers;

        public UserDataServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Get a user
        /// </summary>
        /// <param name="userKey">The key of the user</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Get(string userKey)
        {
            PamaxieUser user = Service.Users.Get(userKey);
            Assert.NotNull(user);
            string str = JsonConvert.SerializeObject(user);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Create a user
        /// </summary>
        /// <param name="userName">The username of the user</param>
        /// <param name="firstName">The firstname of the user</param>
        /// <param name="lastName">The lastname of the user</param>
        /// <param name="emailAddress">The email address of the user</param>
        [Theory]
        [MemberData(nameof(RandomUserData))]
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
            PamaxieUser createdUser = Service.Users.Create(user);
            Assert.NotNull(createdUser);
            Assert.NotEmpty(createdUser.Key);
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to create a user
        /// </summary>
        /// <param name="userName">The username of the user</param>
        /// <param name="firstName">The firstname of the user</param>
        /// <param name="lastName">The lastname of the user</param>
        /// <param name="emailAddress">The email address of the user</param>
        [Theory]
        [MemberData(nameof(RandomUserData))]
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
            bool created = Service.Users.TryCreate(user, out PamaxieUser createdUser);
            Assert.True(created);
            Assert.NotNull(createdUser);
            Assert.NotEmpty(createdUser.Key);
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Updates a user
        /// </summary>
        /// <param name="userKey">Key of the user</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Update(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            string oldEmailAddress = user.EmailAddress;
            user.EmailAddress = RandomService.GenerateRandomName();
            PamaxieUser updatedUser = Service.Users.Update(user);
            Assert.NotNull(updatedUser);
            Assert.NotEqual(oldEmailAddress, updatedUser.EmailAddress);
            Assert.Equal(user.EmailAddress, updatedUser.EmailAddress);
            string str = JsonConvert.SerializeObject(updatedUser);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to updates a user
        /// </summary>
        /// <param name="userKey">Key of the user</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void TryUpdate(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            string oldEmailAddress = user.EmailAddress;
            user.EmailAddress = RandomService.GenerateRandomName();
            bool updated = Service.Users.TryUpdate(user, out PamaxieUser updatedUser);
            Assert.True(updated);
            Assert.NotNull(updatedUser);
            Assert.NotEqual(oldEmailAddress, updatedUser.EmailAddress);
            Assert.Equal(user.EmailAddress, updatedUser.EmailAddress);
            string str = JsonConvert.SerializeObject(updatedUser);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to updates a user
        /// </summary>
        /// <param name="userName">The username of the user</param>
        /// <param name="firstName">The firstname of the user</param>
        /// <param name="lastName">The lastname of the user</param>
        /// <param name="emailAddress">The email address of the user</param>
        [Theory]
        [MemberData(nameof(RandomUserData))]
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
            bool created = Service.Users.UpdateOrCreate(user, out PamaxieUser createdUser);
            Assert.True(created);
            Assert.NotNull(createdUser);
            Assert.NotEmpty(createdUser.Key);
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Updates or creates a user
        /// </summary>
        /// <param name="userKey">Key of the user</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void UpdateOrCreate_Update(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            string oldEmailAddress = user.EmailAddress;
            user.EmailAddress = RandomService.GenerateRandomName();
            bool updated = Service.Users.UpdateOrCreate(user, out PamaxieUser updatedUser);
            Assert.True(updated);
            Assert.NotNull(updatedUser);
            Assert.NotEqual(oldEmailAddress, updatedUser.EmailAddress);
            Assert.Equal(user.EmailAddress, updatedUser.EmailAddress);
            string str = JsonConvert.SerializeObject(updatedUser);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Deletes a user
        /// </summary>
        /// <param name="userKey">Key of the user</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Delete(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            bool deleted = Service.Users.Delete(user);
            Assert.True(deleted);
            TestOutputHelper.WriteLine("Deleted {0}", true);

            //Add it back, so it will not fail other tests
            TestUserData.ListOfUsers.Add(user);
        }

        /// <summary>
        /// Gets all applications the user owns
        /// </summary>
        /// <param name="userKey">Key of the user</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void GetAllApplications(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            List<PamaxieApplication> applications = Service.Users.GetAllApplications(user).ToList();
            Assert.NotNull(applications);
            foreach (string str in applications.Select(JsonConvert.SerializeObject)) TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Verifies a user's email address
        /// </summary>
        /// <param name="userKey">Key of the user</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void VerifyEmail(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            bool verified = Service.Users.VerifyEmail(user);
            Assert.True(verified);
            TestOutputHelper.WriteLine("Verified Email {0} - {1}", true, user.EmailAddress);
        }
    }
}