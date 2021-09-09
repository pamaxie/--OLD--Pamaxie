using System.Collections.Generic;
using Pamaxie.Data;
using Pamaxie.Database.Design;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// Extension methods for <see cref="PamaxieUser"/>
    /// </summary>
    public static class UserDataServiceExtension
    {
        private static IUserDataService UserService => DatabaseService.UserService;

        /// <inheritdoc cref="UserDataService.Get"/>
        public static PamaxieUser Get(string key)
        {
            return UserService.Get(key);
        }

        /// <inheritdoc cref="UserDataService.Create"/>
        public static PamaxieUser Create(this PamaxieUser value)
        {
            return UserService.Create(value as PamaxieUser);
        }

        /// <inheritdoc cref="UserDataService.TryCreate"/>
        public static bool TryCreate(this PamaxieUser value, out PamaxieUser createdValue)
        {
            return UserService.TryCreate(value as PamaxieUser, out createdValue);
        }

        /// <inheritdoc cref="UserDataService.Update"/>
        public static PamaxieUser Update(this PamaxieUser value)
        {
            return UserService.Update(value as PamaxieUser);
        }

        /// <inheritdoc cref="UserDataService.TryUpdate"/>
        public static bool TryUpdate(this PamaxieUser value, out PamaxieUser updatedValue)
        {
            return UserService.TryUpdate(value as PamaxieUser, out updatedValue);
        }

        /// <inheritdoc cref="UserDataService.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this PamaxieUser value, out PamaxieUser databaseValue)
        {
            return UserService.UpdateOrCreate(value as PamaxieUser, out databaseValue);
        }

        /// <inheritdoc cref="UserDataService.Exists"/>
        public static bool Exists(string value)
        {
            return UserService.Exists(value);
        }

        /// <inheritdoc cref="UserDataService.Delete"/>
        public static bool Delete(this PamaxieUser value)
        {
            return UserService.Delete(value as PamaxieUser);
        }

        /// <inheritdoc cref="UserDataService.GetAllApplications"/>
        public static IEnumerable<PamaxieApplication> GetAllApplications(this PamaxieUser value)
        {
            return UserService.GetAllApplications(value as PamaxieUser);
        }

        /// <inheritdoc cref="UserDataService.VerifyEmail"/>
        public static bool VerifyEmail(this PamaxieUser value)
        {
            return UserService.VerifyEmail(value as PamaxieUser);
        }
    }
}