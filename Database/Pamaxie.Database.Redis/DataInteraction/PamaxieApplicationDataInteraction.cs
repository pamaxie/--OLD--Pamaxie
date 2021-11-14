using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Database.Server.DataInteraction
{
    /// <inheritdoc/>
    internal class PamaxieApplicationDataInteraction : PamaxieDataInteractionBase<IPamaxieApplication>, IPamaxieApplicationDataInteraction
    {
        private PamaxieDatabaseDriver _owner;
        /// <summary>
        /// Passes through the database driver required for interaction with the database
        /// </summary>
        /// <param name="owner"></param>
        public PamaxieApplicationDataInteraction(PamaxieDatabaseDriver owner) : base(owner)
        {
            _owner = owner;
        }

        /// <inheritdoc/>
        public IPamaxieApplication EnableOrDisable(IPamaxieApplication value)
        {
            if (_owner == null)
            {
                throw new NullReferenceException($"The required property {nameof(_owner)} was null. Please ensure it is set before calling this class. This should usually never happen.");
            }

            if (_owner.Configuration == null || string.IsNullOrWhiteSpace(_owner.Configuration.ToString()))
            {
                throw new InvalidOperationException("This method cannot be called before the configuration has been initialized and properly configured. Please configure the database settings first.");
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(value.UniqueKey))
            {
                throw new ArgumentNullException(nameof(value.UniqueKey));
            }

            using var conn = ConnectionMultiplexer.Connect(_owner.Configuration.ToString());
            IDatabase db = conn.GetDatabase();

            if (!db.KeyExists(value.UniqueKey))
            {
                throw new ArgumentException("A data entry with the specified value could not be found.");
            }

            value.Disabled = !value.Disabled;

            string data = JsonConvert.SerializeObject(value);
            db.StringSet(value.UniqueKey, data);
            return value;
        }

        /// <inheritdoc/>
        public IPamaxieUser GetOwner(IPamaxieApplication value)
        {
            if (_owner == null)
            {
                throw new NullReferenceException($"The required property {nameof(_owner)} was null. Please ensure it is set before calling this class. This should usually never happen.");
            }

            if (_owner.Configuration == null || string.IsNullOrWhiteSpace(_owner.Configuration.ToString()))
            {
                throw new InvalidOperationException("This method cannot be called before the configuration has been initialized and properly configured. Please configure the database settings first.");
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (string.IsNullOrWhiteSpace(value.UniqueKey))
            {
                throw new ArgumentNullException(nameof(value.UniqueKey));
            }

            using var conn = ConnectionMultiplexer.Connect(_owner.Configuration.ToString());
            IDatabase db = conn.GetDatabase();

            if (!db.KeyExists(value.UniqueKey))
            {
                throw new ArgumentException("A data entry with the specified value could not be found.");
            }

            RedisValue rawData = db.StringGet(value.OwnerKey);
            return string.IsNullOrEmpty(rawData) ? default : JsonConvert.DeserializeObject<PamaxieUser>(rawData);
        }

        /// <inheritdoc/>
        public bool VerifyAuthentication(IPamaxieApplication value)
        {
            throw new NotSupportedException("This method has not been implemented yet, and therefore can't be supported on this API yet.");
        }
    }
}
