﻿using System;
using System.Collections.Generic;
using System.Linq;
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
    public sealed class JsonWebTokenTestBaseTest : TestBase
    {
        private readonly string _secret;

        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;

        public JsonWebTokenTestBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            IConfigurationSection configurationSection = Configuration.GetSection("JwtToken");
            _secret = configurationSection.GetValue<string>("Secret");
        }

        /// <summary>
        /// Testing for encoding IBody to token string.
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Encode_Success(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            ConfirmEmailBody body = new ConfirmEmailBody(user);
            string token = JsonWebToken.Encode(body, _secret);
            TestOutputHelper.WriteLine(token);
        }

        /// <summary>
        /// Testing to check if the token is valid.
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Decode_Success(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            //Newly created token
            ConfirmEmailBody body = new ConfirmEmailBody(user);
            string token = JsonWebToken.Encode(body, _secret);
            ConfirmEmailBody decodedBody = JsonWebToken.Decode<ConfirmEmailBody>(token, _secret) as ConfirmEmailBody;
            TestOutputHelper.WriteLine(JsonConvert.SerializeObject(decodedBody));
            Assert.NotNull(decodedBody);
        }

        /// <summary>
        /// Testing to check if the token is invalid if changed.
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Decode_Failure_Invalid(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            ConfirmEmailBody body = new ConfirmEmailBody(user);
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
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Decode_Failure_Expired(string userKey)
        {
            PamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            ConfirmEmailBody body = new ConfirmEmailBody(user)
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