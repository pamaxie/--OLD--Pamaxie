using System;
using System.Net.Http;
using Pamaxie.Database.Design;

namespace Pamaxie.Database.Extensions.Client
{
    /// <inheritdoc/>
    public class DatabaseService : IDatabaseService<HttpClient>
    {
        /// <inheritdoc/>
        public HttpClient Service { get; internal init; } = new();
        
        /// <inheritdoc/>
        public IPamaxieDataContext DataContext { get; }
        
        /// <inheritdoc/>
        public bool ConnectionSuccess { get; set; }
        
        /// <inheritdoc/>
        public DateTime LastConnectionSuccess { get; set; }
        
        /// <summary>
        /// Contains the service responsible for interacting with user data for the Api
        /// </summary>
        internal static  IUserDataService UserService { get; set; }

        /// <summary>
        /// Contains the Service responsible for interacting with application data for the Api
        /// </summary>
        internal static IApplicationDataService ApplicationService { get; set; }

        public DatabaseService(PamaxieDataContext dataContext)
        {
            DataContext = dataContext;
            UserService = new UserDataService(dataContext, this);
            ApplicationService = new ApplicationDataService(dataContext, this);
            Service.DefaultRequestHeaders.Authorization = dataContext.GetAuthenticationRequestHeader();
        }

        /// <inheritdoc/>
        public bool Connect()
        {
            throw new NotImplementedException();
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