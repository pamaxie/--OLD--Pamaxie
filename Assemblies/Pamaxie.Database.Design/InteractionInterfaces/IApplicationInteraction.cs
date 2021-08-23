using System.Collections.Generic;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.InteractionObjects
{
    /// <summary>
    /// Defines the behavior of <see cref="IApplicationInteraction"/>
    /// </summary>
    public interface IApplicationInteraction : IDatabaseInteraction<IPamaxieApplication>
    {
        /// <summary>
        /// Gets a list of <see cref="IPamaxieApplication"/> from a user key
        /// </summary>
        /// <param name="value">The key of the user who owns the applications</param>
        /// <returns>A list of all applications the user owns</returns>
        public IEnumerable<IPamaxieApplication> GetFromUser(string value);

        /// <summary>
        /// Enables or Disables the application
        /// </summary>
        /// <param name="value">The application that will be enabled or disabled</param>
        /// <returns><see cref="bool"/> if the operation was successful and the value was changed</returns>
        public bool EnableOrDisable(IPamaxieApplication value);
    }
}