// ReSharper disable InconsistentNaming

using System;

namespace Pamaxie.Data
{
    /// <summary>
    /// Defines how a <see cref="IDatabaseObject"/> should be structured (Main item that is used to store values in redis)
    /// </summary>
    public interface IDatabaseObject
    {
        /// <summary>
        /// The key to query for the object in the database
        /// </summary>
        public string UniqueKey { get; set; }

        /// <summary>
        /// Defines the Time To Live for the data object
        /// </summary>
        public DateTime TTL { get; set; }
    }
}