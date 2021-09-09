using System.Collections.Generic;

namespace Pamaxie.Data
{
    /// <summary>
    /// This designs the basic implementation of a user to be stored inside the Database
    /// </summary>
    public interface IPamaxieUser : IDatabaseObject
    {
        /// <summary>
        /// The user name of the user
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// The first name of the user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// The last name of the user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// The Email-Address the user used to register with our services
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Was the email of the user verified
        /// </summary>
        public bool EmailVerified { get; set; }

        /// <summary>
        /// The url / address where the profile picture of the user resides
        /// </summary>
        public string ProfilePictureAddress { get; set; }

        /// <summary>
        /// A list of all application keys the user owns
        /// </summary>
        public IEnumerable<string> ApplicationKeys { get; set; }

        /// <summary>
        /// If the user was blocked by our system (if <see cref="Deleted"/> is true too this means
        /// that the user has been deleted and <see cref="IDatabaseObject.TTL"/> ran out and the data of the user has been cleared from our database.
        /// This means that next time the user is at all looked up through our system the user is cleared
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// If the user was deleted by request
        /// </summary>
        public bool Deleted { get; set; }
    }
}