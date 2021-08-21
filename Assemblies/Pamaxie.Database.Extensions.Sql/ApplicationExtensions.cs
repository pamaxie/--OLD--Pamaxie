using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Sql.Basic;
using Pamaxie.Database.Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pamaxie.Extensions.Sql
{
    public static class ApplicationExtensions
    {
        public static SqlDbContext DbContext { private get; set; } = new();

        /// <summary>
        /// Verify the Authentication of the User via the reached in applicationAuth
        /// </summary>
        /// <param name="userId">Id of the user to get the applications from</param>
        /// <returns></returns>
        public static IEnumerable<Application> GetApplications(long userId)
        {
            using (DbContext)
                return DbContext.Applications.Where(x => x.UserId == userId).ToList();
        }

        /// <summary>
        /// Get the last Id of the Application Extensions
        /// </summary>
        /// <returns><see cref="long"/> representing the last known Id</returns>
        public static long GetLastIndex()
        {
            using (DbContext)
            {
                long? application = DbContext.Applications.OrderBy(key => key.ApplicationId).LastOrDefault()
                    ?.ApplicationId;
                return application ?? 1;
            }
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
            using (DbContext)
            {
                try
                {
                    application.ApplicationId = default;
                    application.AppTokenHash =
                        BCrypt.Net.BCrypt.HashPassword(application.AppToken, ByCrptExt.CalculateSaltCost());
                    application.AppToken = string.Empty;
                    EntityEntry<Application> dbApp = DbContext.Applications.Add(new Application
                    {
                        ApplicationId = application.ApplicationId,
                        ApplicationName = application.ApplicationName,
                        AppTokenHash = application.AppTokenHash,
                        Disabled = application.Disabled,
                        UserId = application.UserId,
                        LastAuth = application.LastAuth,
                        RateLimited = false
                    });
                    createdApplication = dbApp.Entity;

                    DbContext.SaveChanges();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return false;
                }
            }
        }
        
        /// <summary>
        /// Disables an application
        /// </summary>
        /// <returns><see cref="bool"/> if operation was successful</returns>
        public static bool SetApplicationStatus(this Application application, bool enabled)
        {
            using (DbContext)
            {
                Application dbApp =
                    DbContext.Applications.FirstOrDefault(x => x.ApplicationId == application.ApplicationId);
                if (dbApp == null) return false;

                //Sets the application to be disabled. This prevents login attempts 
                dbApp.Disabled = !enabled;
                DbContext.SaveChanges();
                return true;
            }
        }

        /// <summary>
        /// Deletes an application
        /// </summary>
        /// <returns><see cref="bool"/> if operation was successful</returns>
        public static bool DeleteApplication(this Application application)
        {
            using (DbContext)
            {
                Application dbApp =
                    DbContext.Applications.FirstOrDefault(x => x.ApplicationId == application.ApplicationId);
                if (dbApp == null) return false;

                //Set the column to deleted and clear / anonymize its values
                dbApp.LastAuth = DateTime.MinValue;
                dbApp.ApplicationName = string.Empty;
                dbApp.AppTokenHash = string.Empty;
                dbApp.Disabled = false;
                dbApp.Deleted = true;
                dbApp.UserId = 0;
                DbContext.SaveChanges();
                return true;
            }
        }
    }
}
