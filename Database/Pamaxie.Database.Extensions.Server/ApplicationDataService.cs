﻿using System;
using System.Data;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Server
{
    /// Implementation to get <see cref="PamaxieApplication"/> data from the server
    public sealed class ApplicationDataService : ServerDataServiceBase<PamaxieApplication>, IApplicationDataService
    {
        /// <inheritdoc/>
        internal ApplicationDataService(IPamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
        }

        /// <inheritdoc/>
        public PamaxieUser GetOwner(PamaxieApplication value)
        {
            if (Service.Service == null)
                throw new DataException(
                    "Please ensure that the Service is connected and initialized before attempting to poll data from it");

            IDatabase db = Service.Service.GetDatabase();
            RedisValue rawData = db.StringGet(value.OwnerKey);
            return string.IsNullOrEmpty(rawData) ? default : JsonConvert.DeserializeObject<PamaxieUser>(rawData);
        }

        /// <inheritdoc/>
        public PamaxieApplication EnableOrDisable(PamaxieApplication value)
        {
            if (Service.Service == null)
                throw new DataException(
                    "Please ensure that the Service is connected and initialized before attempting to poll or push data from/to it");

            IDatabase db = Service.Service.GetDatabase();
            if (!db.KeyExists(value.Key))
                throw new ArgumentException("The key u entered does not exist in our database yet");

            value.Disabled = !value.Disabled;

            string data = JsonConvert.SerializeObject(value);
            db.StringSet(value.Key, data);
            return value;
        }

        /// <inheritdoc/>
        public bool VerifyAuthentication(PamaxieApplication value)
        {
            throw new NotImplementedException();
        }
    }
}