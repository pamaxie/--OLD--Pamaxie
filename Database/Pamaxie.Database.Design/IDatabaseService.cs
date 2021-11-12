using System;

namespace Pamaxie.Database.Design
{
    /// <summary>
    /// Service responsible for handling interaction with the database or database api. This automatically detects
    /// the connection context.
    /// </summary>
    public interface IDatabaseService<out T, out T2>
    {

        /// <summary>
        /// The actual Database Service that we use to connect to the database or database api
        /// </summary>
        public T Service { get; }

        /// <summary>
        /// The data context that should be used for connecting to instances
        /// </summary>
        public T2 DataContext { get; }

        /// <summary>
        /// Is set if <see cref="Connect"/> ran successfully, please see <see cref="LastConnectionSuccess"/> when the last connection was successful
        /// </summary>
        public bool IsConnected { get; set; }

        /// <summary>
        /// Defines when the last connection was made successfully
        /// </summary>
        public DateTime LastConnectionSuccess { get; set; }

        /// <summary>
        /// Attempts a connection with the instance
        /// </summary>
        /// <returns><see cref="bool"/> if the connection attempt was successful</returns>
        public bool Connect();

        /// <summary>
        /// Disconnects from the database instance
        /// </summary>
        /// <returns></returns>
        public bool Disconnect();

        /// <summary>
        /// Validates if the service is available
        /// </summary>
        /// <returns><see cref="bool"/> if the service is available</returns>
        public bool IsServiceAvailable();

        /// <summary>
        /// Validates the roundtrip latency to the service
        /// </summary>
        /// <returns><inheritdoc cref="ushort"/> Denoting the latency to the service</returns>
        public ushort ServiceLatency();
    }
}