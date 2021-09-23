using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Server
{
    /// Implementation to get <see cref="PamaxieUser"/> data from the server
    public sealed class UserDataService : ServerDataServiceBase<PamaxieUser>, IUserDataService
    {
        /// <inheritdoc/>
        internal UserDataService(DatabaseService service)
        {
            Service = service;
        }

        /// <inheritdoc/>
        public IEnumerable<PamaxieApplication> GetAllApplications(PamaxieUser value)
        {
            if (Service.Service == null)
            {
                throw new DataException(
                    "Please ensure that the Service is connected and initialized before attempting to poll data from it");
            }

            IDatabase db = Service.Service.GetDatabase();

            if (!db.KeyExists(value.Key))
            {
                throw new ArgumentException("The key u entered does not exist in our database yet");
            }

            IEnumerable<string> applicationKeys = value.ApplicationKeys;
            List<PamaxieApplication> applications = new List<PamaxieApplication>();

            foreach (string applicationKey in applicationKeys)
            {
                RedisValue rawData = db.StringGet(applicationKey);

                if (!string.IsNullOrEmpty(rawData))
                {
                    PamaxieApplication application = JsonConvert.DeserializeObject<PamaxieApplication>(rawData);

                    if (application != null)
                    {
                        if (application.OwnerKey == value.Key)
                        {
                            applications.Add(application);
                        }
                    }
                }
            }

            return applications.AsEnumerable();
        }

        /// <inheritdoc/>
        public bool VerifyEmail(PamaxieUser value)
        {
            if (Service.Service == null)
            {
                return false;
            }

            IDatabase db = Service.Service.GetDatabase();

            if (!db.KeyExists(value.Key))
            {
                return false;
            }

            value.EmailVerified = true;

            string data = JsonConvert.SerializeObject(value);
            db.StringSet(value.Key, data);
            return true;
        }
    }
}