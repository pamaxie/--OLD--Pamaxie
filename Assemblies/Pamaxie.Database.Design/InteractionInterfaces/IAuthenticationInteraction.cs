using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.InteractionObjects
{
    /// <summary>
    /// Defines the behavior of <see cref="IAuthenticationInteraction"/>
    /// </summary>
    public interface IAuthenticationInteraction : IDatabaseInteraction<AppAuthCredentials>
    {
        /// <summary>
        /// Verify the Authentication of the <see cref="AppAuthCredentials"/>
        /// </summary>
        /// <param name="value">The <see cref="AppAuthCredentials"/> from the <see cref="IPamaxieApplication"/></param>
        /// <returns><see cref="bool"/> if the authentication was verified</returns>
        public bool VerifyAuthentication(AppAuthCredentials value);
    }
}