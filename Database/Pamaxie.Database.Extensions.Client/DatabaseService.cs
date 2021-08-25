using System;
using Pamaxie.Database.Extensions.DatabaseExtensions;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Client
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
        internal static  UserDataService UserService { get; private set; }

        /// <summary>
        /// Contains the Service responsible for interacting with application data in the redis database
        /// </summary>
        internal static ApplicationDataService ApplicationService { get; private set; }

        public DatabaseService(PamaxieDataContext dataContext)
        {
            DataContext = dataContext;
            UserService = new UserDataService(dataContext, this);
            ApplicationService = new ApplicationDataService(dataContext, this);
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