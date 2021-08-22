using System;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pamaxie.Website.Models;
using Pamaxie.Website.Services;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Testing class for JsonWebToken
    /// </summary>
    public class JsonWebTokenTest : Base.Test
    {
        private readonly string _secret;

        public JsonWebTokenTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            IConfigurationSection configurationSection = Configuration.GetSection("JwtToken");
            _secret = configurationSection.GetValue<string>("Secret");
        }

        /// <summary>
        /// Testing for encoding IBody to token string.
        /// </summary>
        [Fact]
        public void Encode_Success()
        {
            ProfileData data = TestProfileData.Profile;
            ConfirmEmailBody body = new(data);
            string token = JsonWebToken.Encode(body, _secret);
            TestOutputHelper.WriteLine(token);
        }
        
        /// <summary>
        /// Testing to check if the token is valid.
        /// </summary>
        [Fact]
        public void Decode_Success()
        {
            ProfileData data = TestProfileData.Profile;
            //Newly created token
            ConfirmEmailBody body = new(data);
            string token = JsonWebToken.Encode(body, _secret);
            ConfirmEmailBody decodedBody = JsonWebToken.Decode<ConfirmEmailBody>(token, _secret) as ConfirmEmailBody;
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(decodedBody));
            Assert.NotNull(decodedBody);
        }
        
        /// <summary>
        /// Testing to check if the token is invalid if changed.
        /// </summary>
        [Fact]
        public void Decode_Failure_Invalid()
        {
            ProfileData data = TestProfileData.Profile;
            ConfirmEmailBody body = new(data);
            string token = JsonWebToken.Encode(body, _secret);
            //Make the token invalid
            token = token.Replace('s', 'd');
            ConfirmEmailBody decodedBody = JsonWebToken.Decode<ConfirmEmailBody>(token, _secret) as ConfirmEmailBody;
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(decodedBody));
            Assert.Null(decodedBody);
        }
        
        /// <summary>
        /// Testing to check if the token is invalid when expired.
        /// </summary>
        [Fact]
        public void Decode_Failure_Expired()
        {
            ProfileData data = TestProfileData.Profile;
            ConfirmEmailBody body = new(data)
            {
                //Expire the token
                Expiration = DateTime.UtcNow.Subtract(TimeSpan.FromDays(10))
            };
            string token = JsonWebToken.Encode(body, _secret);
            ConfirmEmailBody decodedBody = JsonWebToken.Decode<ConfirmEmailBody>(token, _secret) as ConfirmEmailBody;
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(decodedBody));
            Assert.Null(decodedBody);
        }
    }
}