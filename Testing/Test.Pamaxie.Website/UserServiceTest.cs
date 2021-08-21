using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Sql.Data;
using Pamaxie.Database.Sql;
using Pamaxie.Extensions.Sql;
using Pamaxie.Website.Authentication;
using Pamaxie.Website.Services;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Testing class for UserService
    /// </summary>
    public class UserServiceTest : Base.Test
    {
        public static IEnumerable<object[]> AllVerifiedUsers => MemberData.AllVerifiedGoogleClaimUsers;
        public static IEnumerable<object[]> AllUnverifiedUsers => MemberData.AllUnverifiedGoogleClaimUsers;
        
        public UserServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        [Theory]
        [MemberData(nameof(AllVerifiedUsers))]
        public void IsEmailOfCurrentUserVerified_Success(string googleUserId)
        {
            //Mock Database
            SqlDbContext sqlDbContext = MockSqlDbContext.Mock();
            UserExtensions.DbContext = sqlDbContext;

            //Get Google Claims from the googleUserId
            Claim[] googleClaims = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.FirstOrDefault(_ => _[0].Value == googleUserId);
            Assert.NotNull(googleClaims);
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);

            UserService userService = new(Configuration, httpContextAccessor, null);
            Assert.True(userService.IsEmailOfCurrentUserVerified());
        }
        
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void IsEmailOfCurrentUserVerified_Failure(string googleUserId)
        {
            //Mock Database
            SqlDbContext sqlDbContext = MockSqlDbContext.Mock();
            UserExtensions.DbContext = sqlDbContext;

            //Get Google Claims from the googleUserId
            Claim[] googleClaims = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.FirstOrDefault(_ => _[0].Value == googleUserId);
            Assert.NotNull(googleClaims);
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);

            UserService userService = new(Configuration, httpContextAccessor, null);
            Assert.False(userService.IsEmailOfCurrentUserVerified());
        }

        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void GenerateEmailConfirmationToken_Success(string googleUserId)
        {
            //Get Google Claims from the googleUserId
            Claim[] googleClaims = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.FirstOrDefault(_ => _[0].Value == googleUserId);
            Assert.NotNull(googleClaims);
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);

            //Get ProfileData from ClaimPrinciple
            ProfileData profile = httpContextAccessor.HttpContext?.User.GetGoogleAuthData(out bool _)?.GetProfileData();
            Assert.NotNull(profile);
            
            UserService userService = new(Configuration, httpContextAccessor, null);
            string token = userService.GenerateEmailConfirmationToken(profile);
            TestOutputHelper.WriteLine(token);
            Assert.NotEmpty(token);
        }
        
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void ConfirmEmail_Success(string googleUserId)
        {
            //Get Google Claims from the googleUserId
            Claim[] googleClaims = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.FirstOrDefault(_ => _[0].Value == googleUserId);
            Assert.NotNull(googleClaims);
            
            //Mock Database
            SqlDbContext sqlDbContext = MockSqlDbContext.Mock();
            UserExtensions.DbContext = sqlDbContext;
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);

            //Get ProfileData from ClaimPrinciple
            ProfileData profile = httpContextAccessor.HttpContext?.User.GetGoogleAuthData(out bool _)?.GetProfileData();
            Assert.NotNull(profile);

            User unverifiedUser = UserExtensions.GetUser(googleUserId);
            TestOutputHelper.WriteLine("Email verified: " + unverifiedUser.EmailVerified);

            UserService userService = new(Configuration, httpContextAccessor, null);
            string token = userService.GenerateEmailConfirmationToken(profile);
            Assert.True(userService.ConfirmEmail(token));
            
            User verifiedUser = UserExtensions.GetUser(googleUserId);
            TestOutputHelper.WriteLine("Email verified: " + verifiedUser.EmailVerified);
            Assert.True(userService.IsEmailOfCurrentUserVerified());
        }
    }
}