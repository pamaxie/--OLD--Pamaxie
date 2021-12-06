using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pamaxie.Data;
using Pamaxie.Website.Models;
using Pamaxie.Website.Services;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website_Test
{
    /// <summary>
    /// Testing class for <see cref="JsonWebToken"/>
    /// </summary>
    public sealed class JsonWebTokenTest : TestBase
    {
        private readonly string _secret;

        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;

        public JsonWebTokenTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            IConfigurationSection configurationSection = Configuration.GetSection("JwtToken");
            _secret = configurationSection.GetValue<string>("Secret");
        }

        /// <summary>
        /// Testing for encoding IBody to token string.
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Encode(PamaxieUser user)
        {
            //Act
            ConfirmEmailBody body = new ConfirmEmailBody(user);
            string token = JsonWebToken.Encode(body, _secret);

            //Assert
            Assert.False(string.IsNullOrEmpty(token));
            TestOutputHelper.WriteLine(token);
        }

        /// <summary>
        /// Testing to check if the token is valid with a newly created token
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Decode_Success(PamaxieUser user)
        {
            //Arrange
            ConfirmEmailBody body = new ConfirmEmailBody(user);
            string token = JsonWebToken.Encode(body, _secret);

            //Act
            ConfirmEmailBody decodedBody = JsonWebToken.Decode<ConfirmEmailBody>(token, _secret) as ConfirmEmailBody;

            //Assert
            Assert.NotNull(decodedBody);
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(decodedBody));
        }

        /// <summary>
        /// Testing to check if the token is invalid
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Decode_Failure_Invalid(PamaxieUser user)
        {
            //Arrange
            ConfirmEmailBody body = new ConfirmEmailBody(user);
            string token = JsonWebToken.Encode(body, _secret);

            //Act
            token = token.Replace('s', 'd'); //Make the token invalid
            ConfirmEmailBody decodedBody = JsonWebToken.Decode<ConfirmEmailBody>(token, _secret) as ConfirmEmailBody;

            //Assert
            Assert.Null(decodedBody);
        }

        /// <summary>
        /// Testing to check if the token is invalid when expired.
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Decode_Failure_Expired(PamaxieUser user)
        {
            //Arrange
            ConfirmEmailBody body = new ConfirmEmailBody(user)
            {
                //Expire the token
                Expiration = DateTime.UtcNow.Subtract(TimeSpan.FromDays(10))
            };
            string token = JsonWebToken.Encode(body, _secret);

            //Act
            ConfirmEmailBody decodedBody = JsonWebToken.Decode<ConfirmEmailBody>(token, _secret) as ConfirmEmailBody;

            //Assert
            Assert.Null(decodedBody);
        }
    }
}