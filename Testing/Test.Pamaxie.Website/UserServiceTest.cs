using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Pamaxie.Data;
using Pamaxie.Website.Authentication;
using Pamaxie.Website.Services;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Testing class for <see cref="UserService"/>
    /// </summary>
    public class UserServiceTest : Base.Test
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllVerifiedUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllVerifiedUsers => MemberData.AllVerifiedUsers;
        
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUnverifiedUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers => MemberData.AllUnverifiedUsers;
        
        public UserServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        /// <summary>
        /// Goes through all verified users from the testing data, and tests the IsEmailOfCurrentUserVerified succeeds
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllVerifiedUsers))]
        public void IsEmailOfCurrentUserVerified_Success(string userKey)
        {
            //Get Google Claims from the googleUserId
            Claim[] googleClaims = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.FirstOrDefault(_ => _[0].Value == userKey);
            Assert.NotNull(googleClaims);
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);

            UserService userService = new(Configuration, httpContextAccessor, null);
            Assert.True(userService.IsEmailOfCurrentUserVerified());
        }
        
        /// <summary>
        /// Goes through all unverified users from the testing data, and tests the IsEmailOfCurrentUserVerified fails
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void IsEmailOfCurrentUserVerified_Failure(string userKey)
        {
            //TODO Mock UserInteractionExtension, as it will be used in UserService
            
            //Get Google Claims from the googleUserId
            Claim[] googleClaims = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.FirstOrDefault(_ => _[0].Value == userKey);
            Assert.NotNull(googleClaims);
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);

            UserService userService = new(Configuration, httpContextAccessor, null);
            Assert.False(userService.IsEmailOfCurrentUserVerified());
        }

        /// <summary>
        /// Goes through all unverified users and generates a email confirmation token
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void GenerateEmailConfirmationToken_Success(string userKey)
        {
            //Get Google Claims from the googleUserId
            Claim[] googleClaims = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.FirstOrDefault(_ => _[0].Value == userKey);
            Assert.NotNull(googleClaims);
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);

            //Get ProfileData from ClaimPrinciple
            IPamaxieUser user = httpContextAccessor.HttpContext?.User.GetGoogleAuthData(out bool _);
            Assert.NotNull(user);
            
            UserService userService = new(Configuration, httpContextAccessor, null);
            string token = userService.GenerateEmailConfirmationToken(user);
            TestOutputHelper.WriteLine(token);
            Assert.NotEmpty(token);
        }
        
        /// <summary>
        /// Goes through all unverified users and confirms their email out of a generated email confirmation token
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void ConfirmEmail_Success(string userKey)
        {
            //Get Google Claims from the googleUserId
            Claim[] googleClaims = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.FirstOrDefault(_ => _[0].Value == userKey);
            Assert.NotNull(googleClaims);
            
            //TODO Mock UserInteractionExtension, as it will be used in UserService
            MockUserDataService mockedUserDataService = new(); //Delete this once a mocking implementation have been added
            
            //Mock HttpContext with principle claims
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);

            //Check if the user is the current logged in
            Assert.NotNull(httpContextAccessor.HttpContext?.User.GetGoogleAuthData(out bool _));

            IPamaxieUser unverifiedPamaxieUser = mockedUserDataService.Get(userKey);
            TestOutputHelper.WriteLine("Email verified: " + unverifiedPamaxieUser.EmailVerified);

            UserService userService = new(Configuration, httpContextAccessor, null);
            string token = userService.GenerateEmailConfirmationToken(unverifiedPamaxieUser);
            Assert.True(userService.ConfirmEmail(token));
            
            IPamaxieUser verifiedPamaxieUser = mockedUserDataService.Get(userKey);
            TestOutputHelper.WriteLine("Email verified: " + verifiedPamaxieUser.EmailVerified);
            Assert.True(userService.IsEmailOfCurrentUserVerified());
        }
    }
}