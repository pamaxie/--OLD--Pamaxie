using System;
using System.Data;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client.Base;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Client
{
    /// Implementation to get <see cref="IPamaxieApplication"/> data from the server
    public class ApplicationDataService : ClientDataServiceBase<IPamaxieApplication>
    {
        /// <inheritdoc/>
        internal ApplicationDataService(PamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
            ApplicationDataServiceExtension.ApplicationService = this;
        }

        /// <summary>
        /// Enables or Disables the application
        /// </summary>
        /// <param name="value">The application that will be enabled or disabled</param>
        /// <returns>The updated value of the database</returns>
        public IPamaxieApplication EnableOrDisable(IPamaxieApplication value)
        {
            if (Service.Service == null)
                throw new DataException("Please ensure that the Service is connected and initialized before attempting to poll or push data from/to it");

            IDatabase db = Service.Service.GetDatabase();
            if (!db.KeyExists(value.Key))
                throw new ArgumentException("The key u entered does not exist in our database yet");

            value.Disabled = !value.Disabled;
            
            string data = JsonConvert.SerializeObject(value);
            db.StringSet(value.Key, data);
            return value;
        }
        
        /// <summary>
        /// Verify the Authentication of the <see cref="AppAuthCredentials"/>
        /// </summary>
        /// <param name="value">The <see cref="AppAuthCredentials"/> from the <see cref="IPamaxieApplication"/></param>
        /// <returns><see cref="bool"/> if the authentication was verified</returns>
        public bool VerifyAuthentication(IPamaxieApplication value)
        {
            throw new NotImplementedException();
        }
    }
}