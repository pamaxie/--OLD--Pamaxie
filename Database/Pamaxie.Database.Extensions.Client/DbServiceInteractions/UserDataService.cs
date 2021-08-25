using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client.Base;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Client
{
    /// Implementation to get <see cref="IPamaxieUser"/> data from the server
    public class UserDataService : ClientDataServiceBase<IPamaxieUser>
    {
        /// <inheritdoc/>
        internal UserDataService(PamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
            UserDataServiceExtension.UserService = this;
        }
        
        /// <summary>
        /// Gets a list of <see cref="IPamaxieApplication"/> from a user
        /// </summary>
        /// <param name="value">The key of the user who owns the applications</param>
        /// <returns>A list of all applications the user owns</returns>
        public IEnumerable<IPamaxieApplication> GetAllApplications(IPamaxieUser value)
        {
            if (Service.Service == null)
                throw new DataException(
                    "Please ensure that the Service is connected and initialized before attempting to poll data from it");

            IDatabase db = Service.Service.GetDatabase();
            if (!db.KeyExists(value.Key))
                throw new ArgumentException("The key u entered does not exist in our database yet");

            IEnumerable<string> applicationKeys = value.ApplicationKeys;
            List<IPamaxieApplication> applications = (from key in applicationKeys
                select db.StringGet(key)
                into rawData
                where !string.IsNullOrEmpty(rawData)
                select JsonConvert.DeserializeObject<IPamaxieApplication>(rawData)
                into application
                where application != null
                where application.OwnerKey != value.Key
                select application).ToList();
            return applications.AsEnumerable();
        }

        /// <summary>
        /// Verifies the email of the user
        /// </summary>
        /// <param name="value">The user that will have their email verified</param>
        /// <returns><see cref="bool"/> if the operation was successful and the email was verified</returns>
        public bool VerifyEmail(IPamaxieUser value)
        {
            if (Service.Service == null)
                return false;

            IDatabase db = Service.Service.GetDatabase();
            if (!db.KeyExists(value.Key))
                return false;

            value.EmailVerified = true;
            
            string data = JsonConvert.SerializeObject(value);
            db.StringSet(value.Key, data);
            return true;
        }
    }
}