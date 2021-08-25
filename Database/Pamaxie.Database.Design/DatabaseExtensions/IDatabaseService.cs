using System;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.InteractionObjects.BaseInterfaces;

namespace Pamaxie.Database.Extensions.DatabaseExtensions
{
    /// <summary>
    /// Service responsible for handling interaction with the database or database api. This automatically detects
    /// the connection context.
    /// </summary>
    public interface IDatabaseService<T>
    {
        #region Services
        /// <summary>
            /// The actual Database Service that we use to connect to the database
            /// </summary>
            public T Service { get; }

        #endregion

        /// <summary>
        /// The data context that should be used for connecting to instances
        /// </summary>
        public IPamaxieDataContext DataContext { get; }

        /// <summary>
        /// Is set if <see cref="Connect"/> ran successfully, please see <see cref="LastConnectionSuccess"/> when the last connection was successful
        /// </summary>
        public bool ConnectionSuccess { get; set; }

        /// <summary>
        /// Defines when the last connection was made successfully
        /// </summary>
        public DateTime LastConnectionSuccess { get; set; }

        /// <summary>
        /// Attempts a connection with the redis instance
        /// </summary>
        /// <returns><see cref="bool"/> if the connection attempt was successful</returns>
        public bool Connect();
        
        /// <summary>
        /// Validates if the database is available
        /// </summary>
        /// <returns><see cref="bool"/> if the database is available</returns>
        public bool IsDatabaseAvailable();
        
        /// <summary>
        /// Validates the roundtrip latency to the database
        /// </summary>
        /// <returns><inheritdoc cref="ushort"/> Denoting the latency to the database</returns>
        public ushort DatabaseLatency();
    }
}