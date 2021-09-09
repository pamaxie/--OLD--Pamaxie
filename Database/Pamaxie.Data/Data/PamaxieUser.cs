using System;
using System.Collections.Generic;

namespace Pamaxie.Data
{
    /// <summary>
    /// Class containing information for website users
    /// </summary>
    public class PamaxieUser : IPamaxieUser
    {
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime TTL { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string ProfilePictureAddress { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IEnumerable<string> ApplicationKeys { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Deleted { get; set; }
    }
}