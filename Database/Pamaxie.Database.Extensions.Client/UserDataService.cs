using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using Pamaxie.Database.Extensions.Client.Extensions;

namespace Pamaxie.Database.Extensions.Client
{
    /// Implementation to get <see cref="PamaxieUser"/> data from the server
    internal sealed class UserDataService : ClientDataServiceBase<PamaxieUser>, IUserDataService
    {
        /// <inheritdoc/>
        internal UserDataService(PamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
            ParentPath = DataContext.ApiUrl + "User";
        }

        /// <inheritdoc/>
        public IEnumerable<PamaxieApplication> GetAllApplications(PamaxieUser value)
        {
            HttpRequestMessage requestMessage = DataContext.GetRequestMessage(ParentPath + "/GetAllApplications", value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            return response.ReadJsonResponse<IEnumerable<PamaxieApplication>>();
        }

        /// <inheritdoc/>
        public bool VerifyEmail(PamaxieUser value)
        {
            HttpRequestMessage requestMessage = DataContext.PostRequestMessage(ParentPath + "/VerifyEmail", value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            return response.ReadJsonResponse<bool>();
        }
    }
}