using Pamaxie.Website.Services;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Testing class for EmailSender
    /// </summary>
    public class EmailSenderTest : Base.Test
    {
        public EmailSenderTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        /// <summary>
        /// Sends a confirmation email to the email inside Data
        /// </summary>
        [Fact]
        public void SendConfirmationEmail_Success()
        {
            ProfileData data = TestProfileData.Profile;
            
            EmailSender emailSender =
                new(Configuration, MockNavigationManager.Mock(), new UserService(Configuration, null!, null));
            emailSender.SendConfirmationEmail(data);
        }

    }
}