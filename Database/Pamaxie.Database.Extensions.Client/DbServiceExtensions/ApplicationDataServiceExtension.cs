using Pamaxie.Data;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// Extension methods for <see cref="IPamaxieApplication"/>
    /// </summary>
    public static class ApplicationDataServiceExtension
    {
        private static ApplicationDataService ApplicationService => DatabaseService.ApplicationService;
        
        /// <inheritdoc cref="ApplicationDataService.Get"/>
        public static IPamaxieApplication Get(string key)
        {
            return ApplicationService.Get(key);
        }
        
        /// <inheritdoc cref="ApplicationDataService.Create"/>
        public static IPamaxieApplication Create(this IPamaxieApplication value)
        {
            return ApplicationService.Create(value);
        }
        
        /// <inheritdoc cref="ApplicationDataService.TryCreate"/>
        public static bool TryCreate(this IPamaxieApplication value, out IPamaxieApplication createdValue)
        {
            return ApplicationService.TryCreate(value, out createdValue);
        }

        /// <inheritdoc cref="ApplicationDataService.Update"/>
        public static IPamaxieApplication Update(this IPamaxieApplication value)
        {
            return ApplicationService.Update(value);
        }

        /// <inheritdoc cref="ApplicationDataService.TryUpdate"/>
        public static bool TryUpdate(this IPamaxieApplication value, out IPamaxieApplication updatedValue)
        {
            return ApplicationService.TryUpdate(value, out updatedValue);
        }

        /// <inheritdoc cref="ApplicationDataService.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this IPamaxieApplication value, out IPamaxieApplication databaseValue)
        {
            return ApplicationService.UpdateOrCreate(value, out databaseValue);
        }

        /// <inheritdoc cref="ApplicationDataService.Delete"/>
        public static bool Delete(this IPamaxieApplication value)
        {
            return ApplicationService.Delete(value);
        }
        
        /// <inheritdoc cref="ApplicationDataService.EnableOrDisable"/>
        public static IPamaxieApplication EnableOrDisable(this IPamaxieApplication value)
        {
            return ApplicationService.EnableOrDisable(value);
        }

        /// <inheritdoc cref="ApplicationDataService.VerifyAuthentication"/>
        public static bool VerifyAuthentication(this IPamaxieApplication value)
        {
            return ApplicationService.VerifyAuthentication(value);
        }
    }
}