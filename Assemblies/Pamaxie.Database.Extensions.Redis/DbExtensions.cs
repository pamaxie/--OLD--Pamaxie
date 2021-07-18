using StackExchange.Redis;
using System;

namespace Pamaxie.Database.Extensions.Redis
{
    public static class DbExtensions
    {
        /// <summary>
        /// Verifies the connection to the Redis Database
        /// </summary>
        /// <param name="errorReason">Reason the Redis database could not connect</param>
        /// <returns></returns>
        public static bool RedisDbCheckup(out string errorReason)
        {
            errorReason = string.Empty;
            ConnectionMultiplexer connectionMultiplexer =
#if DEBUG
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("PamaxiePublicRedisAddr") ?? string.Empty);
#else
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("PamaxieRedisAddr") ?? string.Empty);
#endif
            if (connectionMultiplexer.IsConnected)
            {
                IDatabase db = connectionMultiplexer.GetDatabase();

                if (!db.StringSet("testKey", "testValue"))
                {
                    errorReason += "We tried setting a value but it didn't seem to get set. Please verify if your Redis Database is setup properly. The TTL for this key is 10 minutes. The key is: \"testKey\" the value should be \"testValue\"";
                    return false;
                }
                if (db.StringGet("testKey") != "testValue")
                {
                    errorReason += "We tried setting a value but it didn't seem to get set to the right value. Please verify if your Redis Database is setup properly. The TTL for this key is 10 minutes. The key is: \"testKey\" the value should be \"testValue\"";
                    return false;
                }
                if (!db.KeyDelete("testKey"))
                {
                    errorReason += "We deleting our test key but it didn't seem to happen. This should not be the case. Please verify if your Redis Database is setup properly. The TTL for this key is 10 minutes. The key is: \"testKey\" the value should be \"testValue\"";
                    return false;
                }
                db.KeyDelete("testKey");
                return true;
            }
            errorReason += "Redis Connection was not possible. Please ensure the value is set. For configuration examples please view our documentation at https://wiki.pamaxie.com";
            return true;
        }
    }
}
