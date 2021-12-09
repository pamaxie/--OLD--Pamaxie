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
        /// <inheritdoc cref="MemberData.AllUsers"/>
        public static IEnumerable<object[]> AllUserKeys => MemberData.AllUserKeys;

        /// <inheritdoc cref="MemberData.AllUsers"/>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;

        /// <inheritdoc cref="MemberData.RandomUsers"/>
        public static IEnumerable<object[]> RandomUserData => MemberData.RandomUsers;

        public UserDataServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
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
            PamaxieUser user = Service.Users.Get(userKey);

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
            PamaxieUser createdUser = Service.Users.Create(user);

            //Assert
            Assert.NotNull(createdUser);
            Assert.False(string.IsNullOrWhiteSpace(createdUser.UniqueKey));
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);

            //Delete after being created or it will slow down other tests
            Service.Users.Delete(user);
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
            bool created = Service.Users.TryCreate(user, out PamaxieUser createdUser);

            //Assert
            Assert.True(created);
            Assert.NotNull(createdUser);
            Assert.False(string.IsNullOrWhiteSpace(createdUser.UniqueKey));
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);

            //Delete after being created or it will slow down other tests
            Service.Users.Delete(user);
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
            PamaxieUser updatedUser = Service.Users.Update(user);

            //Assert
            Assert.NotNull(updatedUser);
            Assert.NotEqual(oldEmailAddress, updatedUser.EmailAddress);
            Assert.Equal(user.EmailAddress, updatedUser.EmailAddress);
            string str = JsonConvert.SerializeObject(updatedUser);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            user.EmailAddress = oldEmailAddress;
            Service.Users.Update(user);
        }

        /// <summary>
        /// Tries to update a <see cref="PamaxieUser"/>
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
            bool updated = Service.Users.TryUpdate(user, out PamaxieUser updatedUser);

            //Assert
            Assert.True(updated);
            Assert.NotNull(updatedUser);
            Assert.NotEqual(oldEmailAddress, updatedUser.EmailAddress);
            Assert.Equal(user.EmailAddress, updatedUser.EmailAddress);
            string str = JsonConvert.SerializeObject(updatedUser);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            user.EmailAddress = oldEmailAddress;
            Service.Users.Update(user);
        }

        /// <summary>
        /// Tries to create a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to create</param>
        [Theory]
        [MemberData(nameof(RandomUserData))]
        public void UpdateOrCreate_Create(PamaxieUser user)
        {
            //Act
            bool created = Service.Users.UpdateOrCreate(user, out PamaxieUser createdUser);

            //Assert
            Assert.True(created);
            Assert.NotNull(createdUser);
            Assert.False(string.IsNullOrWhiteSpace(createdUser.UniqueKey));
            string str = JsonConvert.SerializeObject(createdUser);
            TestOutputHelper.WriteLine(str);

            //Delete after being created or it will slow down other tests
            Service.Users.Delete(user);
        }

        /// <summary>
        /// Updates or creates a <see cref="PamaxieUser"/>
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
            bool created = Service.Users.UpdateOrCreate(user, out PamaxieUser updatedUser);

            //Assert
            Assert.False(created);
            Assert.NotNull(updatedUser);
            Assert.NotEqual(oldEmailAddress, updatedUser.EmailAddress);
            Assert.Equal(user.EmailAddress, updatedUser.EmailAddress);
            string str = JsonConvert.SerializeObject(updatedUser);
            TestOutputHelper.WriteLine(str);

            //Change the email address back, to avoid confusing between tests
            user.EmailAddress = oldEmailAddress;
            Service.Users.Update(user);
        }

        /// <summary>
        /// Deletes a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to update</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Delete(PamaxieUser user)
        {
            //Act
            bool deleted = Service.Users.Delete(user);

            //Assert
            Assert.True(deleted);
            TestOutputHelper.WriteLine("Deleted {0}", true);

            //Add it back, so it will not fail other tests
            Service.Users.Create(user);
        }

        /// <summary>
        /// Gets all <see cref="PamaxieApplication"/>s the <see cref="PamaxieUser"/> owns
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to get <see cref="PamaxieApplication"/>s from</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void GetAllApplications(PamaxieUser user)
        {
            //Act
            List<PamaxieApplication> applications = Service.Users.GetAllApplications(user).ToList();

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
        /// <param name="user">The <see cref="PamaxieUser"/> to update</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void VerifyEmail(PamaxieUser user)
        {
            //Act
            bool verified = Service.Users.VerifyEmail(user);

            //Assert
            Assert.True(verified);
            TestOutputHelper.WriteLine("Verified Email {0} - {1}", true, user.EmailAddress);
        }
    }
}