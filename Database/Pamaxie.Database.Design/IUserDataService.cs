using System.Collections.Generic;
using Pamaxie.Data;

namespace Pamaxie.Database.Design
{
    /// <summary>
    /// Interface that defines User interactions
    /// </summary>
    public interface IUserDataService : IDataServiceBase<PamaxieUser>
    {
        /// <summary>
        /// Gets a list of <see cref="PamaxieApplication"/> from a user
        /// </summary>
        /// <param name="value">The key of the user who owns the applications</param>
        /// <returns>A list of all applications the user owns</returns>
        public IEnumerable<PamaxieApplication> GetAllApplications(PamaxieUser value);

        /// <summary>
        /// Verifies the email of the user
        /// </summary>
        /// <param name="value">The user that will have their email verified</param>
        /// <returns><see cref="bool"/> if the operation was successful and the email was verified</returns>
        public bool VerifyEmail(PamaxieUser value);
    }
}