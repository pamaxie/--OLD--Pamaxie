using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.Client
{
    internal interface IRestClient
    {
        /// <summary>
        /// API endpoint / Website for the database api
        /// </summary>
        public Uri DbClientEndpoint { get; }

        /// <summary>
        /// API endpoint / Website for the ml api
        /// </summary>
        public Uri MlClientEndpoint { get; }

        /// <summary>
        /// API endpoint / Website for the website
        /// </summary>
        public Uri WebClientEndpoint { get; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        IDatabaseClient DbClient { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        IMachineLearningClient MlClient { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        IWebsiteClient WebClient { get; set; }

        /// <summary>
        /// Gets a user token and logs them in
        /// </summary>
        /// <returns>
        /// <see cref="string"/> with the users Jwt bearer token
        /// </returns>
        public string GetToken(NetworkCredential credentials);

        /// <summary>
        /// Refreshes a given user token
        /// </summary>
        /// <returns>
        /// <see cref="string"/> with the users Jwt bearer token
        /// </returns>
        /// <exception cref="WebException">
        /// If server cannot be reached or the user is not allowed to access this resource
        /// TODO: Make custom exceptions for this.
        /// </exception>
        public string RefreshToken(string token);
    }
}
