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
    /// Implementation to get <see cref="PamaxieApplication"/> data from the server
    internal sealed class ApplicationDataService : ClientDataServiceBase<PamaxieApplication>, IApplicationDataService
    {
        /// <inheritdoc/>
        internal ApplicationDataService(IPamaxieDataContext dataContext, DatabaseService service)
        {
            DataContext = dataContext;
            Service = service;
            Url = DataContext.DataInstances + "/Application";
        }

        /// <inheritdoc/>
        public PamaxieUser GetOwner(PamaxieApplication value)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Url + "/GetOwner");
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

            PamaxieUser result = JsonConvert.DeserializeObject<PamaxieUser>(content);
            return result;
        }

        /// <inheritdoc/>
        public PamaxieApplication EnableOrDisable(PamaxieApplication value)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, Url + "/EnableOrDisable");
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

            PamaxieApplication result = JsonConvert.DeserializeObject<PamaxieApplication>(content);
            return result;
        }

        /// <inheritdoc/>
        public bool VerifyAuthentication(PamaxieApplication value)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Url + "/VerifyAuthentication");
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