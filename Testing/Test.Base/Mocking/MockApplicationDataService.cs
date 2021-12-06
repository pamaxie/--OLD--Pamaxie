using System;
using System.Linq;
using Moq;
using Pamaxie.Data;
using Pamaxie.Database.Design;

namespace Test.Base
{
    /// <summary>
    /// Class containing method for mocking <see cref="ApplicationDataService"/>.
    /// </summary>
    public static class MockApplicationDataService
    {
        private delegate void OutAction<in T, TOut>(T val, out TOut outVal);

        /// <summary>
        /// Mocks the <see cref="ApplicationDataService"/> and applies it to the ApplicationDataServiceExtension for testing usage
        /// </summary>
        public static IDatabasePamaxieApplicationInteraction Mock()
        {
            ApplicationDataService applicationDataService = new ApplicationDataService();
            Mock<IDatabasePamaxieApplicationInteraction> mockApplicationDataService = new Mock<IDatabasePamaxieApplicationInteraction>();

            //Setup for Get
            mockApplicationDataService.Setup(_ => _.Get(It.IsAny<string>()))
                .Returns<string>((key) => applicationDataService.Get(key));

            //Setup for Create
            mockApplicationDataService.Setup(_ => _.Create(It.IsAny<PamaxieApplication>()))
                .Returns<PamaxieApplication>((value) => applicationDataService.Create(value));

            //Setup for TryCreate
            PamaxieApplication createdValue = null;
            bool created = false;
            mockApplicationDataService.Setup(_ => _.TryCreate(It.IsAny<PamaxieApplication>(), out createdValue))
                .Callback(new OutAction<PamaxieApplication, PamaxieApplication>(
                    (PamaxieApplication value, out PamaxieApplication outValue) =>
                    {
                        created = applicationDataService.TryCreate(value, out outValue);
                        createdValue = outValue;
                    }))
                .Returns<PamaxieApplication, PamaxieApplication>((_, _) => created);

            //Setup for Update
            mockApplicationDataService.Setup(_ => _.Update(It.IsAny<PamaxieApplication>()))
                .Returns<PamaxieApplication>((value) => applicationDataService.Update(value));

            //Setup for TryUpdated
            PamaxieApplication updatedValue = null;
            bool updated = false;
            mockApplicationDataService.Setup(_ => _.TryUpdate(It.IsAny<PamaxieApplication>(), out updatedValue))
                .Callback(new OutAction<PamaxieApplication, PamaxieApplication>(
                    (PamaxieApplication value, out PamaxieApplication outValue) =>
                    {
                        updated = applicationDataService.TryUpdate(value, out outValue);
                        updatedValue = outValue;
                    }))
                .Returns<PamaxieApplication, PamaxieApplication>((_, _) => updated);

            //Setup for UpdateOrCreate
            PamaxieApplication updatedOrCreatedValue = null;
            bool updatedOrCreated = false;
            mockApplicationDataService
                .Setup(_ => _.UpdateOrCreate(It.IsAny<PamaxieApplication>(), out updatedOrCreatedValue))
                .Callback(new OutAction<PamaxieApplication, PamaxieApplication>(
                    (PamaxieApplication value, out PamaxieApplication outValue) =>
                    {
                        updatedOrCreated = applicationDataService.UpdateOrCreate(value, out outValue);
                        updatedOrCreatedValue = outValue;
                    }))
                .Returns<PamaxieApplication, PamaxieApplication>((_, _) => updatedOrCreated);

            //Setup for Delete
            mockApplicationDataService.Setup(_ => _.Delete(It.IsAny<PamaxieApplication>()))
                .Returns<PamaxieApplication>((value) => applicationDataService.Delete(value));

            //Setup for GetOwner
            mockApplicationDataService.Setup(_ => _.GetOwner(It.IsAny<PamaxieApplication>()))
                .Returns<PamaxieApplication>((value) => applicationDataService.GetOwner(value));

            //Setup for EnableOrDisable
            mockApplicationDataService.Setup(_ => _.EnableOrDisable(It.IsAny<PamaxieApplication>()))
                .Returns<PamaxieApplication>((value) => applicationDataService.EnableOrDisable(value));

            //Setup for VerifyAuthentication
            mockApplicationDataService.Setup(_ => _.VerifyAuthentication(It.IsAny<PamaxieApplication>()))
                .Returns<PamaxieApplication>((value) => applicationDataService.VerifyAuthentication(value));

            return mockApplicationDataService.Object;
        }

        /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction"/>
        private sealed class ApplicationDataService : IDatabasePamaxieApplicationInteraction
        {
            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.Get"/>
            public PamaxieApplication Get(string key)
            {
                return string.IsNullOrEmpty(key)
                    ? null
                    : TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.UniqueKey == key);
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.Create"/>
            public PamaxieApplication Create(PamaxieApplication value)
            {
                if (value == null || !string.IsNullOrEmpty(value.UniqueKey))
                {
                    return null;
                }

                string key;
                do
                {
                    key = RandomService.GenerateRandomKey(6);
                } while (TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.UniqueKey == key) != null);

                value.UniqueKey = key;

                PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == value.OwnerKey);

                if (user == null)
                {
                    return null;
                }

                user.ApplicationKeys.ToList().Add(value.OwnerKey);
                TestApplicationData.ListOfApplications.Add(value);
                return value;
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.TryCreate"/>
            public bool TryCreate(PamaxieApplication value, out PamaxieApplication createdValue)
            {
                createdValue = null;

                if (value == null || !string.IsNullOrEmpty(value.UniqueKey))
                {
                    return false;
                }

                string key;
                do
                {
                    key = RandomService.GenerateRandomKey(6);
                } while (TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.UniqueKey == key) != null);

                value.UniqueKey = key;

                PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == value.OwnerKey);

                if (user == null)
                {
                    return false;
                }

                user.ApplicationKeys.ToList().Add(value.OwnerKey);
                TestApplicationData.ListOfApplications.Add(value);
                createdValue = value;
                return true;
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.Update"/>
            public PamaxieApplication Update(PamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.UniqueKey))
                {
                    return null;
                }

                if (TestApplicationData.ListOfApplications.Any(_ => _.UniqueKey == value.UniqueKey))
                {
                    return value;
                }

                TestApplicationData.ListOfApplications.Add(value);
                return value;
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.TryUpdate"/>
            public bool TryUpdate(PamaxieApplication value, out PamaxieApplication updatedValue)
            {
                updatedValue = null;

                if (value == null || string.IsNullOrEmpty(value.UniqueKey))
                {
                    return false;
                }

                int indexToUpdate = TestApplicationData.ListOfApplications.FindIndex(_ => _.UniqueKey == value.UniqueKey);

                if (indexToUpdate == -1)
                {
                    return false;
                }

                TestApplicationData.ListOfApplications[indexToUpdate] = value;
                updatedValue = value;
                return true;
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.UpdateOrCreate"/>
            public bool UpdateOrCreate(PamaxieApplication value, out PamaxieApplication updatedOrCreatedValue)
            {
                updatedOrCreatedValue = null;

                if (value == null)
                {
                    throw new Exception("Bad data");
                }

                if (string.IsNullOrEmpty(value.UniqueKey) ||
                    TestApplicationData.ListOfApplications.All(_ => _.UniqueKey != value.UniqueKey))
                {
                    string key = value.UniqueKey;

                    if (string.IsNullOrEmpty(key))
                    {
                        do
                        {
                            key = RandomService.GenerateRandomKey(6);
                        } while (TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.UniqueKey == key) != null);
                    }

                    value.UniqueKey = key;
                    PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == value.OwnerKey);

                    if (user == null)
                    {
                        throw new Exception("No owner for the application");
                    }

                    user.ApplicationKeys.ToList().Add(value.OwnerKey);
                    TestApplicationData.ListOfApplications.Add(value);
                    updatedOrCreatedValue = value;
                    return true;
                }
                else
                {
                    int indexToUpdate = TestApplicationData.ListOfApplications.FindIndex(_ => _.UniqueKey == value.UniqueKey);
                    TestApplicationData.ListOfApplications[indexToUpdate] = value;
                    updatedOrCreatedValue = value;
                    return false;
                }
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.Exists"/>
            public bool Exists(string key)
            {
                return !string.IsNullOrEmpty(key) && TestApplicationData.ListOfApplications.Any(_ => _.UniqueKey == key);
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.Delete"/>
            public bool Delete(PamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.UniqueKey))
                {
                    return false;
                }

                PamaxieApplication valueToRemove =
                    TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.UniqueKey == value.UniqueKey);

                if (valueToRemove == null)
                {
                    return false;
                }

                TestApplicationData.ListOfApplications.Remove(valueToRemove);
                return true;
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.GetOwner"/>
            public PamaxieUser GetOwner(PamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.UniqueKey) || string.IsNullOrEmpty(value.OwnerKey))
                {
                    return null;
                }

                PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.UniqueKey == value.OwnerKey);
                return user;
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.EnableOrDisable"/>
            public PamaxieApplication EnableOrDisable(PamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.UniqueKey))
                {
                    return null;
                }

                PamaxieApplication valueToEnableOrDisable =
                    TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.UniqueKey == value.UniqueKey);

                if (valueToEnableOrDisable == null)
                {
                    return null;
                }

                valueToEnableOrDisable.Disabled = !valueToEnableOrDisable.Disabled;
                return valueToEnableOrDisable;
            }

            /// <inheritdoc cref="IDatabasePamaxieApplicationInteraction.VerifyAuthentication"/>
            public bool VerifyAuthentication(PamaxieApplication value)
            {
                throw new NotImplementedException();
            }
        }
    }
}