using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pamaxie.Database.Extensions.DatabaseExtensions;

namespace Pamaxie.Database.Extensions.Server
{
    public class PamaxieDataContext : IPamaxieDataContext
    {
        /// <summary>
        /// Connection properties to a database with a password and a custom amount of reconnection attempts
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="password"></param>
        /// <param name="reconnectionAttempts"></param>
        public PamaxieDataContext(string instanceName, string password, int reconnectionAttempts)
        {
            DataInstances = instanceName;
            Password = password;
            ReconnectionAttempts = reconnectionAttempts;
        }

        /// <summary>
        /// Connection properties to a database with a password and a custom amount of reconnection attempts
        /// </summary>
        /// <param name="instanceName"></param>
        /// <param name="password"></param>
        public PamaxieDataContext(string instanceName, string password) : this(instanceName, password, 3)
        {
        }

        /// <summary>
        /// Connection properties to a database without a password !!THIS IS NOT RECOMMENDED!!
        /// </summary>
        /// <param name="instanceName"></param>
        [Obsolete("This should not be used, since this means u don't have a password on your database, which exposes your database to attacks. Please always make sure to use a password on your database.")]
        public PamaxieDataContext(string instanceName) : this(instanceName, string.Empty, 3)
        {
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <returns></returns>
        public string ConnectionString()
        {
            return string.Empty;
        }

        public string DataInstances { get; }
        public string Password { get; }
        public int ReconnectionAttempts { get; }
    }
}
