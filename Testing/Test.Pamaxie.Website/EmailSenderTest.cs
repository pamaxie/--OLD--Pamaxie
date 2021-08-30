﻿using System.Collections.Generic;
using System.Linq;
using Pamaxie.Data;
using Pamaxie.Website.Services;
using Test.Base;
using Xunit;
using Xunit.Abstractions;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Testing class for <see cref="EmailSender"/>
    /// </summary>
    public class EmailSenderTest : Base.Test
    {
        /// <summary>
        /// <inheritdoc cref="MemberData.PersonalUser"/>
        /// </summary>
        public static IEnumerable<object[]> PersonalUser => MemberData.PersonalUser;
        
        public EmailSenderTest(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }
        
        /// <summary>
        /// Sends a confirmation email to the email inside Data
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
                new(Configuration, MockNavigationManager.Mock(), new UserService(Configuration, null!, null));
            emailSender.SendConfirmationEmail(user);
        }

    }
}