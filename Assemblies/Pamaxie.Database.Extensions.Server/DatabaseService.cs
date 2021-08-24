using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Pamaxie.Database.Extensions.DatabaseExtensions;
using Pamaxie.Database.Extensions.InteractionObjects;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Server
{
    public class DatabaseService : IDatabaseService<ConnectionMultiplexer>
    {
        public DatabaseService(PamaxieDataContext dataContext)
        {
            DataContext = dataContext;
            Applications = new ApplicationDataService(dataContext, this);
        }

        #region Data Services

        /// <summary>
        /// Contains the Service responsible for interacting with application data in the redis database
        /// </summary>
        public ApplicationDataService Applications { get; }

        /// <summary>
        /// Contains the service responsible for interacting with user data in the redis database
        /// </summary>
        public UserDataService Users { get; }
        

        public ConnectionMultiplexer Service { get; set;  }

        #endregion Data Services

        /// <inheritdoc/>
        public IPamaxieDataContext DataContext { get; }

        /// <inheritdoc/>
        public bool ConnectionSuccess { get; set; }

        /// <inheritdoc/>
        public DateTime LastConnectionSuccess { get; set; }


        /// <inheritdoc/>
        public bool Connect()
        {
            Service = ConnectionMultiplexer.Connect((DataContext.ConnectionString()) ?? string.Empty);
            if (Service != null)
            {
                ConnectionSuccess = true;
                LastConnectionSuccess = DateTime.Now;
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool IsDatabaseAvailable()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ushort DatabaseLatency()
        {
            throw new NotImplementedException();
        }
    }
}