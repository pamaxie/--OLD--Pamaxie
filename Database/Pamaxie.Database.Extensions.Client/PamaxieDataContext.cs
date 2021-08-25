using System;
using Pamaxie.Database.Extensions.DatabaseExtensions;

namespace Pamaxie.Database.Extensions.Client
{
    public class PamaxieDataContext : IPamaxieDataContext
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
        /// <param name="instanceName">TODO</param>
        /// <param name="password">TODO</param>
        /// <param name="reconnectionAttempts">TODO</param>
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
        /// <param name="instanceName">TODO</param>
        [Obsolete(
            "This should not be used, since this means u don't have a password on your database, which exposes your database to attacks. Please always make sure to use a password on your database.")]
        public PamaxieDataContext(string instanceName) : this(instanceName, string.Empty) { }

        /// <inheritdoc/>
        public string ConnectionString()
        {
            throw new NotImplementedException();
        }
    }
}