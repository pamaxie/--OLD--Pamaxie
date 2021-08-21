using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pamaxie.Data
{
    /// <summary>
    ///     Data for Application specific things
    /// </summary>
    public class Application : IDatabaseObject
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
        /// The App Token that is reached in to us for auth
        /// </summary>
        public string AppToken { get; set; }

        /// <summary>
        /// The User who owns the application
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// The Hashed application Token
        /// </summary>
        public string AppTokenHash { get; set; }
        
        /// <summary>
        /// The Name of the application
        /// </summary>
        public string ApplicationName { get; set; }
        
        /// <summary>
        /// The last time the application was Authorized
        /// </summary>
        public DateTime LastAuth { get; set; }
        
        /// <summary>
        /// Did the application get rate limited
        /// </summary>
        public bool RateLimited { get; set; }
        
        /// <summary>
        /// Did the application get disabled by the user
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Did the application get Deleted from our Database
        /// </summary>
        public bool Deleted { get; set; }
    }
}