using Pamaxie.Database.Sql;
using Pamaxie.Database.Sql.DataClasses;
using System.Diagnostics;
using System.Linq;
using System;
using System.Collections.Generic;
using Pamaxie.Database.Extensions.Basic;
using Pamaxie.Database.Extensions.Data;

namespace Pamaxie.Extensions
{
    public static class UserExtensions
    {
        /// <summary>
        /// Gets a user via their google User Id
        /// </summary>
        /// <param name="userId">Id of the user to get the applications from</param>
        /// <returns></returns>
        public static User GetUser(string GoogleUserId)
        {
            using var dbContext = new SqlDbContext();
            return dbContext.Users.FirstOrDefault(x => x.GoogleUserId == GoogleUserId);
        }
        
        /// <summary>
        /// Creates a new Application via the reached in variable
        /// </summary>
        /// <param name="userProfile"> The user to be created</param>
        /// <returns><see cref="bool"/> was success?</returns>
        public static bool CreateUser(IProfileData userProfile)
        {
            using var dbContext = new SqlDbContext();
            try
            {
                var existingDbUser =
                    dbContext.Users.FirstOrDefault(x => x.GoogleUserId == userProfile.GoogleClaimUserId);
                if (existingDbUser != null)
                {
                    existingDbUser.DeletedAccount = false;
                    existingDbUser.Email = userProfile.EmailAddress;
                    existingDbUser.Username = userProfile.UserName;
                    dbContext.Update(existingDbUser);
                    dbContext.SaveChanges();
                    return true;
                }
                
                var user = new User() { GoogleUserId = userProfile.GoogleClaimUserId, Email = userProfile.EmailAddress, Username = userProfile.UserName};
                dbContext.Users.Add(user);
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
        /// Deletes all data associated with a user profile from our database
        /// </summary>
        /// <param name="UserProfile">User profile to delete</param>
        /// <returns><see cref="bool"/> was successful?</returns>
        public static bool DeleteUserData(this IProfileData UserProfile)
        {
            using var dbContext = new SqlDbContext();
            var user = dbContext.Users.FirstOrDefault(x => x.Id == UserProfile.Id);
            if (user == null)
                return false;
            var applications = ApplicationExtensions.GetApplications(UserProfile.Id);
            foreach (var application in applications)
            {
                application.DeleteApplication();

            }

            user.Email = string.Empty;
            user.Username = string.Empty;
            user.DeletedAccount = true;
            dbContext.Users.Update(user);
            dbContext.SaveChanges();
            return true;
        }


    }
}
