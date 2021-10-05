using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
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
        /// Add the email where you want to receive the email
        /// </summary>
        private const string UserEmail = "";

        /// <summary>
        /// Add the email that will send the email
        /// </summary>
        private const string SenderEmail = "";

        /// <summary>
        /// Add the password to the email that will send the email
        /// </summary>
        private const string SenderPassword = "";

        /// <summary>
        /// <inheritdoc cref="MemberData.PersonalUser"/>
        /// </summary>
        public static IEnumerable<object[]> PersonalUser => MemberData.PersonalUser;

        public EmailSenderTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
            Configuration["UserData:EmailAddress"] = UserEmail;
            Configuration["EmailSender:EmailAddress"] = SenderEmail;
            Configuration["EmailSender:Password"] = SenderPassword;
        }

        /// <summary>
        /// This test is used to send a confirmation email to a personal email,
        /// by testing how the layout or prevent the email to be flagged as spam
        /// <para>This test will succeed if not enough information is provided</para>
        /// </summary>
        /// <param name="user">The <see cref="PamaxieUser"/> from inlined data</param>
        [Theory]
        [MemberData(nameof(PersonalUser))]
        public void SendConfirmationEmail(PamaxieUser user)
        {
            if (string.IsNullOrEmpty(UserEmail) ||
                string.IsNullOrEmpty(SenderEmail) ||
                string.IsNullOrEmpty(SenderPassword))
            {
                TestOutputHelper.WriteLine("Information not provided, so the test will not run");
                return;
            }

            //Arrange
            EmailSender emailSender =
                new EmailSender(Configuration, MockNavigationManager.Mock(),
                    new UserService(Configuration, null!, null));

            //Act
            emailSender.SendConfirmationEmail(user);

            //Assert
            TestOutputHelper.WriteLine("Email sent to your personal email address!");
        }
    }
}