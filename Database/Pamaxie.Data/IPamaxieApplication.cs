using System;

namespace Pamaxie.Data
{
    public interface IPamaxieApplication : IDatabaseObject
    {
        /// <summary>
        /// The credentials used by application to authenticate with the api
        /// </summary>
        public AppAuthCredentials Credentials { get; set; }

        /// <summary>
        /// The Key of the owner of the application
        /// </summary>
        public string OwnerKey { get; set; }

        /// <summary>
        /// The Name of the application
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// The date time since last accessed
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