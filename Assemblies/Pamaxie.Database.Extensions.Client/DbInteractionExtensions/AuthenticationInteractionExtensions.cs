using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// Extension methods for <see cref="AppAuthCredentials"/>
    /// </summary>
    public static class AuthenticationInteractionExtensions
    {
        private static readonly IAuthenticationDataService AuthenticationDataService = new AuthenticationDataService();
        
        /// <inheritdoc cref="IDataServiceBase{T}.Get"/>
        public static AppAuthCredentials Get(string key)
        {
            return AuthenticationDataService.Get(key);
        }
        
        /// <inheritdoc cref="IDataServiceBase{T}.Create"/>
        public static AppAuthCredentials Create(this AppAuthCredentials value)
        {
            return AuthenticationDataService.Create(value);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.TryCreate"/>
        public static bool TryCreate(this AppAuthCredentials value, out AppAuthCredentials createdValue)
        {
            return AuthenticationDataService.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.Update"/>
        public static AppAuthCredentials Update(this AppAuthCredentials value)
        {
            return AuthenticationDataService.Update(value);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.TryUpdate"/>
        public static bool TryUpdate(this AppAuthCredentials value, out AppAuthCredentials updatedValue)
        {
            return AuthenticationDataService.TryUpdate(value, out updatedValue);
        }
        
        /// <inheritdoc cref="IDataServiceBase{T}.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this AppAuthCredentials value, out AppAuthCredentials databaseValue)
        {
            return AuthenticationDataService.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="IAuthenticationDataService.VerifyAuthentication"/>
        public static bool VerifyAuthentication(this AppAuthCredentials value)
        {
            return AuthenticationDataService.VerifyAuthentication(value);
        }
    }
}