using System;
using System.Net.Http.Headers;
using Pamaxie.Database.Design;

namespace Pamaxie.Database.Extensions.Client
{
    /// <inheritdoc/>
    public sealed class PamaxieDataContext : IPamaxieDataContext
    {
        /// <inheritdoc/>
        public string DataInstances { get; }

        /// <inheritdoc/>
        public string Password { get; }

        /// <inheritdoc/>
        public int ReconnectionAttempts { get; }

        /// <summary>
        /// Connection properties to a database with a password and a custom amount of reconnection attempts
        /// </summary>
        /// <param name="instanceName">The instance of the database</param>
        /// <param name="password">Password to use for the database</param>
        /// <param name="reconnectionAttempts">How many attempts should be made to connect to the database</param>
        // ReSharper disable once MemberCanBePrivate.Global
        public PamaxieDataContext(string instanceName, string password, int reconnectionAttempts = 3)
        {
            DataInstances = instanceName;
            Password = password;
            ReconnectionAttempts = reconnectionAttempts;
        }

        /// <summary>
        /// Connection properties to a database without a password !!THIS IS NOT RECOMMENDED!!
        /// </summary>
        /// <param name="instanceName">The instance of the database</param>
        [Obsolete(
            "This should not be used, since this means u don't have a password on your database api, which exposes your database to attacks. Please always make sure to use a password on your database.")]
        public PamaxieDataContext(string instanceName) : this(instanceName, string.Empty)
        {
        }

        /// <inheritdoc/>
        public string ConnectionString()
        {
            throw new NotSupportedException("Cannot use connection strings for connecting to the client as of now.");
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <returns>TODO</returns>
        public AuthenticationHeaderValue GetAuthenticationRequestHeader()
        {
            return new AuthenticationHeaderValue("Basic", Password);
        }
    }
}