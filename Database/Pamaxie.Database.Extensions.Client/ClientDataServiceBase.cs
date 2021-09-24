using System;
using System.Net;
using System.Net.Http;
using Pamaxie.Data;
using Pamaxie.Database.Design;
using Pamaxie.Database.Extensions.Client.Extensions;

// ReSharper disable SwitchStatementMissingSomeEnumCasesNoDefault

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// Database Api integrations
    /// </summary>
    /// <typeparam name="T"><inheritdoc cref="IDataServiceBase{T}"/></typeparam>
    internal class ClientDataServiceBase<T> : IDataServiceBase<T> where T : IDatabaseObject
    {
        /// <summary>
        /// Data Context responsible for connecting to Pamaxie
        /// </summary>
        internal PamaxieDataContext DataContext { get; init; }

        /// <summary>
        /// The Service that should be used to connect to the database
        /// </summary>
        internal DatabaseService Service { get; init; }

        /// <summary>
        /// The base url of the Api
        /// </summary>
        internal string Url { get; init; }

        /// <inheritdoc/>
        public T Get(string key)
        {
            HttpRequestMessage requestMessage = DataContext.GetRequestMessage(new Uri(Url + "/Get=" + key));
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            return response.ReadJsonResponse<T>();
        }

        /// <inheritdoc/>
        public T Create(T value)
        {
            HttpRequestMessage requestMessage = DataContext.PostRequestMessage(new Uri(Url + "/Create"), value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            return response.ReadJsonResponse<T>();
        }

        /// <inheritdoc/>
        public bool TryCreate(T value, out T createdValue)
        {
            HttpRequestMessage requestMessage = DataContext.PostRequestMessage(new Uri(Url + "/TryCreate"), value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            createdValue = response.ReadJsonResponse<T>();
            return true;
        }

        /// <inheritdoc/>
        public T Update(T value)
        {
            HttpRequestMessage requestMessage = DataContext.PutRequestMessage(new Uri(Url + "/Update"), value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            return response.ReadJsonResponse<T>();
        }

        /// <inheritdoc/>
        public bool TryUpdate(T value, out T updatedValue)
        {
            HttpRequestMessage requestMessage = DataContext.PutRequestMessage(new Uri(Url + "/TryUpdate"), value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            updatedValue = response.ReadJsonResponse<T>();
            return true;
        }

        /// <inheritdoc/>
        public bool UpdateOrCreate(T value, out T updatedOrCreatedValue)
        {
            HttpRequestMessage requestMessage =
                DataContext.PostRequestMessage(new Uri(Url + "/UpdateOrCreate"), value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            updatedOrCreatedValue = response.ReadJsonResponse<T>();

            if (response.StatusCode == HttpStatusCode.Created)
            {
                return true;
            }

            return false;
        }

        /// <inheritdoc/>
        public bool Exists(string key)
        {
            HttpRequestMessage requestMessage = DataContext.GetRequestMessage(new Uri(Url + "/Exists=" + key));
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            bool result = response.ReadJsonResponse<bool>();
            return result;
        }

        /// <inheritdoc/>
        public bool Delete(T value)
        {
            HttpRequestMessage requestMessage = DataContext.DeleteRequestMessage(new Uri(Url + "/Delete"), value);
            HttpResponseMessage response = Service.SendRequestMessage(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            bool result = response.ReadJsonResponse<bool>();
            return result;
        }
    }
}