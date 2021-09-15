using System.Collections.Generic;
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
    public sealed class EmailSenderTest : TestBase
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.PersonalUser"/>
        /// </summary>
        public static IEnumerable<object[]> PersonalUser => MemberData.PersonalUser;

        public EmailSenderTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        /// <summary>
        /// Sends a confirmation email to the personal email inside appsettings.test.json,
        /// MemberData will be empty if no email address have been put in
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(PersonalUser))]
        public void SendConfirmationEmail(PamaxieUser user)
        {
            //Act
            EmailSender emailSender =
                new EmailSender(Configuration, MockNavigationManager.Mock(),
                    new UserService(Configuration, null!, null));
            emailSender.SendConfirmationEmail(user);
            TestOutputHelper.WriteLine("Email sent to your personal email address!");
        }
    }
}