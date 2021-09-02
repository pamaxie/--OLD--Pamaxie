using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using Pamaxie.Database.Extensions.Server.Base;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Server
{
    /// Implementation to get <see cref="IPamaxieUser"/> data from the server
    public class UserDataService : ServerDataServiceBase<IPamaxieUser>, IUserDataService
    {
        /// <inheritdoc/>
        internal UserDataService(IPamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
        }
        
        /// <inheritdoc/>
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

        /// <inheritdoc/>
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