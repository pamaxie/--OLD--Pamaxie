using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Website.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Pamaxie.Website.Services
{
    public static class JsonWebToken
    {
        private static readonly Func<byte[], byte[], byte[]> Rs256 = (key, value) =>
        {
            using HMACSHA256 sha = new(key);
            return sha.ComputeHash(value);
        };
        
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

            string stringToSign = string.Join(".", segments.ToArray());

            byte[] bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] signature = Rs256(secretBytes, bytesToSign);
            
            segments.Add(Base64UrlEncode(signature));
            return string.Join(".", segments.ToArray());
        }

        public static IBody? Decode<T>(string token, string secret)
        {
            return Decode<T>(Encoding.UTF8.GetString(Base64UrlDecode(token)), secret, true);
        }

        private static IBody? Decode<T>(string token, string secret, bool verify)
        {
            try
            {
                string[] parts = token.Split('.');
                string header = parts[0];
                string body = parts[1];
                byte[] crypto = Base64UrlDecode(parts[2]);

                string headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
                JObject headerData = JObject.Parse(headerJson);
                string bodyJson = Encoding.UTF8.GetString(Base64UrlDecode(body));
                IBody? bodyObject = JsonConvert.DeserializeObject<T>(bodyJson) as IBody;

                if (!verify)
                    return bodyObject;
                if (DateTime.UtcNow > bodyObject?.Expiration)
                    return default;
                
                byte[] bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", body));
                byte[] secretBytes = Encoding.UTF8.GetBytes(secret);
                string name = (string)headerData["name"]!;
                string type = (string)headerData["type"]!;

                if (name is not "Pamaxie" && type is not "Jwt") return default;
                
                byte[] signature = Rs256(secretBytes, bytesToSign);
                string decodedCrypto = Base64UrlEncode(crypto);
                string decodedSignature = Base64UrlEncode(signature);

                return decodedCrypto != decodedSignature ? default : bodyObject;
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