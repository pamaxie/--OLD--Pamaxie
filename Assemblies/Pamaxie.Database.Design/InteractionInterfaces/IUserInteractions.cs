using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.InteractionObjects
{
    /// <summary>
    /// Defines the behavior of <see cref="IUserInteraction"/>
    /// </summary>
    public interface IUserInteraction : IDatabaseInteraction<IPamaxieUser>
    {
        /// <summary>
        /// Verifies the email of the user
        /// </summary>
        /// <param name="value">The user that will have their email verified</param>
        /// <returns><see cref="bool"/> if the operation was successful and the email was verified</returns>
        public bool VerifyEmail(IPamaxieUser value);
    }
}