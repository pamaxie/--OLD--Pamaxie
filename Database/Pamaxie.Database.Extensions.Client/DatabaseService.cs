using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Pamaxie.Database.Design;
using Pamaxie.Jwt;

namespace Pamaxie.Database.Extensions.Client
{
    /// <inheritdoc/>
    public sealed class DatabaseService : IDatabaseService<HttpClient, PamaxieDataContext>
    {
        /// <inheritdoc/>
        public HttpClient Service { get; internal init; } = new HttpClient();

        /// <inheritdoc/>
        public PamaxieDataContext DataContext { get; }

        /// <inheritdoc/>
        public bool IsConnected { get; set; }

        /// <inheritdoc/>
        public DateTime LastConnectionSuccess { get; set; }

        /// <summary>
        /// Contains the service responsible for interacting with user data for the Api
        /// </summary>
        internal static IUserDataService UserService { get; set; }

        /// <summary>
        /// Contains the Service responsible for interacting with application data for the Api
        /// </summary>
        internal static IApplicationDataService ApplicationService { get; set; }

        /// <summary>
        /// Credentials used for authenticating with the API the first time
        /// </summary>
        private NetworkCredential _credentials;

        public DatabaseService(PamaxieDataContext dataContext, NetworkCredential credential)
        {
            DataContext = dataContext;
            UserService = new UserDataService(dataContext, this);
            ApplicationService = new ApplicationDataService(dataContext, this);
            _credentials = credential;
        }

        /// <inheritdoc/>
        public bool Connect()
        {
            if (_credentials == null)
            {
                throw new AuthenticationException("Connecting to the API requires a username and password for authentication");
            }

            if (string.IsNullOrEmpty(_credentials.UserName) || string.IsNullOrEmpty(_credentials.Password))
            {
                throw new AuthenticationException("Connecting to the API requires a username and password for authentication");
            }

            //The way User Data is stored is that if a user is created their KEY equals a one way SHA256 hash we compute.
            //Without this one way hash we do not know the user and it will be impossible to figure out except for using security questions to re-retrieve the id.
            var data = JsonConvert.SerializeObject(_credentials);
            var binaryData = Encoding.UTF32.GetBytes(data);

            if (binaryData.Length == 0)
            {
                return false;
            }

            SHA256 shaManager = new SHA256Managed();
            var result = shaManager.ComputeHash(binaryData);
            var userId = Convert.ToBase64String(result);
            var username = UserService.Get(userId);

            HttpRequestMessage requestMessage = DataContext.PostRequestMessage($"/authenticate/login={username}");
            HttpResponseMessage response = Service.Send(requestMessage);

            if (!response.IsSuccessStatusCode)
            {
                throw new WebException(response.StatusCode.ToString());
            }

            var token = response.ReadJsonResponse<AuthToken>();
            _credentials = null;
            return true;
        }

        /// <inheritdoc/>
        public bool Disconnect()
        {
            IsConnected = false;
            DataContext.Disconnect();
            return false;
        }

        /// <inheritdoc/>
        public bool IsServiceAvailable()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public ushort ServiceLatency()
        {
            throw new NotImplementedException();
        }
    }
}