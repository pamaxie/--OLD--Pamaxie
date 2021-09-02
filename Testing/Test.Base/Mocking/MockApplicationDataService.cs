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

        /// <summary>
        /// Mocks the <see cref="ApplicationDataService"/> and applies it to the <see cref="ApplicationDataServiceExtension"/> for testing usage
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
            IPamaxieApplication createdValue = null;
            bool created = false;
            mockApplicationDataService.Setup(_ => _.TryCreate(It.IsAny<IPamaxieApplication>(), out createdValue))
                .Callback(new OutAction<IPamaxieApplication, IPamaxieApplication>(
                    (IPamaxieApplication value, out IPamaxieApplication outValue) =>
                    {
                        created = applicationDataService.TryCreate(value, out outValue);
                        createdValue = outValue;
                    }))
                .Returns<IPamaxieApplication, IPamaxieApplication>((_, _) => created);

            //Setup for Update
            mockApplicationDataService.Setup(_ => _.Update(It.IsAny<IPamaxieApplication>()))
                .Returns<IPamaxieApplication>((value) => applicationDataService.Update(value));

            //Setup for TryUpdated
            IPamaxieApplication updatedValue = null;
            bool updated = false;
            mockApplicationDataService.Setup(_ => _.TryUpdate(It.IsAny<IPamaxieApplication>(), out updatedValue))
                .Callback(new OutAction<IPamaxieApplication, IPamaxieApplication>(
                    (IPamaxieApplication value, out IPamaxieApplication outValue) =>
                    {
                        updated = applicationDataService.TryUpdate(value, out outValue);
                        updatedValue = outValue;
                    }))
                .Returns<IPamaxieApplication, IPamaxieApplication>((_, _) => updated);

            //Setup for UpdateOrCreate
            IPamaxieApplication updatedOrCreatedValue = null;
            bool updatedOrCreated = false;
            mockApplicationDataService
                .Setup(_ => _.UpdateOrCreate(It.IsAny<IPamaxieApplication>(), out updatedOrCreatedValue))
                .Callback(new OutAction<IPamaxieApplication, IPamaxieApplication>(
                    (IPamaxieApplication value, out IPamaxieApplication outValue) =>
                    {
                        updatedOrCreated = applicationDataService.UpdateOrCreate(value, out outValue);
                        updatedOrCreatedValue = outValue;
                    }))
                .Returns<IPamaxieApplication, IPamaxieApplication>((_, _) => updatedOrCreated);

            //Setup for Delete
            mockApplicationDataService.Setup(_ => _.Delete(It.IsAny<IPamaxieApplication>()))
                .Returns<IPamaxieApplication>((value) => applicationDataService.Delete(value));

            //Setup for GetOwner
            mockApplicationDataService.Setup(_ => _.GetOwner(It.IsAny<IPamaxieApplication>()))
                .Returns<IPamaxieApplication>((value) => applicationDataService.GetOwner(value));
            
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
                return string.IsNullOrEmpty(key)
                    ? null
                    : TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == key);
            }

            /// <inheritdoc cref="IApplicationDataService.Create"/>
            public IPamaxieApplication Create(IPamaxieApplication value)
            {
                if (value == null || !string.IsNullOrEmpty(value.Key))
                    return null;
                
                string key;
                do
                {
                    key = RandomService.GenerateRandomKey(6);
                } while (TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == key) != null);

                value.Key = key;
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
                if (value == null || !string.IsNullOrEmpty(value.Key))
                    return false;
                string key;
                do
                {
                    key = RandomService.GenerateRandomKey(6);
                } while (TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == key) != null);

                value.Key = key;
                
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
                if (value == null || string.IsNullOrEmpty(value.Key))
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
            public bool UpdateOrCreate(IPamaxieApplication value, out IPamaxieApplication updatedOrCreatedValue)
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
                if (value == null || string.IsNullOrEmpty(value.Key))
                    return false;
                IPamaxieApplication valueToRemove =
                    TestApplicationData.ListOfApplications.FirstOrDefault(_ => _.Key == value.Key);
                if (valueToRemove == null)
                    return false;
                TestApplicationData.ListOfApplications.Remove(valueToRemove);
                return true;
            }

            /// <inheritdoc cref="IApplicationDataService.GetOwner"/>
            public IPamaxieUser GetOwner(IPamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.Key) || string.IsNullOrEmpty(value.OwnerKey))
                    return null;
                IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == value.OwnerKey);
                return user;
            }

            /// <inheritdoc cref="IApplicationDataService.EnableOrDisable"/>
            public IPamaxieApplication EnableOrDisable(IPamaxieApplication value)
            {
                if (value == null || string.IsNullOrEmpty(value.Key))
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
                throw new NotImplementedException(); //TODO
            }
        }
    }
}