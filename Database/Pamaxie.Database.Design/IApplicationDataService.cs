using Pamaxie.Data;

namespace Pamaxie.Database.Design
{
    /// <summary>
    /// Interface that defines Application interactions
    /// </summary>
    public interface IApplicationDataService : IDataServiceBase<PamaxieApplication>
    {
        /// <summary>
        /// Get the owner of the application
        /// </summary>
        /// <param name="value">The application to get the owner from</param>
        /// <returns>The application's owner</returns>
        public PamaxieUser GetOwner(PamaxieApplication value);

        /// <summary>
        /// Enables or Disables the application
        /// </summary>
        /// <param name="value">The application that will be enabled or disabled</param>
        /// <returns>The updated value of the database</returns>
        public PamaxieApplication EnableOrDisable(PamaxieApplication value);

        /// <summary>
        /// Verify the Authentication of the <see cref="AppAuthCredentials"/>
        /// </summary>
        /// <param name="value">The <see cref="AppAuthCredentials"/> from the <see cref="PamaxieApplication"/></param>
        /// <returns><see cref="bool"/> if the authentication was verified</returns>
        public bool VerifyAuthentication(PamaxieApplication value);
    }
}