using System;
using System.Net;
using System.Net.Http.Headers;
<<<<<<< HEAD
using System.Text;
using Pamaxie.Database.Design;
=======
>>>>>>> database-rework
using Pamaxie.Jwt;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// DataContext class used for the connection information to the database api and the Redis database
    /// </summary>
    public sealed class PamaxieDataContext
    {
        /// <summary>
<<<<<<< HEAD
        /// Defines the available instances for data connection
        /// </summary>
        public string ApiUrl { get; }

        /// <summary>
        /// Defines the password to use for the instances
        /// </summary>
        public AuthToken Token { get; private set; }

        /// <summary>
        /// How many attempts should be made to connect to the service
        /// </summary>
        public int ReconnectionAttempts { get; }

=======
        /// Defines the url of the api that will be 
        /// </summary>
        public string DataInstances { get; }

>>>>>>> database-rework
        /// <summary>
        /// Defines the password to use for the instances
        /// </summary>
<<<<<<< HEAD
        /// <param name="instanceName">The instance of the database</param>
        /// <param name="token">Token to use for the database authentication</param>
        /// <param name="reconnectionAttempts">How many attempts should be made to connect to the database</param>
        // ReSharper disable once MemberCanBePrivate.Global
        public PamaxieDataContext(string instanceName, AuthToken token, int reconnectionAttempts = 3)
        {
            ApiUrl = instanceName;
            Token = token;
            ReconnectionAttempts = reconnectionAttempts;
        }

        /// <summary>
=======
        public AuthToken Token { get; }

        /// <summary>
        /// Connection properties to a api
        /// </summary>
        /// <param name="instance">The url of the api</param>
        /// <param name="token">Token to use for the api authentication</param>
        public PamaxieDataContext(string instance, AuthToken token)
        {
            DataInstances = instance;
            Token = token;
        }

        /// <summary>
>>>>>>> database-rework
        /// Gets the <see cref="AuthenticationHeaderValue"/> used for the HttpClient.DefaultRequestHeaders.Authorization
        /// </summary>
        /// <returns>A <see cref="AuthenticationHeaderValue"/> with a Bearer scheme authentication</returns>
        public AuthenticationHeaderValue GetAuthenticationRequestHeader()
        {
            if (Token == null)
                throw new InvalidOperationException("Please make sure that the token are initialized before calling this method");

            return new AuthenticationHeaderValue("Bearer", Token.Token);
        }

        public void Disconnect()
        {
            Token = null;
        }
    }
}