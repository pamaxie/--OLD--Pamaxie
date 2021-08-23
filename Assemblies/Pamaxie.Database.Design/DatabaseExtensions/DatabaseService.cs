using System;

namespace Pamaxie.Database.Extensions.DatabaseExtensions
{
    /// <inheritdoc/>
    public class DatabaseService : IDatabaseService
    {
        /// <inheritdoc/>
        string IDatabaseService.RedisInstances { get; set; }

        /// <inheritdoc/>
        string IDatabaseService.Password { get; set; }

        /// <inheritdoc/>
        int IDatabaseService.ReconnectionAttempts { get; set; }

        /// <inheritdoc/>
        public bool Connect()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool IsDatabaseAvailable()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ushort DatabaseLatency()
        {
            throw new NotImplementedException();
        }
    }
}