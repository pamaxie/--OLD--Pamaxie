using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Database.Design;

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

        /// <inheritdoc/>
        public T Get(string key)
        {
            HttpRequestMessage requestMessage = new(HttpMethod.Get, DataContext.DataInstances);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", DataContext.Password);
            HttpResponseMessage response = Service.Service.Send(requestMessage);
            string stream = response.Content.ReadAsStream().ToString();
            if (stream == null)
                throw new WebException("OHH NO!");
            T content = JsonConvert.DeserializeObject<T>(stream);
            return content;
        }

        /// <inheritdoc/>
        public T Create(T value)
        {
            HttpRequestMessage requestMessage = new(HttpMethod.Post, DataContext.DataInstances);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", DataContext.Password);
            string body = JsonConvert.SerializeObject(value);
            byte[] bodyBytes = Encoding.ASCII.GetBytes(body);
            requestMessage.Content = new ByteArrayContent(bodyBytes);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = Service.Service.Send(requestMessage);
            string stream = response.Content.ReadAsStream().ToString();
            if (stream == null)
                throw new WebException("OHH NO!");
            T content = JsonConvert.DeserializeObject<T>(stream);
            return content;
        }

        /// <inheritdoc/>
        public bool TryCreate(T value, out T createdValue)
        {
            HttpRequestMessage requestMessage = new(HttpMethod.Post, DataContext.DataInstances);
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", DataContext.Password);
            string body = JsonConvert.SerializeObject(value);
            byte[] bodyBytes = Encoding.ASCII.GetBytes(body);
            requestMessage.Content = new ByteArrayContent(bodyBytes);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = Service.Service.Send(requestMessage);
            string stream = response.Content.ReadAsStream().ToString();
            if (stream == null)
                throw new WebException("OHH NO!");
            createdValue = JsonConvert.DeserializeObject<T>(stream);
            return true;
        }

        /// <inheritdoc/>
        public T Update(T value)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryUpdate(T value, out T updatedValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(T value, out T updatedOrCreatedValue)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool Delete(T value)
        {
            throw new NotImplementedException();
        }
    }
}