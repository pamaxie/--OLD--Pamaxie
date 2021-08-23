using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Website.Models;

namespace Pamaxie.Website.Services
{
    /// <summary>
    /// Custom JsonWebToken service for our website
    /// Used for email links
    /// </summary>
    public static class JsonWebToken
    {
        private static readonly Func<byte[], byte[], byte[]> Rs256 = (key, value) =>
        {
            using HMACSHA256 sha = new(key);
            return sha.ComputeHash(value);
        };
        
        /// <summary>
        /// Encodes a IBody into a base64Url string through Rs256 encryption.
        /// </summary>
        /// <param name="body">Email body</param>
        /// <param name="secret">JWT Secret</param>
        /// <returns>Encoded JWT Token</returns>
        public static string Encode(IBody body, string secret)
        {
            return Base64UrlEncode(Encoding.UTF8.GetBytes(Encode(body, Encoding.UTF8.GetBytes(secret))));
        }

        private static string Encode(IBody body, byte[] secretBytes)
        {
            List<string> segments = new();
            var header = new { name = "Pamaxie", typ = "Jwt" };

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            byte[] bodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body, Formatting.None));
            
            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(bodyBytes));

            byte[] bytesToSign = Encoding.UTF8.GetBytes(string.Join(".", segments.ToArray()));
            byte[] signature = Rs256(secretBytes, bytesToSign);
            
            segments.Add(Base64UrlEncode(signature));
            return string.Join(".", segments.ToArray());
        }

        /// <summary>
        /// Decodes a Base64Url string into a IBody using the same secret as encoded with.
        /// </summary>
        /// <param name="token">JWT Token</param>
        /// <param name="secret">JWT Secret</param>
        /// <typeparam name="T">Class inheriting <see cref="IBody"/></typeparam>
        /// <returns><see cref="IBody"/> of T</returns>
        public static IBody? Decode<T>(string token, string secret)
        {
            return Decode<T>(Encoding.UTF8.GetString(Base64UrlDecode(token)), Encoding.UTF8.GetBytes(secret), true);
        }

        private static IBody? Decode<T>(string token, byte[] secretBytes, bool verify)
        {
            try
            {
                string[] segments = token.Split('.');
                string header = segments[0];
                string body = segments[1];
                byte[] signature = Base64UrlDecode(segments[2]);

                string headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
                JObject headerData = JObject.Parse(headerJson);
                string bodyJson = Encoding.UTF8.GetString(Base64UrlDecode(body));
                IBody? bodyObject = JsonConvert.DeserializeObject<T>(bodyJson) as IBody;

                if (!verify)
                    return bodyObject;
                if (DateTime.UtcNow > bodyObject?.Expiration)
                    return default;
                
                byte[] bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", body));
                string name = (string)headerData["name"]!;
                string type = (string)headerData["type"]!;

                if (name is not "Pamaxie" && type is not "Jwt") return default;
                
                byte[] compareSignature = Rs256(secretBytes, bytesToSign);
                string decodedSignature = Base64UrlEncode(signature);
                string decodedCompareSignature = Base64UrlEncode(compareSignature);

                return !Equals(decodedSignature, decodedCompareSignature) ? default : bodyObject;
            }
            catch
            {
                return default;
            }
        }

        private static string Base64UrlEncode(byte[] input)
        {
            string output = Convert.ToBase64String(input);
            output = output.Split('=')[0];
            output = output.Replace('+', '-');
            output = output.Replace('/', '_');
            return output;
        }

        private static byte[] Base64UrlDecode(string input)
        {
            string output = input;
            output = output.Replace('-', '+');
            output = output.Replace('_', '/');
            switch (output.Length % 4)
            {
                case 0: break;
                case 2:
                    output += "==";
                    break;
                case 3:
                    output += "=";
                    break;
                default:
                    return Array.Empty<byte>();
            }

            byte[] converted = Convert.FromBase64String(output);
            return converted;
        }
    }
}