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
        internal UserDataService(IPamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
            Url = DataContext.DataInstances + "User";
        }

        /// <inheritdoc/>
        public IEnumerable<PamaxieApplication> GetAllApplications(PamaxieUser value)
        {
            HttpRequestMessage requestMessage = WebExtensions.GetRequestMessage(new Uri(Url + "/GetAllApplications"), value);
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
            HttpRequestMessage requestMessage = WebExtensions.PostRequestMessage(new Uri(Url + "/VerifyEmail"), value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            return response.ReadJsonResponse<bool>();
        }
    }
}