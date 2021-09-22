using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// This class handles extensions relating to the client connecting to the api
    /// </summary>
    internal static class WebExtensions
    {
        /// <summary>
        /// Creates a HTTP Request via an endpoint url
        /// </summary>
        /// <param name="uri">URI for the request to be send to</param>
        /// <param name="value">Optional value to send to the api</param>
        /// <param name="baseMessage">This is the original request (if deviance from standard procedure gets are required !!if a uri is specified the original uri is overwritten!!)</param>
        /// <returns><see cref="HttpRequestMessage"/> for the request</returns>
        public static HttpRequestMessage PostRequestMessage(Uri uri, object value = null,
            HttpRequestMessage baseMessage = null)
            => GetMessage(uri, HttpMethod.Post, value, baseMessage);

        /// <summary>
        /// Creates a HTTP Request via an endpoint url
        /// </summary>
        /// <param name="uri">URI for the request to be send to</param>
        /// <param name="value">Optional value to send to the api</param>
        /// <param name="baseMessage">This is the original request (if deviance from standard procedure gets are required !!if a uri is specified the original uri is overwritten!!)</param>
        /// <returns><see cref="HttpRequestMessage"/> for the request</returns>
        public static HttpRequestMessage PutRequestMessage(Uri uri, object value = null,
            HttpRequestMessage baseMessage = null)
            => GetMessage(uri, HttpMethod.Put, value, baseMessage);

        /// <summary>
        /// Creates a HTTP Request via an endpoint url
        /// </summary>
        /// <param name="uri">URI for the request to be send to</param>
        /// <param name="value">Optional value to send to the api</param>
        /// <param name="baseMessage">This is the original request (if deviance from standard procedure gets are required !!if a uri is specified the original uri is overwritten!!)</param>
        /// <returns><see cref="HttpRequestMessage"/> for the request</returns>
        public static HttpRequestMessage GetRequestMessage(Uri uri, object value = null,
            HttpRequestMessage baseMessage = null)
            => GetMessage(uri, HttpMethod.Get, value, baseMessage);

        /// <summary>
        /// Creates a HTTP Request via an endpoint url
        /// </summary>
        /// <param name="uri">URI for the request to be send to</param>
        /// <param name="value">Optional value to send to the api</param>
        /// <param name="baseMessage">This is the original request (if deviance from standard procedure gets are required !!if a uri is specified the original uri is overwritten!!)</param>
        /// <returns><see cref="HttpRequestMessage"/> for the request</returns>
        public static HttpRequestMessage DeleteRequestMessage(Uri uri, object value = null,
            HttpRequestMessage baseMessage = null)
            => GetMessage(uri, HttpMethod.Delete, value, baseMessage);

        /// <summary>
        /// Creates a new Request with the defined method type. This is a very generalized implementation of requests.
        /// </summary>
        /// <param name="uri">The endpoint url this request is targeting</param>
        /// <param name="messageMethod">The method that should be used for the request</param>
        /// <param name="value">The value that should be written to the http message, depending on the type of message setting this value makes more or less sense</param>
        /// <param name="baseMessage">Overwrites the internally created HttpRequestMessage to allow for customizations in the message for specific method types (Put, set, get, ect...)</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static HttpRequestMessage GetMessage(Uri uri, HttpMethod messageMethod, object value,
            HttpRequestMessage baseMessage)
        {
            HttpRequestMessage requestMessage = null;
            if (baseMessage != null)
            {
                requestMessage = baseMessage;
            }

            if (uri != null)
                requestMessage = new HttpRequestMessage(messageMethod, uri);

            if (requestMessage == null && baseMessage == null)
                throw new ArgumentException("Please make sure to specify a uri before calling this method.");

            if (value != null)
            {
                string body = JsonConvert.SerializeObject(value);
                byte[] bodyBytes = Encoding.ASCII.GetBytes(body);
                requestMessage.Content = new ByteArrayContent(bodyBytes);
                requestMessage.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            return requestMessage;
        }

        /// <summary>
        /// Read out the requests response if it is Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        public static T ReadJsonResponse<T>(this HttpResponseMessage response)
        {
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
    }
}