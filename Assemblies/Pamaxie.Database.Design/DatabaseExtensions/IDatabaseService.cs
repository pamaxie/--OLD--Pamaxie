namespace Pamaxie.Database.Extensions.DatabaseExtensions
{
    /// <summary>
    /// Service responsible for handling interaction with the database or database api. This automatically detects
    /// the connection context.
    /// </summary>
    public interface IDatabaseService
    {
        /// <summary>
        /// Defines the available Redis instances
        /// </summary>
        internal string RedisInstances { get; set; }
        
        /// <summary>
        /// Defines the password to use for the redis instances
        /// </summary>
        internal string Password { get; set; }
        
        /// <summary>
        /// How many attempts should be made to connect to the database
        /// </summary>
        internal int ReconnectionAttempts { get; set; }
        
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