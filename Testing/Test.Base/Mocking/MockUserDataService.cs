using System.Collections.Generic;
using System.Linq;
using Moq;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using Pamaxie.Database.Extensions.Client;

namespace Test.TestBase
{
    /// <summary>
    /// Class containing method for mocking <see cref="UserDataService"/>.
    /// </summary>
    public static class MockUserDataService
    {
        private delegate void OutAction<in T, TOut>(T val, out TOut outVal);

        private static IPamaxieUser _createdValue;
        private static bool _created;
        private static IPamaxieUser _updatedValue;
        private static bool _updated;
        private static IPamaxieUser _updatedOrCreatedValue;
        private static bool _updatedOrCreated;

        /// <summary>
        /// Mocks the <see cref="UserDataService"/> for testing usage
        /// </summary>
        public static void Mock()
        {
            UserDataService userDataService = new();
            Mock<IUserDataService> mockUserDataService = new();
            
            //Setup for Get
            mockUserDataService.Setup(_ => _.Get(It.IsAny<string>()))
                .Returns<string>((key) => userDataService.Get(key));
            
            //Setup for Create
            mockUserDataService.Setup(_ => _.Create(It.IsAny<IPamaxieUser>()))
                .Returns<IPamaxieUser>((value) => userDataService.Create(value));
            
            //Setup for TryCreate
            mockUserDataService.Setup(_ => _.TryCreate(It.IsAny<IPamaxieUser>(), out _createdValue))
                .Callback(new OutAction<IPamaxieUser, IPamaxieUser>(
                    (IPamaxieUser value, out IPamaxieUser createdValue) =>
                    {
                        _created = userDataService.TryCreate(value, out createdValue);
                        _createdValue = createdValue;
                    }))
                .Returns<IPamaxieUser, IPamaxieUser>((_, _) => _created);
            
            //Setup for Update
            mockUserDataService.Setup(_ => _.Update(It.IsAny<IPamaxieUser>()))
                .Returns<IPamaxieUser>((value) => userDataService.Update(value));
            
            //Setup for TryUpdated
            mockUserDataService.Setup(_ => _.TryUpdate(It.IsAny<IPamaxieUser>(), out _updatedValue))
                .Callback(new OutAction<IPamaxieUser, IPamaxieUser>(
                    (IPamaxieUser value, out IPamaxieUser updatedValue) =>
                    {
                        _updated = userDataService.TryUpdate(value, out updatedValue);
                        _updatedValue = updatedValue;
                    }))
                .Returns<IPamaxieUser, IPamaxieUser>((_, _) => _updated);
            
            //Setup for UpdateOrCreate
            mockUserDataService.Setup(_ => _.UpdateOrCreate(It.IsAny<IPamaxieUser>(), out _updatedOrCreatedValue))
                .Callback(new OutAction<IPamaxieUser, IPamaxieUser>(
                    (IPamaxieUser value, out IPamaxieUser updatedOrCreatedValue) =>
                    {
                        _updatedOrCreated = userDataService.UpdateOrCreate(value, out updatedOrCreatedValue);
                        _updatedOrCreatedValue = updatedOrCreatedValue;
                    }))
                .Returns<IPamaxieUser, IPamaxieUser>((_, _) => _updatedOrCreated);
            
            //Setup for Delete
            mockUserDataService.Setup(_ => _.Delete(It.IsAny<IPamaxieUser>()))
                .Returns<IPamaxieUser>((value) => userDataService.Delete(value));
            
            //Setup for GetAllApplications
            mockUserDataService.Setup(_ => _.GetAllApplications(It.IsAny<IPamaxieUser>()))
                .Returns<IPamaxieUser>((value) => userDataService.GetAllApplications(value));
            
            //Setup for VerifyEmail
            mockUserDataService.Setup(_ => _.VerifyEmail(It.IsAny<IPamaxieUser>()))
                .Returns<IPamaxieUser>((value) => userDataService.VerifyEmail(value));
            
            DatabaseService.UserService = mockUserDataService.Object;
        }

        /// <inheritdoc cref="IUserDataService"/>
        private class UserDataService : IUserDataService
        {
            /// <inheritdoc cref="IUserDataService.Get"/>
            public IPamaxieUser Get(string key)
            {
                return string.IsNullOrEmpty(key) ? null : TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == key);
            }

            /// <inheritdoc cref="IUserDataService.Create"/>
            public IPamaxieUser Create(IPamaxieUser value)
            {
                if (value == null)
                    return null;
                if (TestUserData.ListOfUsers.Any(_ => _.Key == value.Key))
                    return value;
                TestUserData.ListOfUsers.Add(value);
                return value;
            }

            /// <inheritdoc cref="IUserDataService.TryCreate"/>
            public bool TryCreate(IPamaxieUser value, out IPamaxieUser createdValue)
            {
                createdValue = null;
                if (value == null)
                    return false;
                if (TestUserData.ListOfUsers.Any(_ => _.Key == value.Key))
                    return false;
                TestUserData.ListOfUsers.Add(value);
                createdValue = value;
                return true;
            }

            /// <inheritdoc cref="IUserDataService.Update"/>
            public IPamaxieUser Update(IPamaxieUser value)
            {
                if (value == null)
                    return null;
                if (TestUserData.ListOfUsers.Any(_ => _.Key == value.Key))
                    return value;
                TestUserData.ListOfUsers.Add(value);
                return value;
            }

            /// <inheritdoc cref="IUserDataService.TryUpdate"/>
            public bool TryUpdate(IPamaxieUser value, out IPamaxieUser updatedValue)
            {
                updatedValue = null;
                if (value == null)
                    return false;
                int indexToUpdate = TestUserData.ListOfUsers.FindIndex(_ => _.Key == value.Key);
                if (indexToUpdate == -1)
                    return false;
                TestUserData.ListOfUsers[indexToUpdate] = value;
                updatedValue = value;
                return true;
            }

            /// <inheritdoc cref="IUserDataService.UpdateOrCreate"/>
            public bool UpdateOrCreate(IPamaxieUser value, out IPamaxieUser updatedOrCreatedValue)
            {
                updatedOrCreatedValue = null;
                if (value == null)
                    return false;
                if (TestUserData.ListOfUsers.Any(_ => _.Key != value.Key))
                {
                    TestUserData.ListOfUsers.Add(value);
                }
                else
                {
                    int indexToUpdate = TestUserData.ListOfUsers.FindIndex(_ => _.Key == value.Key);
                    TestUserData.ListOfUsers[indexToUpdate] = value;
                }
                updatedOrCreatedValue = value;
                return true;
            }

            /// <inheritdoc cref="IUserDataService.Delete"/>
            public bool Delete(IPamaxieUser value)
            {
                if (value == null)
                    return false;
                IPamaxieUser valueToRemove = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.Key);
                if (valueToRemove == null)
                    return false;
                TestUserData.ListOfUsers.Remove(valueToRemove);
                return true;
            }

            /// <inheritdoc cref="IUserDataService.GetAllApplications"/>
            public IEnumerable<IPamaxieApplication> GetAllApplications(IPamaxieUser value)
            {
                List<IPamaxieApplication> applications = value.ApplicationKeys
                    .Select(key => TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == key))
                    .Where(application => application != null).Cast<IPamaxieApplication>().ToList();
                return applications.AsEnumerable();
            }

            /// <inheritdoc cref="IUserDataService.VerifyEmail"/>
            public bool VerifyEmail(IPamaxieUser value)
            {
                if (value == null)
                    return false;
                IPamaxieUser valueToVerify = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.Key);
                if (valueToVerify == null)
                    return false;
                valueToVerify.EmailVerified = true;
                return true;
            }
        }
    }
}