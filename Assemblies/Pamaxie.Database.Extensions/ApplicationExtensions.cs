﻿using Pamaxie.Database.Extensions.Basic;
using Pamaxie.Database.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pamaxie.Data;

namespace Pamaxie.Extensions
{
    public static class ApplicationExtensions
    {
        /// <summary>
        /// Verify the Authentication of the User via the reached in applicationAuth
        /// </summary>
        /// <param name="userId">Id of the user to get the applications from</param>
        /// <returns></returns>
        public static List<Application> GetApplications(long userId)
        {
            using SqlDbContext dbContext = new();
            return dbContext.Applications.Where(x => x.UserId == userId).ToList();
        }

        /// <summary>
        /// Get the last Id of the Application Extensions
        /// </summary>
        /// <returns><see cref="long"/> representing the last known Id</returns>
        public static long GetLastIndex()
        {
            using SqlDbContext dbContext = new();
            long? application = dbContext.Applications.OrderBy(key => key.ApplicationId).LastOrDefault()?.ApplicationId;
            return application ?? 1;
        }

        /// <summary>
        /// Creates a new Application via the reached in variable
        /// </summary>
        /// <param name="application"><see cref="Application"/> that should be created</param>
        /// <param name="createdApplication">Application that has been created</param>
        /// <returns><see cref="bool"/> was success?</returns>
        public static bool CreateApplication(Application application, out Application createdApplication)
        {
            createdApplication = null;
            using SqlDbContext dbContext = new();
            try
            {
                application.ApplicationId = default;
                application.AppTokenHash = BCrypt.Net.BCrypt.HashPassword(application.AppToken, ByCrptExt.CalculateSaltCost());
                application.AppToken = string.Empty;
                EntityEntry<Application> dbApp = dbContext.Applications.Add(new Application { 
                    ApplicationId = application.ApplicationId,
                    ApplicationName = application.ApplicationName,
                    AppTokenHash = application.AppTokenHash,
                    Disabled = application.Disabled,
                    UserId = application.UserId,
                    LastAuth = application.LastAuth,
                    RateLimited = false
                });
                createdApplication = dbApp.Entity;
                
                dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        
        /// <summary>
        /// Disables an application
        /// </summary>
        /// <returns><see cref="bool"/> if operation was successful</returns>
        public static bool SetApplicationStatus(this Application application, bool enabled)
        {
            using SqlDbContext dbContext = new();
            Application dbApp = dbContext.Applications.FirstOrDefault(x => x.ApplicationId == application.ApplicationId);
            if (dbApp == null) return false;

            //Sets the application to be disabled. This prevents login attempts 
            dbApp.Disabled = !enabled;
            dbContext.SaveChanges();
            return true;
        }

        /// <summary>
        /// Deletes an application
        /// </summary>
        /// <returns><see cref="bool"/> if operation was successful</returns>
        public static bool DeleteApplication(this Application application)
        {
            using SqlDbContext dbContext = new();
            Application dbApp = dbContext.Applications.FirstOrDefault(x => x.ApplicationId == application.ApplicationId);
            if (dbApp == null) return false;

            //Set the column to deleted and clear / anonymize its values
            dbApp.LastAuth = DateTime.MinValue;
            dbApp.ApplicationName = string.Empty;
            dbApp.AppTokenHash = string.Empty;
            dbApp.Disabled = false;
            dbApp.Deleted = true;
            dbApp.UserId = 0;
            dbContext.SaveChanges();
            return true;
        }
    }
}
