using System;
using System.Data;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Server.Base
{
    public class ServerDataServiceBase<T> : IDataServiceBase<T> where T : IDatabaseObject
    {
        /// <summary>
        /// Data Context responsible for connecting to Pamaxie
        /// </summary>
        internal PamaxieDataContext DataContext { get; set; }

        /// <summary>
        /// The Service that should be used to connect to the database
        /// </summary>
        internal DatabaseService Service { get; init; }

        /// <inheritdoc/>
        public T Get(string key)
        {
            if (Service.Service == null)
                throw new DataException("Please ensure that the Service is connected and initialized before attempting to poll data from it");

            IDatabase db = Service.Service.GetDatabase();
            RedisValue rawData = db.StringGet(key);
            return string.IsNullOrEmpty(rawData) ? default : JsonConvert.DeserializeObject<T>(rawData);
        }

        /// <inheritdoc/>
        public T Create(T value)
        {
            if (Service.Service == null)
                throw new DataException("Please ensure that the Service is connected and initialized before attempting to poll or push data from/to it");

            IDatabase db = Service.Service.GetDatabase();
            if (db.KeyExists(value.Key))
                throw new ArgumentException("The key u tried to create already exists in our database");

            string data = JsonConvert.SerializeObject(value);
            db.StringSet(value.Key, data);
            return value;
        }

        /// <inheritdoc/>
        public bool TryCreate(T value, out T createdValue)
        {
            createdValue = default;
            if (Service.Service == null)
                throw new DataException("Please ensure that the Service is connected and initialized before attempting to poll or push data from/to it");

            IDatabase db = Service.Service.GetDatabase();
            if (db.KeyExists(value.Key))
                return false;

            string data = JsonConvert.SerializeObject(value);
            db.StringSet(value.Key, data);
            createdValue = value;
            return true;
        }

        /// <inheritdoc/>
        public T Update(T value)
        {
            if (Service.Service == null)
                throw new DataException("Please ensure that the Service is connected and initialized before attempting to poll or push data from/to it");

            IDatabase db = Service.Service.GetDatabase();
            if (!db.KeyExists(value.Key))
                throw new ArgumentException("The key u entered does not exist in our database yet");

            string data = JsonConvert.SerializeObject(value);
            db.StringSet(value.Key, data);
            return value;
        }

        /// <inheritdoc/>
        public bool TryUpdate(T value, out T updatedValue)
        {
            updatedValue = default;
            if (Service.Service == null)
                throw new DataException("Please ensure that the Service is connected and initialized before attempting to poll or push data from/to it");

            IDatabase db = Service.Service.GetDatabase();
            if (!db.KeyExists(value.Key))
                return false;

            string data = JsonConvert.SerializeObject(value);
            db.StringSet(value.Key, data);
            updatedValue = value;
            return false;
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(T value, out T databaseValue)
        {
            databaseValue = default;
            if (Service.Service == null)
                throw new DataException("Please ensure that the Service is connected and initialized before attempting to poll or push data from/to it");

            IDatabase db = Service.Service.GetDatabase();
            string data = JsonConvert.SerializeObject(value);
            bool existed = db.KeyExists(value.Key);
            db.StringSet(value.Key, data);
            return existed;
        }

        /// <inheritdoc/>
        public bool Delete(T value)
        {
            if (Service.Service == null)
                throw new DataException("Please ensure that the Service is connected and initialized before attempting to poll or push data from/to it");

            IDatabase db = Service.Service.GetDatabase();

            if (!db.KeyExists(value.Key))
                throw new ArgumentException("The key u entered does not exist in our database yet");

            return db.KeyDelete(value.Key);
        }
    }
}
