using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using Pamaxie.Database.Extensions.Client.Extensions;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// TODO
    /// </summary>
    /// <typeparam name="T">TODO</typeparam>
    internal class ClientDataServiceBase<T> : IDataServiceBase<T> where T : IDatabaseObject
    {
        /// <summary>
        /// Data Context responsible for connecting to Pamaxie
        /// </summary>
        internal IPamaxieDataContext DataContext { get; init; }

        /// <summary>
        /// The Service that should be used to connect to the database
        /// </summary>
        internal DatabaseService Service { get; init; }

        /// <summary>
        /// TODO
        /// </summary>
        internal string Url { get; init; }

        /// <inheritdoc/>
        public T Get(string key)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Url + "/" + key);
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
                throw new WebException("Bad data");
            }

            T result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }

        /// <inheritdoc/>
        public T Create(T value)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Url + "/Create");
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

            T result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }

        /// <inheritdoc/>
        public bool TryCreate(T value, out T createdValue)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Url + "/TryCreate");
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

            createdValue = JsonConvert.DeserializeObject<T>(content);
            return true;
        }

        /// <inheritdoc/>
        public T Update(T value)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, Url + "/Update");
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

            T result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }

        /// <inheritdoc/>
        public bool TryUpdate(T value, out T updatedValue)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, Url + "/TryUpdate");
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

            updatedValue = JsonConvert.DeserializeObject<T>(content);
            return true;
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(T value, out T updatedOrCreatedValue)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, Url + "/UpdateOrCreate");
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

            updatedOrCreatedValue = JsonConvert.DeserializeObject<T>(content);

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Exists(string key)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, Url + "/Exists");
            string body = JsonConvert.SerializeObject(key);
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

        /// <inheritdoc/>
        public bool Delete(T value)
        {
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Delete, Url + "/Delete");
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