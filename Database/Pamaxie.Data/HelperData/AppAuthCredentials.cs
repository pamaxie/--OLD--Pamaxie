using System;

namespace Pamaxie.Data
{
    /// <summary>
    /// AppAuthCredentials data used to authenticate with the API
    /// </summary>
    public class AppAuthCredentials
    {
        /// <summary>
        /// The Authorization Token that is reached in to us for auth
        /// </summary>z
        public string AuthorizationToken { get; set; }

        /// <summary>
        /// The Ciphered Authorization Token, that is stored in the database (Blow-fish 2 Cipher)
        /// </summary>
        public string AuthorizationTokenCipher { get; set; }

        /// <summary>
        /// The last time the application was Authorized
        /// </summary>
        public DateTime LastAuth { get; set; }
    }
}