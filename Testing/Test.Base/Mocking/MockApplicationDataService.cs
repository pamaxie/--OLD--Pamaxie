using System;
using System.Linq;
using Moq;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using Pamaxie.Database.Extensions.Client;

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
        public static void Mock()
        {
            ApplicationDataService applicationDataService = new ApplicationDataService();
            Mock<IApplicationDataService> mockApplicationDataService = new Mock<IApplicationDataService>();

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

            DatabaseService.ApplicationService = mockApplicationDataService.Object;
        }

        /// <inheritdoc cref="IApplicationDataService"/>
        private sealed class ApplicationDataService : IApplicationDataService
        {
            /// <inheritdoc cref="IApplicationDataService.Get"/>
            public PamaxieApplication Get(string key)
            {
                return string.IsNullOrEmpty(key)
                    ? null
                    : TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == key) as PamaxieApplication;
            }

            /// <inheritdoc cref="IApplicationDataService.Create"/>
            public PamaxieApplication Create(PamaxieApplication value)
            {
                if (value == null || !string.IsNullOrEmpty(value.Key))
                    return null;

                string key;
                do
                {
                    key = RandomService.GenerateRandomKey(6);
                } while (TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == key) != null);

                value.Key = key;
                PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.OwnerKey);
                if (user == null)
                    return null;
                user.ApplicationKeys.ToList().Add(value.OwnerKey);
                TestApplicationData.ListOfApplications.Add(value);
                return value;
            }

            /// <inheritdoc cref="IApplicationDataService.TryCreate"/>
            public bool TryCreate(PamaxieApplication value, out PamaxieApplication createdValue)
            {
                createdValue = null;
                if (value == null || !string.IsNullOrEmpty(value.Key))
                    return false;
                string key;
                do
                {
                    key = RandomService.GenerateRandomKey(6);
                } while (TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == key) != null);

                value.Key = key;

                PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.OwnerKey);
                if (user == null)
                    return false;
                user.ApplicationKeys.ToList().Add(value.OwnerKey);
                TestApplicationData.ListOfApplications.Add(value);
                createdValue = value;
                return true;
            }

            /// <inheritdoc cref="IApplicationDataService.Update"/>
            public PamaxieApplication Update(PamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.Key))
                    return null;
                if (TestApplicationData.ListOfApplications.Any(_ => _.Key == value.Key))
                    return value;
                TestApplicationData.ListOfApplications.Add(value);
                return value;
            }

            /// <inheritdoc cref="IApplicationDataService.TryUpdate"/>
            public bool TryUpdate(PamaxieApplication value, out PamaxieApplication updatedValue)
            {
                updatedValue = null;
                if (value == null || string.IsNullOrEmpty(value.Key))
                    return false;
                int indexToUpdate = TestApplicationData.ListOfApplications.FindIndex(_ => _.Key == value.Key);
                if (indexToUpdate == -1)
                    return false;
                TestApplicationData.ListOfApplications[indexToUpdate] = value;
                updatedValue = value;
                return true;
            }

            /// <inheritdoc cref="IApplicationDataService.UpdateOrCreate"/>
            public bool UpdateOrCreate(PamaxieApplication value, out PamaxieApplication updatedOrCreatedValue)
            {
                updatedOrCreatedValue = null;
                if (value == null || string.IsNullOrEmpty(value.Key))
                    return false;
                if (TestApplicationData.ListOfApplications.Any(_ => _.Key != value.Key))
                {
                    string key;
                    do
                    {
                        key = RandomService.GenerateRandomKey(6);
                    } while (TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == key) != null);

                    value.Key = key;
                    PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.OwnerKey);
                    if (user == null)
                        return false;
                    user.ApplicationKeys.ToList().Add(value.OwnerKey);
                    TestApplicationData.ListOfApplications.Add(value);
                }
                else
                {
                    int indexToUpdate = TestApplicationData.ListOfApplications.FindIndex(_ => _.Key == value.Key);
                    TestApplicationData.ListOfApplications[indexToUpdate] = value;
                }

                updatedOrCreatedValue = value;
                return true;
            }

            /// <inheritdoc cref="IApplicationDataService.Exists"/>
            public bool Exists(string key)
            {
                return !string.IsNullOrEmpty(key) && TestApplicationData.ListOfApplications.Any(_ => _.Key == key);
            }

            /// <inheritdoc cref="IApplicationDataService.Delete"/>
            public bool Delete(PamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.Key))
                    return false;
                PamaxieApplication valueToRemove =
                    TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == value.Key);
                if (valueToRemove == null)
                    return false;
                TestApplicationData.ListOfApplications.Remove(valueToRemove);
                return true;
            }

            /// <inheritdoc cref="IApplicationDataService.GetOwner"/>
            public PamaxieUser GetOwner(PamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.Key) || string.IsNullOrEmpty(value.OwnerKey))
                    return null;
                PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.OwnerKey);
                return user as PamaxieUser;
            }

            /// <inheritdoc cref="IApplicationDataService.EnableOrDisable"/>
            public PamaxieApplication EnableOrDisable(PamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.Key))
                    return null;
                PamaxieApplication valueToEnableOrDisable =
                    TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == value.Key);
                if (valueToEnableOrDisable == null)
                    return null;
                valueToEnableOrDisable.Disabled = !valueToEnableOrDisable.Disabled;
                return valueToEnableOrDisable as PamaxieApplication;
            }

            /// <inheritdoc cref="IApplicationDataService.VerifyAuthentication"/>
            public bool VerifyAuthentication(PamaxieApplication value)
            {
                throw new NotImplementedException(); //TODO
            }
        }
    }
}