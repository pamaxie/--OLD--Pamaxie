using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Database.Extensions.Client_Test
{
    /// <summary>
    /// Testing class for <see cref="UserDataService"/>
    /// </summary>
    public sealed class UserDataServiceTest : ClientTestBase, IClassFixture<DatabaseApiFactory>
    {
        /// <inheritdoc cref="MemberData.AllUserKeys"/>
        public static IEnumerable<object[]> AllUserKeys => MemberData.AllUserKeys;

        /// <inheritdoc cref="MemberData.AllUsers"/>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;

        /// <inheritdoc cref="MemberData.RandomUsers"/>
        public static IEnumerable<object[]> RandomUserData => MemberData.RandomUsers;

        public UserDataServiceTest(DatabaseApiFactory fixture, ITestOutputHelper testOutputHelper) : base(
            fixture, testOutputHelper)
        {
        }

        /// <summary>
        /// Get a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="userKey">The key of the <see cref="PamaxieUser"/></param>
        [Theory]
        [MemberData(nameof(AllUserKeys))]
        public void Get(string userKey)
        {
            //Act
            PamaxieUser user = UserDataServiceExtension.Get(userKey);

            //Assert
            Assert.NotNull(user);
            string str = JsonConvert.SerializeObject(user);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Create a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to create</param>
        [Theory]
        [MemberData(nameof(RandomUserData))]
        public void Create(PamaxieUser user)
        {
            //Act
            PamaxieUser createdUser = user.Create();

            //Assert
            Assert.NotNull(createdUser);
            Assert.NotEmpty(createdUser.Key);
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to create a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to create</param>
        [Theory]
        [MemberData(nameof(RandomUserData))]
        public void TryCreate(PamaxieUser user)
        {
            //Act
            bool created = user.TryCreate(out PamaxieUser createdUser);

            //Assert
            Assert.True(created);
            Assert.NotNull(createdUser);
            Assert.NotEmpty(createdUser.Key);
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Updates a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to update</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Update(PamaxieUser user)
        {
            //Arrange
            string oldEmailAddress = user.EmailAddress;
            user.EmailAddress = RandomService.GenerateRandomName();

            //Act
            PamaxieUser updatedUser = user.Update();

            //Assert
            Assert.NotNull(updatedUser);
            Assert.NotEqual(oldEmailAddress, updatedUser.EmailAddress);
            Assert.Equal(user.EmailAddress, updatedUser.EmailAddress);
            string str = JsonConvert.SerializeObject(updatedUser);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            user.EmailAddress = oldEmailAddress;
            user.Update();
        }

        /// <summary>
        /// Tries to updates a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to update</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void TryUpdate(PamaxieUser user)
        {
            //Arrange
            string oldEmailAddress = user.EmailAddress;
            user.EmailAddress = RandomService.GenerateRandomName();

            //Act
            bool updated = user.TryUpdate(out PamaxieUser updatedUser);

            //Assert
            Assert.True(updated);
            Assert.NotNull(updatedUser);
            Assert.NotEqual(oldEmailAddress, updatedUser.EmailAddress);
            Assert.Equal(user.EmailAddress, updatedUser.EmailAddress);
            string str = JsonConvert.SerializeObject(updatedUser);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            user.EmailAddress = oldEmailAddress;
            user.Update();
        }

        /// <summary>
        /// Tries to updates a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to create</param>
        [Theory]
        [MemberData(nameof(RandomUserData))]
        public void UpdateOrCreate_Create(PamaxieUser user)
        {
            //Act
            bool created = user.UpdateOrCreate(out PamaxieUser createdUser);

            //Assert
            Assert.True(created);
            Assert.NotNull(createdUser);
            Assert.NotEmpty(createdUser.Key);
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to update</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void UpdateOrCreate_Update(PamaxieUser user)
        {
            //Arrange
            string oldEmailAddress = user.EmailAddress;
            user.EmailAddress = RandomService.GenerateRandomName();

            //Act
            bool created = user.UpdateOrCreate(out PamaxieUser updatedUser);

            //Assert
            Assert.False(created);
            Assert.NotNull(updatedUser);
            Assert.NotEqual(oldEmailAddress, updatedUser.EmailAddress);
            Assert.Equal(user.EmailAddress, updatedUser.EmailAddress);
            string str = JsonConvert.SerializeObject(updatedUser);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            user.EmailAddress = oldEmailAddress;
            user.Update();
        }

        /// <summary>
        /// Deletes a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to delete</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Delete(PamaxieUser user)
        {
            //Act
            bool deleted = user.Delete();

            //Assert
            Assert.True(deleted);
            TestOutputHelper.WriteLine("Deleted {0}", true);

            //Add it back, so it will not fail other tests
            user.Create();
        }

        /// <summary>
        /// Gets all <see cref="PamaxieApplication"/> the <see cref="PamaxieUser"/> owns
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to get all <see cref="PamaxieApplication"/> from</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void GetAllApplications(PamaxieUser user)
        {
            //Act
            IEnumerable<PamaxieApplication> applications = user.GetAllApplications();

            //Assert
            Assert.NotNull(applications);

            foreach (string str in applications.Select(JsonConvert.SerializeObject))
            {
                TestOutputHelper.WriteLine(str);
            }
        }

        /// <summary>
        /// Verifies a <see cref="PamaxieUser"/>'s email address
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to verify</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void VerifyEmail(PamaxieUser user)
        {
            //Act
            bool verified = user.VerifyEmail();

            //Assert
            Assert.True(verified);
            TestOutputHelper.WriteLine("Verified Email {0} - {1}", true, user.EmailAddress);
        }
    }
}