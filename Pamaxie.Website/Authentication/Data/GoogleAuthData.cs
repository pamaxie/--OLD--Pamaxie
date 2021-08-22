using System;
using Pamaxie.Data;

#pragma warning disable 8618

namespace Pamaxie.Website.Authentication.Data
{
    public class GoogleAuthData : IPamaxieUser
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
        public string ProfilePictureAddress { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool EmailVerified { get; set; }
    }
}
