using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.Client
{
    public static class UserInteractionExtensions
    {
        private static readonly IUserInteraction UserInteraction = new UserInteraction();
        
        /// <inheritdoc cref="IDatabaseInteraction{T}.Get"/>
        public static IPamaxieUser Get(string key)
        {
            return UserInteraction.Get(key);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this IPamaxieUser value, out IPamaxieUser databaseValue)
        {
            return UserInteraction.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.TryUpdate"/>
        public static bool TryUpdate(this IPamaxieUser value, out IPamaxieUser updatedValue)
        {
            return UserInteraction.TryUpdate(value, out updatedValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Update"/>
        public static IPamaxieUser Update(this IPamaxieUser value)
        {
            return UserInteraction.Update(value);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.TryCreate"/>
        public static bool TryCreate(this IPamaxieUser value, out IPamaxieUser createdValue)
        {
            return UserInteraction.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Create"/>
        public static IPamaxieUser Create(this IPamaxieUser value)
        {
            return UserInteraction.Create(value);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Delete"/>
        public static bool Delete(this IPamaxieUser value)
        {
            return UserInteraction.Delete(value);
        }
        
        /// <inheritdoc cref="IUserInteraction.VerifyEmail"/>
        public static bool VerifyEmail(this IPamaxieUser value)
        {
            return UserInteraction.VerifyEmail(value);
        }
    }
}