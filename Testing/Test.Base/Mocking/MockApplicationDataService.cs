using System;
using System.Linq;
using Moq;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using Pamaxie.Database.Extensions.Client;

namespace Test.TestBase
{
    /// <summary>
    /// Class containing method for mocking <see cref="ApplicationDataService"/>.
    /// </summary>
    public static class MockApplicationDataService
    {
        private delegate void OutAction<in T, TOut>(T val, out TOut outVal);

        private static IPamaxieApplication _createdValue;
        private static bool _created;
        private static IPamaxieApplication _updatedValue;
        private static bool _updated;
        private static IPamaxieApplication _updatedOrCreatedValue;
        private static bool _updatedOrCreated;

        /// <summary>
        /// Mocks the <see cref="ApplicationDataServiceExtension"/> for testing usage
        /// </summary>
        public static void Mock()
        {
            ApplicationDataService applicationDataService = new();
            Mock<IApplicationDataService> mockApplicationDataService = new();

            //Setup for Get
            mockApplicationDataService.Setup(_ => _.Get(It.IsAny<string>()))
                .Returns<string>((key) => applicationDataService.Get(key));

            //Setup for Create
            mockApplicationDataService.Setup(_ => _.Create(It.IsAny<IPamaxieApplication>()))
                .Returns<IPamaxieApplication>((value) => applicationDataService.Create(value));

            //Setup for TryCreate
            mockApplicationDataService.Setup(_ => _.TryCreate(It.IsAny<IPamaxieApplication>(), out _createdValue))
                .Callback(new OutAction<IPamaxieApplication, IPamaxieApplication>(
                    (IPamaxieApplication value, out IPamaxieApplication createdValue) =>
                    {
                        _created = applicationDataService.TryCreate(value, out createdValue);
                        _createdValue = createdValue;
                    }))
                .Returns<IPamaxieApplication, IPamaxieApplication>((_, _) => _created);

            //Setup for Update
            mockApplicationDataService.Setup(_ => _.Update(It.IsAny<IPamaxieApplication>()))
                .Returns<IPamaxieApplication>((value) => applicationDataService.Update(value));

            //Setup for TryUpdated
            mockApplicationDataService.Setup(_ => _.TryUpdate(It.IsAny<IPamaxieApplication>(), out _updatedValue))
                .Callback(new OutAction<IPamaxieApplication, IPamaxieApplication>(
                    (IPamaxieApplication value, out IPamaxieApplication updatedValue) =>
                    {
                        _updated = applicationDataService.TryUpdate(value, out updatedValue);
                        _updatedValue = updatedValue;
                    }))
                .Returns<IPamaxieApplication, IPamaxieApplication>((_, _) => _updated);

            //Setup for UpdateOrCreate
            mockApplicationDataService
                .Setup(_ => _.UpdateOrCreate(It.IsAny<IPamaxieApplication>(), out _updatedOrCreatedValue))
                .Callback(new OutAction<IPamaxieApplication, IPamaxieApplication>(
                    (IPamaxieApplication value, out IPamaxieApplication updatedOrCreatedValue) =>
                    {
                        _updatedOrCreated = applicationDataService.UpdateOrCreate(value, out updatedOrCreatedValue);
                        _updatedOrCreatedValue = updatedOrCreatedValue;
                    }))
                .Returns<IPamaxieApplication, IPamaxieApplication>((_, _) => _updatedOrCreated);

            //Setup for Delete
            mockApplicationDataService.Setup(_ => _.Delete(It.IsAny<IPamaxieApplication>()))
                .Returns<IPamaxieApplication>((value) => applicationDataService.Delete(value));

            //Setup for EnableOrDisable
            mockApplicationDataService.Setup(_ => _.EnableOrDisable(It.IsAny<IPamaxieApplication>()))
                .Returns<IPamaxieApplication>((value) => applicationDataService.EnableOrDisable(value));

            //Setup for VerifyAuthentication
            mockApplicationDataService.Setup(_ => _.VerifyAuthentication(It.IsAny<IPamaxieApplication>()))
                .Returns<IPamaxieApplication>((value) => applicationDataService.VerifyAuthentication(value));

            DatabaseService.ApplicationService = mockApplicationDataService.Object;
        }

        /// <inheritdoc cref="IApplicationDataService"/>
        private class ApplicationDataService : IApplicationDataService
        {
            /// <inheritdoc cref="IApplicationDataService.Get"/>
            public IPamaxieApplication Get(string key)
            {
                return TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == key);
            }

            /// <inheritdoc cref="IApplicationDataService.Create"/>
            public IPamaxieApplication Create(IPamaxieApplication value)
            {
                if (value == null)
                    return null;
                if (TestApplicationData.ListOfApplications.Any(_ => _.Key == value.Key))
                    return value;
                IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.OwnerKey);
                if (user == null)
                    return null;
                user.ApplicationKeys.ToList().Add(value.OwnerKey);
                TestApplicationData.ListOfApplications.Add(value);
                return value;
            }

            /// <inheritdoc cref="IApplicationDataService.TryCreate"/>
            public bool TryCreate(IPamaxieApplication value, out IPamaxieApplication createdValue)
            {
                createdValue = null;
                if (value == null)
                    return false;
                if (TestApplicationData.ListOfApplications.Any(_ => _.Key == value.Key))
                    return false;
                IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.OwnerKey);
                if (user == null)
                    return false;
                user.ApplicationKeys.ToList().Add(value.OwnerKey);
                TestApplicationData.ListOfApplications.Add(value);
                createdValue = value;
                return true;
            }

            /// <inheritdoc cref="IApplicationDataService.Update"/>
            public IPamaxieApplication Update(IPamaxieApplication value)
            {
                if (value == null)
                    return null;
                if (TestApplicationData.ListOfApplications.Any(_ => _.Key == value.Key))
                    return value;
                TestApplicationData.ListOfApplications.Add(value);
                return value;
            }

            /// <inheritdoc cref="IApplicationDataService.TryUpdate"/>
            public bool TryUpdate(IPamaxieApplication value, out IPamaxieApplication updatedValue)
            {
                updatedValue = null;
                if (value == null)
                    return false;
                int indexToUpdate = TestApplicationData.ListOfApplications.FindIndex(_ => _.Key == value.Key);
                if (indexToUpdate == -1)
                    return false;
                TestApplicationData.ListOfApplications[indexToUpdate] = value;
                updatedValue = value;
                return true;
            }

            /// <inheritdoc cref="IApplicationDataService.UpdateOrCreate"/>
            public bool UpdateOrCreate(IPamaxieApplication value, out IPamaxieApplication updatedOrCreatedValue)
            {
                updatedOrCreatedValue = null;
                if (value == null)
                    return false;
                if (TestApplicationData.ListOfApplications.Any(_ => _.Key != value.Key))
                {
                    IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.OwnerKey);
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

            /// <inheritdoc cref="IApplicationDataService.Delete"/>
            public bool Delete(IPamaxieApplication value)
            {
                if (value == null)
                    return false;
                IPamaxieApplication valueToRemove =
                    TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == value.Key);
                if (valueToRemove == null)
                    return false;
                TestApplicationData.ListOfApplications.Remove(valueToRemove);
                return true;
            }

            /// <inheritdoc cref="IApplicationDataService.EnableOrDisable"/>
            public IPamaxieApplication EnableOrDisable(IPamaxieApplication value)
            {
                if (value == null)
                    return null;
                IPamaxieApplication valueToEnableOrDisable =
                    TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == value.Key);
                if (valueToEnableOrDisable == null)
                    return null;
                valueToEnableOrDisable.Disabled = !valueToEnableOrDisable.Disabled;
                return valueToEnableOrDisable;
            }

            /// <inheritdoc cref="IApplicationDataService.VerifyAuthentication"/>
            public bool VerifyAuthentication(IPamaxieApplication value)
            {
                if (value == null)
                    return false;
                AppAuthCredentials appAuthCredentials =
                    TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == value.Key)?.Credentials;
                if (appAuthCredentials == null)
                    return false;
                string hashedToken = BCrypt.Net.BCrypt.HashPassword(appAuthCredentials.AuthorizationToken,
                    BCryptExtension.CalculateSaltCost());
                return appAuthCredentials.AuthorizationTokenCipher == hashedToken;
            }
        }
    }
}