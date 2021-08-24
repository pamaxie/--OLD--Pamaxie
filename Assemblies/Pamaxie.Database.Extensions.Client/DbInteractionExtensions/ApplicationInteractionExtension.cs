using System.Collections.Generic;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// Extension methods for <see cref="PamaxieApplication"/>
    /// </summary>
    public static class ApplicationInteractionExtension
    {
        private static readonly IApplicationDataService ApplicationDataService = new ApplicationDataService();
        
        /// <inheritdoc cref="IDataServiceBase{T}.Get"/>
        public static IPamaxieApplication Get(string key)
        {
            return ApplicationDataService.Get(key);
        }
        
        /// <inheritdoc cref="IApplicationDataService.GetFromUser"/>
        public static IEnumerable<IPamaxieApplication> GetFromUser(string value)
        {
            return ApplicationDataService.GetFromUser(value);
        }
        
        /// <inheritdoc cref="IDataServiceBase{T}.Create"/>
        public static IPamaxieApplication Create(this IPamaxieApplication value)
        {
            return ApplicationDataService.Create(value);
        }
        
        /// <inheritdoc cref="IDataServiceBase{T}.TryCreate"/>
        public static bool TryCreate(this IPamaxieApplication value, out IPamaxieApplication createdValue)
        {
            return ApplicationDataService.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.Update"/>
        public static IPamaxieApplication Update(this IPamaxieApplication value)
        {
            return ApplicationDataService.Update(value);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.TryUpdate"/>
        public static bool TryUpdate(this IPamaxieApplication value, out IPamaxieApplication updatedValue)
        {
            return ApplicationDataService.TryUpdate(value, out updatedValue);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this IPamaxieApplication value, out IPamaxieApplication databaseValue)
        {
            return ApplicationDataService.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="IDataServiceBase{T}.Delete"/>
        public static bool Delete(this IPamaxieApplication value)
        {
            return ApplicationDataService.Delete(value);
        }
        
        /// <inheritdoc cref="IApplicationDataService.EnableOrDisable"/>
        public static bool EnableOrDisable(this IPamaxieApplication value)
        {
            return ApplicationDataService.EnableOrDisable(value);
        }
    }
}