﻿using Pamaxie.Database.Extensions.Data;
using Pamaxie.Database.Sql;
using System.Linq;
using System;
using System.Collections.Generic;
using Pamaxie.Data;

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
            using SqlDbContext dbContext = new();
            return dbContext.Users.FirstOrDefault(x => x.GoogleUserId == GoogleUserId);
        }
        
        /// <summary>
        /// Creates a new Application via the reached in variable
        /// </summary>
        /// <param name="userProfile"> The user to be created</param>
        /// <returns><see cref="bool"/> was success?</returns>
        public static bool CreateUser(IProfileData userProfile)
        {
            using SqlDbContext dbContext = new();
            try
            {
                User existingDbUser =
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
                
                User user = new() { GoogleUserId = userProfile.GoogleClaimUserId, Email = userProfile.EmailAddress, Username = userProfile.UserName};
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
            using SqlDbContext dbContext = new();
            User user = dbContext.Users.FirstOrDefault(x => x.Id == UserProfile.Id);
            if (user == null) return false;
            List<Application> applications = ApplicationExtensions.GetApplications(UserProfile.Id);
            foreach (Application application in applications)
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
