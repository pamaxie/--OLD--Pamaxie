using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Pamaxie.Api;
using Pamaxie.Api.Controllers;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Server;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Api.HttpClientControllerCall
{
    /// <summary>
    /// Testing class for <see cref="UserController"/>
    /// </summary>
    public class UserControllerTest : Base.Test, IClassFixture<ApiWebApplicationFactory<Startup>>
    {
        private readonly PamaxieDataContext _context;
        private readonly HttpClient _client;
        
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUsers => MemberData.AllUsers;
        
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUnverifiedUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers => MemberData.AllUnverifiedUsers;

        public UserControllerTest(ITestOutputHelper testOutputHelper, ApiWebApplicationFactory<Startup> fixture) : base(
            testOutputHelper)
        {
            _context = new PamaxieDataContext("", ""); //TODO get instance and password from Configuration

            _client = fixture.CreateClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                JwtBearerDefaults.AuthenticationScheme,
                Convert.ToBase64String(
                    Encoding.ASCII.GetBytes(Configuration.GetSection("AuthData").GetValue<string>("Secret"))));
        }

        /// <summary>
        /// Test for getting a user through <see cref="UserController.GetTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public async Task Get(string userKey)
        {
            //TODO Still in progress
            HttpRequestMessage response = new(HttpMethod.Get, "User/get");
 
            byte[] bodyBytes = Encoding.ASCII.GetBytes(userKey);
            response.Content = new ByteArrayContent(bodyBytes);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage responseMessage = _client.SendAsync(response).Result;
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
            IPamaxieUser user = JsonConvert.DeserializeObject<IPamaxieUser>(await response.Content.ReadAsStringAsync());
            Assert.NotNull(user);
        }
        
        /// <summary>
        /// Test for creating a user <see cref="UserController.CreateTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Create(string userKey)
        {
            //TODO Still in progress
            PamaxieDataContext context = new PamaxieDataContext("", "");

            //Get application
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);

            HttpRequestMessage response = new(HttpMethod.Post, context.DataInstances);

            string body = JsonConvert.SerializeObject(user);
            byte[] bodyBytes = Encoding.ASCII.GetBytes(body);
            response.Content = new ByteArrayContent(bodyBytes);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            using HttpResponseMessage responseMessage = _client.SendAsync(response).Result;
            Assert.Equal(HttpStatusCode.OK, responseMessage.StatusCode);
        }
        
        /// <summary>
        /// Test for updating a user through <see cref="UserController.UpdateTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Update(string userKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for creating a user through <see cref="UserController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void UpdateOrCreate_Create(string userKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for updating a user through <see cref="UserController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void UpdateOrCreate_Update(string userKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for deleting a user through <see cref="UserController.DeleteTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void Delete(string userKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for getting all applications from a user through <see cref="UserController.GetAllApplicationsTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUsers))]
        public void GetAllApplications(string userKey)
        {
            //TODO Not yet implemented
        }

        /// <summary>
        /// Test for verify a user through <see cref="UserController.VerifyEmailTask"/>
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void VerifyEmail(string userKey)
        {
            //TODO Not yet implemented
        }
    }
}