using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Authentication;

namespace Test.Base
{
    /// <summary>
    /// Class for handling configuration interactions
    /// </summary>
    public static class ConfigurationService
    {
        /// <summary>
        /// Generates the configuration that will be used for testing
        /// </summary>
        /// <returns>Generated <see cref="IConfiguration"/></returns>
        internal static IConfiguration GenerateConfiguration()
        {
            dynamic obj = new JObject();

            //AuthData
            dynamic objAuthData = new JObject();
            objAuthData.Secret = JwtTokenGenerator.GenerateSecret();
            objAuthData.ExpiresInMinutes = 15;
            obj.AuthData = objAuthData;

            //ApiData
            dynamic objApiData = new JObject();
            objApiData.Instance = "http://localhost/";
            obj.ApiData = objApiData;

            //RedisData
            dynamic objRedisData = new JObject();
            objRedisData.ConnectionString = "";
            obj.RedisData = objRedisData;

            //UserData
            dynamic objUserData = new JObject();
            objUserData.EmailAddress = "";
            obj.UserData = objUserData;

            //EmailSender
            dynamic objEmailSender = new JObject();
            objEmailSender.EmailAddress = "";
            objEmailSender.Password = "";
            obj.EmailSender = objEmailSender;

            //JwtToken
            dynamic objJwtToken = new JObject();
            objJwtToken.Secret = JwtTokenGenerator.GenerateSecret();
            obj.JwtToken = objJwtToken;

            string jsonString = JsonConvert.SerializeObject(obj, Formatting.Indented);
            return new ConfigurationBuilder().AddJsonStream(new MemoryStream(Encoding.ASCII.GetBytes(jsonString)))
                .Build();
        }
    }
}