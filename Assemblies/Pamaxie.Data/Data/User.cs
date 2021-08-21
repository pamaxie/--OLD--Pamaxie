using System;
using System.ComponentModel.DataAnnotations;

namespace Pamaxie.Data
{
    /// <summary>
    /// Class containing information for website users
    /// </summary>
    public class User : IDatabaseObject
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
        /// TODO
        /// </summary>
        public string GoogleUserId { get; set; }
        
        /// <summary>
        /// TODO
        /// </summary>
        public string Username { get; set; }
        
        /// <summary>
        /// TODO
        /// </summary>
        [EmailAddress] public string Email { get; set; }
        
        /// <summary>
        /// TODO
        /// </summary>
        public bool EmailVerified { get; set; } = false;
        
        /// <summary>
        /// TODO
        /// </summary>
        public bool DeletedAccount { get; set; }
    }
}