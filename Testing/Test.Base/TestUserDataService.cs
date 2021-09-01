using System.Collections.Generic;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Xunit;
using Xunit.Abstractions;

namespace Test.TestBase
{
    public class TestUserDataService : BaseTest
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;
        
        /// <summary>
        /// <inheritdoc cref="MemberData.UnusedUserKeys"/>
        /// </summary>
        public static IEnumerable<object[]> RandomUserData => MemberData.RandomUserData;
        
        public TestUserDataService(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            //Mock UserDataService, for UserDataServiceExtension
            MockUserDataService.Mock();
        }

        /// <summary>
        /// Get all users from their key
        /// </summary>
        /// <param name="userKey">The key of the user</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Get(string userKey)
        {
            IPamaxieUser user = UserDataServiceExtension.Get(userKey);
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
            IPamaxieUser user = new PamaxieUser()
            {
                UserName = userName,
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = emailAddress,
                EmailVerified = false,
                ProfilePictureAddress = "",
                Disabled = false,
                Deleted = false
            };
            IPamaxieUser createdUser = user.Create();
            Assert.NotNull(createdUser);
            Assert.NotEmpty(createdUser.Key);
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);
        }
    }
}