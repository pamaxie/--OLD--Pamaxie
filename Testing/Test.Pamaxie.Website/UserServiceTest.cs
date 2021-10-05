using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Pamaxie.Website.Services;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website_Test
{
    /// <summary>
    /// Testing class for <see cref="UserService"/>
    /// </summary>
    public sealed class UserServiceTest : TestBase
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.AllUnverifiedUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers => MemberData.AllUnverifiedUsers;

        /// <summary>
        /// <inheritdoc cref="MemberData.AllVerifiedGoogleClaimUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllVerifiedGoogleClaimUsers => MemberData.AllVerifiedGoogleClaimUsers;

        /// <summary>
        /// <inheritdoc cref="MemberData.AllUnverifiedGoogleClaimUsers"/>
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedGoogleClaimUsers => MemberData.AllUnverifiedGoogleClaimUsers;

        public UserServiceTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            DatabaseService.UserService = MockUserDataService.Mock();
        }

        /// <summary>
        /// Test for IsEmailOfCurrentUserVerified to see if verified users are true
        /// </summary>
        /// <param name="googleClaims">The claims of a google user</param>
        [Theory]
        [MemberData(nameof(AllVerifiedGoogleClaimUsers))]
        public void IsEmailOfCurrentUserVerified_Success(Claim[] googleClaims)
        {
            //Arrange
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);
            UserService userService = new UserService(Configuration, httpContextAccessor, null);

            //Act
            bool verified = userService.IsEmailOfCurrentUserVerified();

            //Assert
            Assert.True(verified);
        }

        /// <summary>
        /// Test for IsEmailOfCurrentUserVerified to see if unverified users are false
        /// </summary>
        /// <param name="googleClaims">The claims of a google user</param>
        [Theory]
        [MemberData(nameof(AllUnverifiedGoogleClaimUsers))]
        public void IsEmailOfCurrentUserVerified_Failure(Claim[] googleClaims)
        {
            //Arrange
            IHttpContextAccessor httpContextAccessor = MockIHttpContextAccessor.Mock(googleClaims);
            UserService userService = new UserService(Configuration, httpContextAccessor, null);

            //Act
            bool verified = userService.IsEmailOfCurrentUserVerified();

            //Assert
            Assert.False(verified);
        }

        /// <summary>
        /// Test for generating a email confirmation token
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to generate a token from</param>
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void GenerateEmailConfirmationToken(PamaxieUser user)
        {
            //Arrange
            UserService userService = new UserService(Configuration, null!, null);

            //Act
            string token = userService.GenerateEmailConfirmationToken(user);

            //Assert
            Assert.False(string.IsNullOrEmpty(token));
            TestOutputHelper.WriteLine(token);
        }

        /// <summary>
        /// Test for confirming a email for a <see cref="PamaxieUser"/>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> to confirm email</param>
        [Theory]
        [MemberData(nameof(AllUnverifiedUsers))]
        public void ConfirmEmail(PamaxieUser user)
        {
            //Arrange
            TestOutputHelper.WriteLine("Email verified: {0}", user.EmailVerified);
            UserService userService = new UserService(Configuration, null!, null);
            string token = userService.GenerateEmailConfirmationToken(user);

            //Act
            bool confirmedEmail = userService.ConfirmEmail(token);

            //Assert
            Assert.True(confirmedEmail);
            PamaxieUser verifiedPamaxieUser = UserDataServiceExtension.Get(user.Key);
            TestOutputHelper.WriteLine("Email verified: {0}", verifiedPamaxieUser.EmailVerified);
            Assert.True(verifiedPamaxieUser.EmailVerified);
        }
    }
}