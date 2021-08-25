using System;
using System.Net.Http;
using Pamaxie.Database.Design;

namespace Pamaxie.Database.Extensions.Client
{
    /// <inheritdoc/>
    public class DatabaseService : IDatabaseService<HttpClient>
    {
        /// <inheritdoc/>
        public HttpClient Service { get; }
        
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
            throw new NotImplementedException();
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