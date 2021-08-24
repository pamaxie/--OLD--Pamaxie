using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// Extension methods for <see cref="IPamaxieUser"/>
    /// </summary>
    public static class UserInteractionExtensions
    {
        private static readonly IUserDataService UserDataService = new UserDataService();
        
        /// <inheritdoc cref="IDataServiceBase{T}.Get"/>
        public static IPamaxieUser Get(string key)
        {
            return UserDataService.Get(key);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.Create"/>
        public static IPamaxieUser Create(this IPamaxieUser value)
        {
            return UserDataService.Create(value);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.TryCreate"/>
        public static bool TryCreate(this IPamaxieUser value, out IPamaxieUser createdValue)
        {
            return UserDataService.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.Update"/>
        public static IPamaxieUser Update(this IPamaxieUser value)
        {
            return UserDataService.Update(value);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.TryUpdate"/>
        public static bool TryUpdate(this IPamaxieUser value, out IPamaxieUser updatedValue)
        {
            return UserDataService.TryUpdate(value, out updatedValue);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this IPamaxieUser value, out IPamaxieUser databaseValue)
        {
            return UserDataService.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.Delete"/>
        public static bool Delete(this IPamaxieUser value)
        {
            return UserDataService.Delete(value);
        }
        
        /// <inheritdoc cref="IUserDataService.VerifyEmail"/>
        public static bool VerifyEmail(this IPamaxieUser value)
        {
            return UserDataService.VerifyEmail(value);
        }
    }
}