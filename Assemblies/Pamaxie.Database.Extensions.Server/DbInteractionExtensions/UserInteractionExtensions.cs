using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.Server
{
    public static class UserInteractionExtensions
    {
        private static readonly IUserInteraction UserInteraction = new UserInteraction();
        
        /// <inheritdoc cref="IDatabaseInteraction{T}.Get"/>
        public static PamaxieUser Get(string key)
        {
            return UserInteraction.Get(key);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this PamaxieUser value, out PamaxieUser databaseValue)
        {
            return UserInteraction.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.TryUpdate"/>
        public static bool TryUpdate(this PamaxieUser value, out PamaxieUser updatedValue)
        {
            return UserInteraction.TryUpdate(value, out updatedValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Update"/>
        public static PamaxieUser Update(this PamaxieUser value)
        {
            return UserInteraction.Update(value);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.TryCreate"/>
        public static bool TryCreate(this PamaxieUser value, out PamaxieUser createdValue)
        {
            return UserInteraction.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Create"/>
        public static PamaxieUser Create(this PamaxieUser value)
        {
            return UserInteraction.Create(value);
        }
    }
}