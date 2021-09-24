using System;
using System.Net.Http;
using Pamaxie.Database.Design;

namespace Pamaxie.Database.Extensions.Client
{
    /// <inheritdoc/>
    public sealed class DatabaseService : IDatabaseService<HttpClient, PamaxieDataContext>
    {
        /// <inheritdoc/>
        public HttpClient Service { get; internal init; } = new HttpClient();

        /// <inheritdoc/>
        public PamaxieDataContext DataContext { get; }

        /// <inheritdoc/>
        public bool ConnectionSuccess { get; set; }

        /// <inheritdoc/>
        public DateTime LastConnectionSuccess { get; set; }

        /// <summary>
        /// Contains the service responsible for interacting with user data for the Api
        /// </summary>
        internal static IUserDataService UserService { get; set; }

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
            //TODO: This technically needs network credentials to connect for the first time. After that use a timer to automatically refresh the token in UserService
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