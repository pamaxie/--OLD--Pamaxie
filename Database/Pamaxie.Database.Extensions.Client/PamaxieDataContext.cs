using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Pamaxie.Database.Design;
using Pamaxie.Jwt;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// DataContext class used for the connection information to the database api and the Redis database
    /// </summary>
    public sealed class PamaxieDataContext
    {
        /// <summary>
        /// Defines the available instances for data connection
        /// </summary>
        public string DataInstances { get; }

        /// <summary>
        /// Defines the password to use for the instances
        /// </summary>
        public AuthToken Token { get; }

        /// <summary>
        /// How many attempts should be made to connect to the service
        /// </summary>
        public int ReconnectionAttempts { get; }

        /// <summary>
        /// Connection properties to a database with a password and a custom amount of reconnection attempts
        /// </summary>
        /// <param name="instanceName">The instance of the database</param>
        /// <param name="token">Token to use for the database authentication</param>
        /// <param name="reconnectionAttempts">How many attempts should be made to connect to the database</param>
        // ReSharper disable once MemberCanBePrivate.Global
        public PamaxieDataContext(string instanceName, AuthToken token, int reconnectionAttempts = 3)
        {
            DataInstances = instanceName;
            Token = token;
            ReconnectionAttempts = reconnectionAttempts;
        }

        /// <summary>
        /// Gets the <see cref="AuthenticationHeaderValue"/> used for the HttpClient.DefaultRequestHeaders.Authorization
        /// </summary>
        /// <returns>A <see cref="AuthenticationHeaderValue"/> with a Bearer scheme authentication</returns>
        public AuthenticationHeaderValue GetAuthenticationRequestHeader()
        {
            if (Token == null)
                throw new InvalidOperationException("Please make sure that the token are initialized before calling this method");

            return new AuthenticationHeaderValue("Bearer", Token.Token);
        }
    }
}