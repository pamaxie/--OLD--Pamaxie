using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
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
            Url = DataContext.DataInstances + "/User";
        }

        /// <inheritdoc/>
        public IEnumerable<PamaxieApplication> GetAllApplications(PamaxieUser value)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Url + "/GetAllApplications");
            string body = JsonConvert.SerializeObject(value);
            byte[] bodyBytes = Encoding.ASCII.GetBytes(body);
            requestMessage.Content = new ByteArrayContent(bodyBytes);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            Stream stream = response.Content.ReadAsStream();
            StreamReader reader = new StreamReader(stream, Encoding.Default);
            string content = reader.ReadToEnd();

            if (string.IsNullOrEmpty(content))
            {
                throw new WebException("Something went wrong here");
            }

            IEnumerable<PamaxieApplication> result =
                JsonConvert.DeserializeObject<IEnumerable<PamaxieApplication>>(content);
            return result;
        }

        /// <inheritdoc/>
        public bool VerifyEmail(PamaxieUser value)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Url + "/VerifyEmail");
            string body = JsonConvert.SerializeObject(value);
            byte[] bodyBytes = Encoding.ASCII.GetBytes(body);
            requestMessage.Content = new ByteArrayContent(bodyBytes);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            Stream stream = response.Content.ReadAsStream();
            StreamReader reader = new StreamReader(stream, Encoding.Default);
            string content = reader.ReadToEnd();

            if (string.IsNullOrEmpty(content))
            {
                throw new WebException("Something went wrong here");
            }

            bool result = JsonConvert.DeserializeObject<bool>(content);
            return result;
        }
    }
}