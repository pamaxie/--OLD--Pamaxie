using Pamaxie.Data;

namespace Pamaxie.Database.Design
{
    /// <summary>
    /// Interface that defines Application interactions
    /// </summary>
    public interface IApplicationDataService : IDataServiceBase<PamaxieApplication>
    {
        /// <summary>
        /// Get the owner of the <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="value">The <see cref="PamaxieApplication"/> to get the owner from</param>
        /// <returns>The <see cref="PamaxieApplication"/>'s owner</returns>
        public PamaxieUser GetOwner(PamaxieApplication value);

        /// <summary>
        /// Enables or Disables the <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="value">The <see cref="PamaxieApplication"/> that will be enabled or disabled</param>
        /// <returns>The enabled or disabled <see cref="PamaxieApplication"/> from the database</returns>
        public PamaxieApplication EnableOrDisable(PamaxieApplication value);

        /// <summary>
        /// Verify the Authentication of the <see cref="AppAuthCredentials"/> in the <see cref="PamaxieApplication"/>
        /// </summary>
        /// <param name="value">The <see cref="AppAuthCredentials"/> from the <see cref="PamaxieApplication"/></param>
        /// <returns><see cref="bool"/> if the authentication was verified</returns>
        public bool VerifyAuthentication(PamaxieApplication value);
    }
}