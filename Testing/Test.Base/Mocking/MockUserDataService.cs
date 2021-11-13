using System.Collections.Generic;
using System.Linq;
using Moq;
using Pamaxie.Data;
using Pamaxie.Database.Design;

namespace Test.Base
{
    /// <summary>
    /// Class containing method for mocking <see cref="UserDataService"/>.
    /// </summary>
    public static class MockUserDataService
    {
        private delegate void OutAction<in T, TOut>(T val, out TOut outVal);

        /// <summary>
        /// Mocks the <see cref="UserDataService"/> and applies it to the UserDataServiceExtension for testing usage
        /// </summary>
        public static IPamaxieUserDataInteraction Mock()
        {
            UserDataService userDataService = new UserDataService();
            Mock<IPamaxieUserDataInteraction> mockUserDataService = new Mock<IPamaxieUserDataInteraction>();

            //Setup for Get
            mockUserDataService.Setup(_ => _.Get(It.IsAny<string>()))
                .Returns<string>((key) => userDataService.Get(key));

            //Setup for Create
            mockUserDataService.Setup(_ => _.Create(It.IsAny<PamaxieUser>()))
                .Returns<PamaxieUser>((value) => userDataService.Create(value));

            //Setup for TryCreate
            PamaxieUser createdValue = null;
            bool created = false;
            mockUserDataService.Setup(_ => _.TryCreate(It.IsAny<PamaxieUser>(), out createdValue))
                .Callback(new OutAction<PamaxieUser, PamaxieUser>(
                    (PamaxieUser value, out PamaxieUser outValue) =>
                    {
                        created = userDataService.TryCreate(value, out outValue);
                        createdValue = outValue;
                    }))
                .Returns<PamaxieUser, PamaxieUser>((_, _) => created);

            //Setup for Update
            mockUserDataService.Setup(_ => _.Update(It.IsAny<PamaxieUser>()))
                .Returns<PamaxieUser>((value) => userDataService.Update(value));

            //Setup for TryUpdated
            PamaxieUser updatedValue = null;
            bool updated = false;
            mockUserDataService.Setup(_ => _.TryUpdate(It.IsAny<PamaxieUser>(), out updatedValue))
                .Callback(new OutAction<PamaxieUser, PamaxieUser>(
                    (PamaxieUser value, out PamaxieUser outValue) =>
                    {
                        updated = userDataService.TryUpdate(value, out outValue);
                        updatedValue = outValue;
                    }))
                .Returns<PamaxieUser, PamaxieUser>((_, _) => updated);

            //Setup for UpdateOrCreate
            PamaxieUser updatedOrCreatedValue = null;
            bool updatedOrCreated = false;
            mockUserDataService.Setup(_ => _.UpdateOrCreate(It.IsAny<PamaxieUser>(), out updatedOrCreatedValue))
                .Callback(new OutAction<PamaxieUser, PamaxieUser>(
                    (PamaxieUser value, out PamaxieUser outValue) =>
                    {
                        updatedOrCreated = userDataService.UpdateOrCreate(value, out outValue);
                        updatedOrCreatedValue = outValue;
                    }))
                .Returns<PamaxieUser, PamaxieUser>((_, _) => updatedOrCreated);

            //Setup for Delete
            mockUserDataService.Setup(_ => _.Delete(It.IsAny<PamaxieUser>()))
                .Returns<PamaxieUser>((value) => userDataService.Delete(value));

            //Setup for GetAllApplications
            mockUserDataService.Setup(_ => _.GetAllApplications(It.IsAny<PamaxieUser>()))
                .Returns<PamaxieUser>((value) => userDataService.GetAllApplications(value));

            //Setup for VerifyEmail
            mockUserDataService.Setup(_ => _.VerifyEmail(It.IsAny<PamaxieUser>()))
                .Returns<PamaxieUser>((value) => userDataService.VerifyEmail(value));

            return mockUserDataService.Object;
        }

        /// <inheritdoc cref="IPamaxieUserDataInteraction"/>
        private sealed class UserDataService : IPamaxieUserDataInteraction
        {
            /// <inheritdoc cref="IPamaxieUserDataInteraction.Get"/>
            public PamaxieUser Get(string key)
            {
                return string.IsNullOrEmpty(key) ? null : TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == key);
            }

            /// <inheritdoc cref="IPamaxieUserDataInteraction.Create"/>
            public PamaxieUser Create(PamaxieUser value)
            {
                if (value == null || !string.IsNullOrEmpty(value.UniqueKey))
                {
                    return null;
                }

                string key;
                do
                {
                    key = RandomService.GenerateRandomKey();
                } while (TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == key) != null);

                value.UniqueKey = key;

                TestUserData.ListOfUsers.Add(value);
                return value;
            }

            /// <inheritdoc cref="IPamaxieUserDataInteraction.TryCreate"/>
            public bool TryCreate(PamaxieUser value, out PamaxieUser createdValue)
            {
                createdValue = null;

                if (value == null || !string.IsNullOrEmpty(value.UniqueKey))
                {
                    return false;
                }

                string key;
                do
                {
                    key = RandomService.GenerateRandomKey();
                } while (TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == key) != null);

                value.UniqueKey = key;

                TestUserData.ListOfUsers.Add(value);
                createdValue = value;
                return true;
            }

            /// <inheritdoc cref="IPamaxieUserDataInteraction.Update"/>
            public PamaxieUser Update(PamaxieUser value)
            {
                if (value == null || string.IsNullOrEmpty(value.UniqueKey))
                {
                    return null;
                }

                if (TestUserData.ListOfUsers.Any(_ => _.UniqueKey == value.UniqueKey))
                {
                    return value;
                }

                TestUserData.ListOfUsers.Add(value);
                return value;
            }

            /// <inheritdoc cref="IPamaxieUserDataInteraction.TryUpdate"/>
            public bool TryUpdate(PamaxieUser value, out PamaxieUser updatedValue)
            {
                updatedValue = null;

                if (value == null || string.IsNullOrEmpty(value.UniqueKey))
                {
                    return false;
                }

                int indexToUpdate = TestUserData.ListOfUsers.FindIndex(_ => _.UniqueKey == value.UniqueKey);

                if (indexToUpdate == -1)
                {
                    return false;
                }

                TestUserData.ListOfUsers[indexToUpdate] = value;
                updatedValue = value;
                return true;
            }

            /// <inheritdoc cref="IPamaxieUserDataInteraction.UpdateOrCreate"/>
            public bool UpdateOrCreate(PamaxieUser value, out PamaxieUser updatedOrCreatedValue)
            {
                updatedOrCreatedValue = null;

                if (value == null)
                {
                    return false;
                }

                if (string.IsNullOrEmpty(value.UniqueKey) || TestUserData.ListOfUsers.All(_ => _.UniqueKey != value.UniqueKey))
                {
                    string key = value.UniqueKey;

                    if (string.IsNullOrEmpty(key))
                    {
                        do
                        {
                            key = RandomService.GenerateRandomKey();
                        } while (TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == key) != null);
                    }

                    value.UniqueKey = key;
                    TestUserData.ListOfUsers.Add(value);
                    updatedOrCreatedValue = value;
                    return true;
                }
                else
                {
                    int indexToUpdate = TestUserData.ListOfUsers.FindIndex(_ => _.UniqueKey == value.UniqueKey);
                    TestUserData.ListOfUsers[indexToUpdate] = value;
                    updatedOrCreatedValue = value;
                    return false;
                }
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.Exists"/>
            public bool Exists(string key)
            {
                return !string.IsNullOrEmpty(key) && TestUserData.ListOfUsers.Any(_ => _.UniqueKey == key);
            }

            /// <inheritdoc cref="IPamaxieUserDataInteraction.Delete"/>
            public bool Delete(PamaxieUser value)
            {
                if (value == null || string.IsNullOrEmpty(value.UniqueKey))
                {
                    return false;
                }

                PamaxieUser valueToRemove = TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == value.UniqueKey);

                if (valueToRemove == null)
                {
                    return false;
                }

                TestUserData.ListOfUsers.Remove(valueToRemove);
                return true;
            }

            /// <inheritdoc cref="IPamaxieUserDataInteraction.GetAllApplications"/>
            public IEnumerable<PamaxieApplication> GetAllApplications(PamaxieUser value)
            {
                if (value == null || string.IsNullOrEmpty(value.UniqueKey))
                {
                    return null;
                }

                List<PamaxieApplication> applications = new List<PamaxieApplication>();

                for (int i = 0; i < TestApplicationData.ListOfApplications.Count; i++)
                {
                    PamaxieApplication application = TestApplicationData.ListOfApplications[i];

                    if (application.OwnerKey == value.UniqueKey)
                    {
                        applications.Add(application);
                    }
                }

                return applications.AsEnumerable();
            }

            /// <inheritdoc cref="IPamaxieUserDataInteraction.VerifyEmail"/>
            public bool VerifyEmail(PamaxieUser value)
            {
                if (value == null || string.IsNullOrEmpty(value.UniqueKey))
                {
                    return false;
                }

                PamaxieUser valueToVerify = TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == value.UniqueKey);

                if (valueToVerify == null)
                {
                    return false;
                }

                valueToVerify.EmailVerified = true;
                return true;
            }
        }
    }
}