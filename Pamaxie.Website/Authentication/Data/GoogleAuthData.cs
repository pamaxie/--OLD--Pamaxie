using Pamaxie.Database.Extensions.Sql.Data;

#pragma warning disable 8618

namespace Pamaxie.Website.Authentication.Data
{
    public class GoogleAuthData : IProfileData
    {
        public long Id { get; set; }
        public string GoogleClaimUserId { get; set; }
        public string UserName { get; set; }
        internal string FirstName { get; set; }
        internal string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureAddress { get; set; }
        public bool Deleted { get; set; }

        public ProfileData GetProfileData()
        {
            return new()
            {
                Id = Id,
                GoogleClaimUserId = GoogleClaimUserId,
                UserName = UserName,
                EmailAddress = EmailAddress,
                ProfilePictureAddress = ProfilePictureAddress
            };
        }
    }
}
