using System;
using Pamaxie.Database.Extensions.DatabaseExtensions;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Server
{
    public class DatabaseService : IDatabaseService<ConnectionMultiplexer>
    {
        /// <inheritdoc/>
        public ConnectionMultiplexer Service { get; private set;  }
        
        /// <inheritdoc/>
        public IPamaxieDataContext DataContext { get; }
        
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

        public DatabaseService(PamaxieDataContext dataContext)
        {
            DataContext = dataContext;
            Users = new UserDataService(dataContext, this);
            Applications = new ApplicationDataService(dataContext, this);
        }
        
        /// <inheritdoc/>
        public bool Connect()
        {
            Service = ConnectionMultiplexer.Connect((DataContext.ConnectionString()) ?? string.Empty);
            if (Service == null) return false;
            ConnectionSuccess = true;
            LastConnectionSuccess = DateTime.Now;
            return true;

        }

        /// <inheritdoc/>
        public bool IsDatabaseAvailable()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ushort DatabaseLatency()
        {
            throw new NotImplementedException();
        }
    }
}