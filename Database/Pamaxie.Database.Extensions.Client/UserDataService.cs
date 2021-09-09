using System;
using System.Collections.Generic;
using Pamaxie.Data;
using Pamaxie.Database.Design;

namespace Pamaxie.Database.Extensions.Client
{
    /// Implementation to get <see cref="PamaxieUser"/> data from the server
    internal sealed class UserDataService : ClientDataServiceBase<PamaxieUser>, IUserDataService
    {
        /// <inheritdoc/>
        internal UserDataService(IPamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
            Url = DataContext.DataInstances + "/User";
        }

        /// <inheritdoc/>
        public IEnumerable<PamaxieApplication> GetAllApplications(PamaxieUser value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool VerifyEmail(PamaxieUser value)
        {
            throw new NotImplementedException();
        }
    }
}