﻿using Pamaxie.Database.Sql;
using System.Linq;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using System;
using StackExchange.Redis;

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
            using var dbContext = new SqlDbContext();
            if (!dbContext.Database.CanConnect())
            {
                errorReason += "SQL Connection was not possible. Please ensure the value is set. For configuration examples please view our documentation at https://wiki.pamaxie.com";
                return false;
            }
            var migrationsAssembly = dbContext.GetService<IMigrationsAssembly>();
            var historyRepository = dbContext.GetService<IHistoryRepository>();
            var all = migrationsAssembly.Migrations.Keys;
            var applied = historyRepository.GetAppliedMigrations().Select(r => r.MigrationId);
            var pending = all.Except(applied).ToList();;
            Console.WriteLine("Checking if database exists...");
            if (dbContext.Database.EnsureCreated())
            {
                Console.WriteLine("SQL Database was created and schemas were applied");
            }

            var applyMigrations = Environment.GetEnvironmentVariable("ApplyMigrations");
            bool.TryParse(applyMigrations, out var doMigration);
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
            catch (Exception ex)
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
            var connectionMultiplexer =
#if DEBUG
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("PamaxiePublicRedisAddr") ?? string.Empty);
#else
            ConnectionMultiplexer.Connect(Environment.GetEnvironmentVariable("PamaxieRedisAddr") ?? string.Empty);
#endif
            if (connectionMultiplexer.IsConnected)
            {
                var db = connectionMultiplexer.GetDatabase();

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
