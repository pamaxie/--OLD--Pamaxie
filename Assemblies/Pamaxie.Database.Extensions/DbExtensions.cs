using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using Pamaxie.Database.Sql;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pamaxie.Database.Extensions
{
    public static class DbExtensions
    {
        /// <summary>
        /// Verifies the connection to the MSSQL Database
        /// </summary>
        /// <param name="errorReason"></param>
        /// <returns></returns>
        public static bool SqlDbCheckup(out string errorReason)
        {
            errorReason = string.Empty;
            using SqlDbContext dbContext = new();
            if (!dbContext.Database.CanConnect())
            {
                errorReason += "SQL Connection was not possible. Please ensure the value is set. For configuration examples please view our documentation at https://wiki.pamaxie.com";
                return false;
            }
            IMigrationsAssembly migrationsAssembly = dbContext.GetService<IMigrationsAssembly>();
            IHistoryRepository historyRepository = dbContext.GetService<IHistoryRepository>();
            IEnumerable<string> all = migrationsAssembly.Migrations.Keys;
            IEnumerable<string> applied = historyRepository.GetAppliedMigrations().Select(r => r.MigrationId);
            List<string> pending = all.Except(applied).ToList();;
            Console.WriteLine("Checking if database exists...");
            if (dbContext.Database.EnsureCreated()) Console.WriteLine("SQL Database was created and schemas were applied");

            string applyMigrations = Environment.GetEnvironmentVariable("ApplyMigrations");
            bool.TryParse(applyMigrations, out bool doMigration);
            if (!pending.Any()) return true;
            Console.WriteLine($"{pending.Count} Pending Migrations were found.");
            if (!doMigration)
            {
                errorReason = "Pending Migrations were found, but the \"Apply Migrations\" variable was either not set or set to false.\n";
                return false;
            }
            try
            {
                Console.WriteLine("Applying Migrations.");
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations were applied successfully");
                return true;
            }
            catch
            {
                Console.WriteLine(string.Empty);
                errorReason += $"Failed automatically applying {pending.Count} migrations. Please ensure your database is properly configured.\n";
                return false;
            }
        }

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
