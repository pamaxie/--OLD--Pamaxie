using System.Collections.Generic;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.Server
{
    /// <summary>
    /// Extension methods for <see cref="PamaxieApplication"/>
    /// </summary>
    public static class ApplicationInteractionExtension
    {
        private static readonly IApplicationInteraction ApplicationInteraction = new ApplicationInteraction();
        
        /// <inheritdoc cref="IDatabaseInteraction{T}.Get"/>
        public static IPamaxieApplication Get(string key)
        {
            return ApplicationInteraction.Get(key);
        }
        
        /// <inheritdoc cref="IApplicationInteraction.GetFromUser"/>
        public static IEnumerable<IPamaxieApplication> GetFromUser(string userKey)
        {
            return ApplicationInteraction.GetFromUser(userKey);
        }
        
        /// <inheritdoc cref="IDatabaseInteraction{T}.Create"/>
        public static IPamaxieApplication Create(this IPamaxieApplication value)
        {
            return ApplicationInteraction.Create(value);
        }
        
        /// <inheritdoc cref="IDatabaseInteraction{T}.TryCreate"/>
        public static bool TryCreate(this IPamaxieApplication value, out IPamaxieApplication createdValue)
        {
            return ApplicationInteraction.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Update"/>
        public static IPamaxieApplication Update(this IPamaxieApplication value)
        {
            return ApplicationInteraction.Update(value);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.TryUpdate"/>
        public static bool TryUpdate(this IPamaxieApplication value, out IPamaxieApplication updatedValue)
        {
            return ApplicationInteraction.TryUpdate(value, out updatedValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this IPamaxieApplication value, out IPamaxieApplication databaseValue)
        {
            return ApplicationInteraction.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="IDatabaseInteraction{T}.Delete"/>
        public static bool Delete(this IPamaxieApplication value)
        {
            return ApplicationInteraction.Delete(value);
        }
        
        /// <inheritdoc cref="IApplicationInteraction.EnableOrDisable"/>
        public static bool EnableOrDisable(this IPamaxieApplication value)
        {
            return ApplicationInteraction.EnableOrDisable(value);
        }
    }
}