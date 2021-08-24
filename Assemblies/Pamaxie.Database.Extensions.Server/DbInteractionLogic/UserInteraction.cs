using System;
using System.Data;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.InteractionObjects;
using Pamaxie.Database.Extensions.Server._BASE_;
using StackExchange.Redis;

namespace Pamaxie.Database.Extensions.Server
{
    /// <inheritdoc cref="IUserDataService" />
    public class UserDataService : ServerDataServiceBase<IPamaxieUser>
    {
        /// <inheritdoc/>
        internal UserDataService(PamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
        }

        /// <summary>
        /// Verifies the email of the user
        /// </summary>
        /// <param name="value">The user that will have their email verified</param>
        /// <returns><see cref="bool"/> if the operation was successful and the email was verified</returns>
        public bool VerifyEmail(IPamaxieUser value)
        {
            throw new NotImplementedException();
        }
    }
}