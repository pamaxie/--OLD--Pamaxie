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
        /// Gets a list of <see cref="PamaxieApplication"/> from a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="value">The key of the <see cref="PamaxieUser"/> who owns the applications</param>
        /// <returns>A list of all applications the <see cref="PamaxieUser"/> owns</returns>
        public IEnumerable<PamaxieApplication> GetAllApplications(PamaxieUser value);

        /// <summary>
        /// Verifies the email of the <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="value">The <see cref="PamaxieUser"/> that will have their email verified</param>
        /// <returns><see cref="bool"/> if the operation was successful and the email was verified</returns>
        public bool VerifyEmail(PamaxieUser value);
    }
}