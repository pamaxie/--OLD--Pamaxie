using System;
using StackExchange.Redis;

namespace Pamaxie.Database.Redis
{
    internal static class RedisData
    {
        /// <summary>
        /// This defines the redis address for connecting to the database. This will be removed in the future.
        /// </summary>
        internal static readonly ConnectionMultiplexer Redis =
#if DEBUG
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("PamaxiePublicRedisAddr") ??
                                          "localhost:6379");
#else
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("PamaxieRedisAddr") ?? string.Empty);
#endif
    }
}