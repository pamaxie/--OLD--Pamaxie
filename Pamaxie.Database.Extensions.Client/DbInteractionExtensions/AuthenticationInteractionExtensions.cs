using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.Client
{
    public static class AuthenticationInteractionExtensions
    {
        private static readonly IAuthenticationInteraction AuthenticationInteraction = new AuthenticationInteraction();
        
        /// <inheritdoc cref="IDatabaseInteraction{T}.Get"/>
        public static AppAuthCredentials Get(string key)
        {
            return AuthenticationInteraction.Get(key);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this AppAuthCredentials value, out AppAuthCredentials databaseValue)
        {
            return AuthenticationInteraction.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.TryUpdate"/>
        public static bool TryUpdate(this AppAuthCredentials value, out AppAuthCredentials updatedValue)
        {
            return AuthenticationInteraction.TryUpdate(value, out updatedValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Update"/>
        public static AppAuthCredentials Update(this AppAuthCredentials value)
        {
            return AuthenticationInteraction.Update(value);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.TryCreate"/>
        public static bool TryCreate(this AppAuthCredentials value, out AppAuthCredentials createdValue)
        {
            return AuthenticationInteraction.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Create"/>
        public static AppAuthCredentials Create(this AppAuthCredentials value)
        {
            return AuthenticationInteraction.Create(value);
        }
    }
}