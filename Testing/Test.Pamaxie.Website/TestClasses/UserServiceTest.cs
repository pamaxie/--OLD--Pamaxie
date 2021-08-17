using System;
using Microsoft.AspNetCore.Http;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Sql.Data;
using Pamaxie.Database.Sql;
using Pamaxie.Extensions.Sql;
using Pamaxie.Website.Services;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Testing class for UserService
    /// </summary>
    public class UserServiceTest : Test
    {
        public UserServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Fact]
        public void IsEmailOfCurrentUserVerified_Success()
        {
            //Mock Database
            SqlDbContext sqlDbContext = MockSqlDbContext.Mock(Configuration);
            UserExtensions.DbContext = sqlDbContext;
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(Configuration);

            UserService userService = new(Configuration, httpContextAccessor, null);
            Assert.True(userService.IsEmailOfCurrentUserVerified());
        }

        [Fact]
        public void GenerateEmailConfirmationToken_Success()
        {
            ProfileData data = TestProfileData.Profile;
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(Configuration);

            UserService userService = new(Configuration, httpContextAccessor, null);
            string token = userService.GenerateEmailConfirmationToken(data);
            TestOutputHelper.WriteLine(token);
            Assert.NotEmpty(token);
        }
        
        [Fact]
        public void ConfirmEmail_Success()
        {
            ProfileData data = TestProfileData.Profile;
            
            //Mock Database
            SqlDbContext sqlDbContext = MockSqlDbContext.Mock(Configuration);
            UserExtensions.DbContext = sqlDbContext;
            
            User unverifiedUser = UserExtensions.GetUser(data.GoogleClaimUserId);
            if (unverifiedUser.EmailVerified) //Make sure the user you are testing is not verified
                throw new Exception("Test user is already verified, change your test data");
            TestOutputHelper.WriteLine(unverifiedUser.EmailVerified.ToString());
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(Configuration);

            UserService userService = new(Configuration, httpContextAccessor, null);
            string token = userService.GenerateEmailConfirmationToken(data);
            Assert.True(userService.ConfirmEmail(token));
            
            User verifiedUser = UserExtensions.GetUser(data.GoogleClaimUserId);
            TestOutputHelper.WriteLine(verifiedUser.EmailVerified.ToString());
            Assert.True(userService.IsEmailOfCurrentUserVerified());
        }
    }
}