using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.DatabaseExtensions;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.Server._BASE_;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Server
{
    /// Implementation to get <see cref="IPamaxieApplication"/> data from the server
    public class ApplicationDataService : ServerDataServiceBase<IPamaxieApplication>
    {
        /// <inheritdoc/>
        internal ApplicationDataService(PamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
        }

        /// <summary>
        /// Gets a list of <see cref="IPamaxieApplication"/> from a user key
        /// </summary>
        /// <param name="value">The key of the user who owns the applications</param>
        /// <returns>A list of all applications the user owns</returns>
        public IEnumerable<IPamaxieApplication> GetFromUser(IPamaxieUser value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Enables or Disables the application
        /// </summary>
        /// <param name="value">The application that will be enabled or disabled</param>
        /// <returns><see cref="bool"/> if the operation was successful and the value was changed</returns>
        public bool EnableOrDisable(IPamaxieApplication value)
        {
            throw new NotImplementedException();
        }
    }
}