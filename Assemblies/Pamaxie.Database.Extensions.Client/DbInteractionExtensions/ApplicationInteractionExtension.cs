using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.Client
{
    public static class ApplicationInteractionExtension
    {
        private static readonly IApplicationInteraction ApplicationInteraction = new ApplicationInteraction();
        
        /// <inheritdoc cref="IDatabaseInteraction{T}.Get"/>
        public static PamaxieApplication Get(string key)
        {
            return ApplicationInteraction.Get(key);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this PamaxieApplication value, out PamaxieApplication databaseValue)
        {
            return ApplicationInteraction.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.TryUpdate"/>
        public static bool TryUpdate(this PamaxieApplication value, out PamaxieApplication updatedValue)
        {
            return ApplicationInteraction.TryUpdate(value, out updatedValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Update"/>
        public static PamaxieApplication Update(this PamaxieApplication value)
        {
            return ApplicationInteraction.Update(value);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.TryCreate"/>
        public static bool TryCreate(this PamaxieApplication value, out PamaxieApplication createdValue)
        {
            return ApplicationInteraction.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Create"/>
        public static PamaxieApplication Create(this PamaxieApplication value)
        {
            return ApplicationInteraction.Create(value);
        }
    }
}