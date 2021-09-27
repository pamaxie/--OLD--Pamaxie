using System;
using System.Net.Http.Headers;
using Pamaxie.Jwt;

namespace Pamaxie.Database.Extensions.Client
{
    /// <summary>
    /// DataContext class used for the connection information to the database api and the Redis database
    /// </summary>
    public sealed class PamaxieDataContext
    {
        /// <summary>
        /// Defines the url of the api that will be 
        /// </summary>
        public string DataInstances { get; }

        /// <summary>
        /// Defines the password to use for the instances
        /// </summary>
        public AuthToken Token { get; }

        /// <summary>
        /// Connection properties to a api
        /// </summary>
        /// <param name="instance">The url of the api</param>
        /// <param name="token">Token to use for the api authentication</param>
        public PamaxieDataContext(string instance, AuthToken token)
        {
            DataInstances = instance;
            Token = token;
        }

        /// <summary>
        /// Gets the <see cref="AuthenticationHeaderValue"/> used for the HttpClient.DefaultRequestHeaders.Authorization
        /// </summary>
        /// <returns>A <see cref="AuthenticationHeaderValue"/> with a Bearer scheme authentication</returns>
        public AuthenticationHeaderValue GetAuthenticationRequestHeader()
        {
            if (Token == null)
                throw new InvalidOperationException("Please make sure that the token are initialized before calling this method");

            return new AuthenticationHeaderValue("Bearer", Token.Token);
        }
    }
}