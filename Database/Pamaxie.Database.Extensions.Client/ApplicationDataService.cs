using System;
using System.Net;
using System.Net.Http;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using Pamaxie.Database.Extensions.Client.Extensions;

namespace Pamaxie.Database.Extensions.Client
{
    /// Implementation to get <see cref="PamaxieApplication"/> data from the server
    internal sealed class ApplicationDataService : ClientDataServiceBase<PamaxieApplication>, IApplicationDataService
    {
        /// <inheritdoc/>
        internal ApplicationDataService(PamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
            ParentPath = DataContext.ApiUrl + "/Application";
        }

        /// <inheritdoc/>
        public PamaxieUser GetOwner(PamaxieApplication value)
        {
            HttpRequestMessage requestMessage = DataContext.GetRequestMessage(ParentPath + "/GetOwner", value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            return response.ReadJsonResponse<PamaxieUser>();
        }

        /// <inheritdoc/>
        public PamaxieApplication EnableOrDisable(PamaxieApplication value)
        {
            HttpRequestMessage requestMessage = DataContext.PutRequestMessage(ParentPath + "/EnableOrDisable", value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            return response.ReadJsonResponse<PamaxieApplication>();
        }

        /// <inheritdoc/>
        public bool VerifyAuthentication(PamaxieApplication value)
        {
            HttpRequestMessage requestMessage =
                DataContext.GetRequestMessage(ParentPath + "/VerifyAuthentication", value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            return response.ReadJsonResponse<bool>();
        }
    }
}