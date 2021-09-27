using System;
using Pamaxie.Database.Design;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Server
{
    /// <inheritdoc/>
    public sealed class DatabaseService : IDatabaseService<IConnectionMultiplexer, ConfigurationOptions>
    {
        /// <inheritdoc/>
        public IConnectionMultiplexer Service { get; internal set; }

        /// <inheritdoc/>
        public ConfigurationOptions DataContext { get; }

        /// <inheritdoc/>
        public bool ConnectionSuccess { get; set; }

        /// <inheritdoc/>
        public DateTime LastConnectionSuccess { get; set; }

        /// <summary>
        /// Contains the service responsible for interacting with user data in the redis database
        /// </summary>
        public UserDataService Users { get; }

        /// <summary>
        /// Contains the Service responsible for interacting with application data in the redis database
        /// </summary>
        public ApplicationDataService Applications { get; }

        public DatabaseService(ConfigurationOptions dataContext)
        {
            DataContext = dataContext;
            Users = new UserDataService(this);
            Applications = new ApplicationDataService(this);
        }

        /// <inheritdoc/>
        public bool Connect()
        {
            Service = ConnectionMultiplexer.Connect(DataContext);

            if (Service == null)
            {
                return false;
            }

            ConnectionSuccess = true;
            LastConnectionSuccess = DateTime.Now;
            return true;
        }

        /// <inheritdoc/>
        public bool IsServiceAvailable()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ushort ServiceLatency()
        {
            throw new NotImplementedException();
        }
    }
}