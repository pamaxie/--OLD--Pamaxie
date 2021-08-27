using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Pamaxie.Api;
using Pamaxie.Api.Controllers;
using Pamaxie.Database.Extensions.Server;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Database.Api.HttpClientControllerCall
{
    /// <summary>
    /// Testing class for <see cref="ApplicationController"/>
    /// </summary>
    public class ApplicationControllerTest : Base.Test, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly PamaxieDataContext _context;
        private readonly HttpClient _client;
        
        /// <summary>
        /// <inheritdoc cref="MemberData.AllApplications"/>
        /// </summary>
        public static IEnumerable<object[]> AllApplications => MemberData.AllApplications;
        
        public ApplicationControllerTest(ITestOutputHelper testOutputHelper, WebApplicationFactory<Startup> fixture) : base(testOutputHelper)
        {
            _context = new PamaxieDataContext("", ""); //TODO get instance and password from Configuration
            _client = fixture.CreateClient();
        }

        /// <summary>
        /// Test for getting a application through <see cref="ApplicationController.GetTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Get(string applicationKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for creating a application <see cref="ApplicationController.CreateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Create(string applicationKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for updating a application through <see cref="ApplicationController.UpdateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Update(string applicationKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for creating a application through <see cref="ApplicationController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void UpdateOrCreate_Create(string applicationKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for updating a application through <see cref="ApplicationController.UpdateOrCreateTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void UpdateOrCreate_Update(string applicationKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for deleting a application through <see cref="ApplicationController.DeleteTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void Delete(string applicationKey)
        {
            //TODO Not yet implemented
        }
        
        /// <summary>
        /// Test for getting all applications from a application through <see cref="ApplicationController.EnableOrDisableTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void EnableOrDisable(string applicationKey)
        {
            //TODO Not yet implemented
        }

        /// <summary>
        /// Test for verify a application through <see cref="ApplicationController.VerifyAuthenticationTask"/>
        /// </summary>
        /// <param name="applicationKey">The application key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllApplications))]
        public void VerifyAuthentication(string applicationKey)
        {
            //TODO Not yet implemented
        }
    }
}