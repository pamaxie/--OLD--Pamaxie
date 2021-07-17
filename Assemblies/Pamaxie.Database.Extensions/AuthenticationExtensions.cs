using Pamaxie.Database.Extensions.Basic;
using Pamaxie.Database.Sql;
using System;
using System.Linq;
using Pamaxie.Data;

namespace Pamaxie.Extensions
{
    public static class AuthenticationExtensions
    {
        /// <summary>
        /// Verify the Authentication of the User via the reached in applicationAuth
        /// </summary>
        /// <param name="applicationAuth"></param>
        /// <returns></returns>
        public static bool VerifyAuth(this Application applicationAuth)
        {
            using SqlDbContext dbContext = new();
            Application dbAppAuth = dbContext.Applications.FirstOrDefault(x => x.ApplicationId == applicationAuth.ApplicationId);
            if (dbAppAuth != null && (dbAppAuth is {Disabled: true} || dbAppAuth.Deleted))
            {
                return false;
            }

            if (dbAppAuth != null)
            {
                dbAppAuth.LastAuth = DateTime.Now;
                dbContext.Applications.Update(dbAppAuth);
                dbContext.SaveChanges();
            }


            return dbAppAuth != null && BCrypt.Net.BCrypt.Verify(applicationAuth.AppToken, dbAppAuth.AppTokenHash) && !dbAppAuth.Disabled;
        }

        /// <summary>
        /// Update the Application authentication or create a new one if the reached in one does not exist
        /// </summary>
        /// <param name="applicationAuthentication"></param>
        /// <param name="createNew">Should a new value be created if it doesn't exist?</param>
        /// <returns><see cref="bool"/> Has been created / altered?</returns>
        public static bool UpdateValue(this Application applicationAuthentication, bool createNew)
        {
            using SqlDbContext dbContext = new();
            Application dbAppAuth = dbContext.Applications.FirstOrDefault(x => x.ApplicationId == applicationAuthentication.ApplicationId);
            if (dbAppAuth == null && createNew) return ((Application) null).CreateValue(out _, false);

            //Creating the Hash and calculating the optimal hash cost automatically
            if (dbAppAuth != null)
            {
                dbAppAuth.AppTokenHash =
                    BCrypt.Net.BCrypt.HashPassword(applicationAuthentication.AppToken, ByCrptExt.CalculateSaltCost());
                dbAppAuth.AppToken = null;
                dbContext.Applications.Update(dbAppAuth);
            }

            return true;
        }


        /// <summary>
        /// Update the Application authentication or create a new one if the reached in one does not exist
        /// </summary>
        /// <param name="applicationAuthentication">The <see cref="Application"/> that should be created</param>
        /// <param name="creationError">The reason the creation failed</param>
        /// <param name="shouldThrow">Should the method throw exceptions or just output the error</param>
        /// <returns><see cref="bool"/> Has been created / altered?</returns>
        private static bool CreateValue(this Application applicationAuthentication, out string creationError, bool shouldThrow)
        {
            creationError = null;
            using SqlDbContext dbContext = new();
            if (default == applicationAuthentication.ApplicationId)
            {
                creationError = "You need to input an Application Id to create a new Application";
                if (shouldThrow) throw new InvalidOperationException(creationError);
                return false;
            }
                    
            if (string.IsNullOrEmpty(applicationAuthentication.AppToken))
            {
                creationError = "You need to input an App Token to create a new Application";
                if (shouldThrow) throw new InvalidOperationException(creationError);
                return false;
            }

            if (string.IsNullOrEmpty(applicationAuthentication.AppTokenHash))
            {
                creationError = "You cannot have a hash already for a non known application. The hashing is handled by us because we determine automatic optimal salt.";
                if (shouldThrow) throw new InvalidOperationException(creationError);
                return false;
            }

            //Creating the hash and calculating the optimal hash cost automatically
            applicationAuthentication.AppTokenHash = BCrypt.Net.BCrypt.HashPassword(applicationAuthentication.AppToken, ByCrptExt.CalculateSaltCost());
            applicationAuthentication.AppToken = null;  
            dbContext.Applications.Add(applicationAuthentication);
            return true;
        }
    }
}
