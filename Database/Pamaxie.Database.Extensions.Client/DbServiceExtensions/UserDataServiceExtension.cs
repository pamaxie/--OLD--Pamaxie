using System.Collections.Generic;
using Pamaxie.Data;

namespace Pamaxie.Database.Extensions.Client
{
    public static class UserDataServiceExtension
    {
        private static UserDataService UserService => DatabaseService.UserService;
        
        /// <inheritdoc cref="UserDataService.Get"/>
        public static IPamaxieUser Get(string key)
        {
            return UserService.Get(key);
        }
        
        /// <inheritdoc cref="UserDataService.Create"/>
        public static IPamaxieUser Create(this IPamaxieUser value)
        {
            return UserService.Create(value);
        }
        
        /// <inheritdoc cref="UserDataService.TryCreate"/>
        public static bool TryCreate(this IPamaxieUser value, out IPamaxieUser createdValue)
        {
            return UserService.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="UserDataService.Update"/>
        public static IPamaxieUser Update(this IPamaxieUser value)
        {
            return UserService.Update(value);
        }

        /// <inheritdoc cref="UserDataService.TryUpdate"/>
        public static bool TryUpdate(this IPamaxieUser value, out IPamaxieUser updatedValue)
        {
            return UserService.TryUpdate(value, out updatedValue);
        }

        /// <inheritdoc cref="UserDataService.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this IPamaxieUser value, out IPamaxieUser databaseValue)
        {
            return UserService.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="UserDataService.Delete"/>
        public static bool Delete(this IPamaxieUser value)
        {
            return UserService.Delete(value);
        }
        
        /// <inheritdoc cref="UserDataService.GetAllApplicationsFromUser"/>
        public static IEnumerable<IPamaxieApplication> GetAllApplications(this IPamaxieUser value)
        {
            return UserService.GetAllApplications(value);
        }
        /// <inheritdoc cref="UserDataService.VerifyEmail"/>
        public static bool VerifyEmail(this IPamaxieUser value)
        {
            return UserService.VerifyEmail(value);
        }
    }
}