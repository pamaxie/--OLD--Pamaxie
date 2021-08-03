using System;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using ProfileData = Pamaxie.Database.Extensions.Sql.Data.ProfileData;
using JsonWebToken = Pamaxie.Website.Services.JsonWebToken;
using ConfirmEmailBody = Pamaxie.Website.Models.ConfirmEmailBody;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Testing class for JsonWebToken
    /// </summary>
    public class JsonWebTokenTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public JsonWebTokenTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private const string Secret = "Pamaxie";

        private static ProfileData Data { get; } = new()
        {
            Id = 0,
            GoogleClaimUserId = "123123123",
            EmailAddress = "Hello@Hello.com",
            UserName = "Hello",
            Deleted = false,
            ProfilePictureAddress = ""
        };

        [Fact]
        public void Encode_Success()
        {
            ConfirmEmailBody body = new(Data);
            string token = JsonWebToken.Encode(body, Secret);
            _testOutputHelper.WriteLine(token);
        }

        [Fact]
        public void Decode_Success()
        {
            //Newly created token
            ConfirmEmailBody body = new(Data);
            string token = JsonWebToken.Encode(body, Secret);
            ConfirmEmailBody decodedBody = JsonWebToken.Decode<ConfirmEmailBody>(token, Secret) as ConfirmEmailBody;
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(decodedBody));
            Assert.NotNull(decodedBody);
        }
        
        [Fact]
        public void Decode_Failure()
        {
            ConfirmEmailBody body = new(Data)
            {
                //Expire the token
                Expiration = DateTime.UtcNow.Subtract(TimeSpan.FromDays(10))
            };
            string token = JsonWebToken.Encode(body, Secret);
            ConfirmEmailBody decodedBody = JsonWebToken.Decode<ConfirmEmailBody>(token, Secret) as ConfirmEmailBody;
            _testOutputHelper.WriteLine(JsonConvert.SerializeObject(decodedBody));
            Assert.Null(decodedBody);
        }
    }
}