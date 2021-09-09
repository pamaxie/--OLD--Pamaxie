using System.Collections.Generic;
using System.Linq;
using Pamaxie.Data;
using Pamaxie.Website.Services;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website_Test
{
    /// <summary>
    /// Testing class for <see cref="EmailSender"/>
    /// </summary>
    public sealed class EmailSenderTestBaseTest : TestBase
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.PersonalUser"/>
        /// </summary>
        public static IEnumerable<object[]> PersonalUser => MemberData.PersonalUser;

        public EmailSenderTestBaseTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Sends a confirmation email to the personal email inside appsettings.test.json, will fail if no email have been put in
        /// </summary>
        /// <param name="userKey">The user key from inlined data</param>
        [Theory]
        [MemberData(nameof(PersonalUser))]
        public void SendConfirmationEmail_Success(string userKey)
        {
            IPamaxieUser user = TestUserData.ListOfUsers.FirstOrDefault(_ => _.Key == userKey);
            Assert.NotNull(user);
            Assert.False(user.Deleted);

            EmailSender emailSender =
                new EmailSender(Configuration, MockNavigationManager.Mock(),
                    new UserService(Configuration, null!, null));
            emailSender.SendConfirmationEmail(user);
        }
    }
}