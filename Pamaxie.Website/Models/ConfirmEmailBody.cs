using System;

namespace Pamaxie.Website.Models
{
    public class ConfirmEmailBody : IBody
    {
        public EmailPurpose Purpose => EmailPurpose.EMAIL_CONFIRMATION;
        public DateTime Expiration { get; set; } = DateTime.UtcNow.AddDays(10);
        public ProfileData ProfileData { get; }

        public ConfirmEmailBody(ProfileData profileData)
        {
            ProfileData = profileData;
        }
    }
}