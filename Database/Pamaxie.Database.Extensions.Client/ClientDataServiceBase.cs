using System;
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
            HttpRequestMessage requestMessage = new(HttpMethod.Get, Url + "/Get");
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);
            if (!response.IsSuccessStatusCode)
                throw new WebException(response.StatusCode.ToString());
            Stream stream = response.Content.ReadAsStream();
            StreamReader reader = new(stream, Encoding.Default);
            string content = reader.ReadToEnd();
            if (string.IsNullOrEmpty(content))
                throw new WebException("Something went wrong here");
            T result = JsonConvert.DeserializeObject<T>(content);     
            return result;
        }

        /// <inheritdoc/>
        public T Create(T value)
        {
            HttpRequestMessage requestMessage = new(HttpMethod.Post, Url + "/Create");
            string body = JsonConvert.SerializeObject(value);
            byte[] bodyBytes = Encoding.ASCII.GetBytes(body);
            requestMessage.Content = new ByteArrayContent(bodyBytes);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);
            if (!response.IsSuccessStatusCode)
                throw new WebException(response.StatusCode.ToString());
            Stream stream = response.Content.ReadAsStream();
            StreamReader reader = new(stream, Encoding.Default);
            string content = reader.ReadToEnd();
            if (string.IsNullOrEmpty(content))
                throw new WebException("Something went wrong here");
            T result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }

        /// <inheritdoc/>
        public bool TryCreate(T value, out T createdValue)
        {
            HttpRequestMessage requestMessage = new(HttpMethod.Post, DataContext.DataInstances);
            string body = JsonConvert.SerializeObject(value);
            byte[] bodyBytes = Encoding.ASCII.GetBytes(body);
            requestMessage.Content = new ByteArrayContent(bodyBytes);
            requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);
            if (!response.IsSuccessStatusCode)
                throw new WebException(response.StatusCode.ToString());
            Stream stream = response.Content.ReadAsStream();
            StreamReader reader = new(stream, Encoding.Default);
            string content = reader.ReadToEnd();
            if (string.IsNullOrEmpty(content))
                throw new WebException("Something went wrong here");
            createdValue = JsonConvert.DeserializeObject<T>(content);
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
        public bool Exists(string key)
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