using StackExchange.Redis;
using System;

namespace Pamaxie.Database.Redis
{
    internal static class RedisData
    {
        /// <summary>
        /// The release build of this should use another address this would be your local server address 
        /// currently this is hard coded you wanna make this accessable through a config in the future though.
        /// You could theoretically add multiple server endpoints here like this: server1:6379,server2:6379
        /// </summary>
        internal static readonly ConnectionMultiplexer Redis =
#if DEBUG
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("PamaxiePublicRedisAddr") ?? string.Empty);
#else
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("PamaxieRedisAddr") ?? string.Empty);
#endif

    }
}