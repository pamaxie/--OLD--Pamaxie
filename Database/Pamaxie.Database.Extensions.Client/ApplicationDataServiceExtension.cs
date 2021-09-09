using Pamaxie.Data;
using Pamaxie.Database.Design;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// Extension methods for <see cref="PamaxieApplication"/>
    /// </summary>
    public static class ApplicationDataServiceExtension
    {
        private static IApplicationDataService ApplicationService => DatabaseService.ApplicationService;

        /// <inheritdoc cref="ApplicationDataService.Get"/>
        public static PamaxieApplication Get(string key)
        {
            return ApplicationService.Get(key);
        }

        /// <inheritdoc cref="ApplicationDataService.Create"/>
        public static PamaxieApplication Create(this PamaxieApplication value)
        {
            return ApplicationService.Create(value as PamaxieApplication);
        }

        /// <inheritdoc cref="ApplicationDataService.TryCreate"/>
        public static bool TryCreate(this PamaxieApplication value, out PamaxieApplication createdValue)
        {
            return ApplicationService.TryCreate(value as PamaxieApplication, out createdValue);
        }

        /// <inheritdoc cref="ApplicationDataService.Update"/>
        public static PamaxieApplication Update(this PamaxieApplication value)
        {
            return ApplicationService.Update(value as PamaxieApplication);
        }

        /// <inheritdoc cref="ApplicationDataService.TryUpdate"/>
        public static bool TryUpdate(this PamaxieApplication value, out PamaxieApplication updatedValue)
        {
            return ApplicationService.TryUpdate(value as PamaxieApplication, out updatedValue);
        }

        /// <inheritdoc cref="ApplicationDataService.UpdateOrCreate"/>
        public static bool UpdateOrCreate(this PamaxieApplication value, out PamaxieApplication databaseValue)
        {
            return ApplicationService.UpdateOrCreate(value as PamaxieApplication, out databaseValue);
        }

        /// <inheritdoc cref="ApplicationDataService.Exists"/>
        public static bool Exists(string value)
        {
            return ApplicationService.Exists(value);
        }

        /// <inheritdoc cref="ApplicationDataService.Delete"/>
        public static bool Delete(this PamaxieApplication value)
        {
            return ApplicationService.Delete(value as PamaxieApplication);
        }

        /// <inheritdoc cref="ApplicationDataService.GetOwner"/>
        public static PamaxieUser GetOwner(this PamaxieApplication value)
        {
            return ApplicationService.GetOwner(value as PamaxieApplication);
        }

        /// <inheritdoc cref="ApplicationDataService.EnableOrDisable"/>
        public static PamaxieApplication EnableOrDisable(this PamaxieApplication value)
        {
            return ApplicationService.EnableOrDisable(value as PamaxieApplication);
        }

        /// <inheritdoc cref="ApplicationDataService.VerifyAuthentication"/>
        public static bool VerifyAuthentication(this PamaxieApplication value)
        {
            return ApplicationService.VerifyAuthentication(value as PamaxieApplication);
        }
    }
}