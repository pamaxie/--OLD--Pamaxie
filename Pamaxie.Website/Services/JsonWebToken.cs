using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pamaxie.Website.Models;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Pamaxie.Website.Services
{
    public enum JwtHashAlgorithm
    {
        RS256,
        HS384,
        HS512
    }

    public static class JsonWebToken
    {
        private static readonly Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>> HashAlgorithms;

        static JsonWebToken()
        {
            HashAlgorithms = new Dictionary<JwtHashAlgorithm, Func<byte[], byte[], byte[]>>
            {
                {
                    JwtHashAlgorithm.RS256, (key, value) =>
                    {
                        using HMACSHA256 sha = new(key);
                        return sha.ComputeHash(value);
                    }
                },
                {
                    JwtHashAlgorithm.HS384, (key, value) =>
                    {
                        using HMACSHA384 sha = new(key);
                        return sha.ComputeHash(value);
                    }
                },
                {
                    JwtHashAlgorithm.HS512, (key, value) =>
                    {
                        using HMACSHA512 sha = new(key);
                        return sha.ComputeHash(value);
                    }
                }
            };
        }
            
        public static string Encode(IBody body, string secret, JwtHashAlgorithm algorithm)
        {
            return Encode(body, Encoding.UTF8.GetBytes(secret), algorithm);
        }

        private static string Encode(IBody body, byte[] secretBytes, JwtHashAlgorithm algorithm)
        {
            List<string> segments = new();
            var header = new { alg = algorithm.ToString(), typ = "Jwt" };

            byte[] headerBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header, Formatting.None));
            byte[] bodyBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(body, Formatting.None));
            
            segments.Add(Base64UrlEncode(headerBytes));
            segments.Add(Base64UrlEncode(bodyBytes));

            string stringToSign = string.Join(".", segments.ToArray());

            byte[] bytesToSign = Encoding.UTF8.GetBytes(stringToSign);

            byte[] signature = HashAlgorithms[algorithm](secretBytes, bytesToSign);
            segments.Add(Base64UrlEncode(signature));

            return string.Join(".", segments.ToArray());
        }

        public static IBody? Decode<T>(string token, string secret)
        {
            return Decode<T>(token, secret, true);
        }

        private static IBody? Decode<T>(string token, string secret, bool verify)
        {
            string[] parts = token.Split('.');
            string header = parts[0];
            string body = parts[1];
            byte[] crypto = Base64UrlDecode(parts[2]);

            string headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            JObject headerData = JObject.Parse(headerJson);
            string bodyJson = Encoding.UTF8.GetString(Base64UrlDecode(body));
            IBody? bodyObject = JsonConvert.DeserializeObject<T>(bodyJson) as IBody;

            if (!verify) return bodyObject;
            
            byte[] bytesToSign = Encoding.UTF8.GetBytes(string.Concat(header, ".", body));
            byte[] secretBytes = Encoding.UTF8.GetBytes(secret);
            string algorithm = (string)headerData["alg"]!;

            byte[] signature = HashAlgorithms[GetHashAlgorithm(algorithm)](secretBytes, bytesToSign);
            string decodedCrypto = Convert.ToBase64String(crypto);
            string decodedSignature = Convert.ToBase64String(signature);

            return decodedCrypto != decodedSignature ? default : bodyObject;
        }

        private static JwtHashAlgorithm GetHashAlgorithm(string algorithm)
        {
            return algorithm switch
            {
                "RS256" => JwtHashAlgorithm.RS256,
                "HS384" => JwtHashAlgorithm.HS384,
                "HS512" => JwtHashAlgorithm.HS512,
                _ => throw new InvalidOperationException("Algorithm not supported.")
            };
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