using System;

namespace Pamaxie.Database.Extensions.Client
{
    public class DatabaseService
    {
        /// <summary>
        /// Contains the service responsible for interacting with user data in the redis database
        /// </summary>
        internal static  UserDataService UserService { get; private set; }

        /// <summary>
        /// Contains the Service responsible for interacting with application data in the redis database
        /// </summary>
        internal static ApplicationDataService ApplicationService { get; private set; }

        public DatabaseService()
        {
            UserService = new UserDataService(this);
            ApplicationService = new ApplicationDataService(this);
        }
        
        public bool IsDatabaseAvailable()
        {
            throw new NotImplementedException();
        }

        public ushort DatabaseLatency()
        {
            throw new NotImplementedException();
        }
    }
}