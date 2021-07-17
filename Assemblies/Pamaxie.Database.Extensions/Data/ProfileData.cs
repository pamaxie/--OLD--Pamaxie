namespace Pamaxie.Database.Extensions.Data
{
    public class ProfileData : IProfileData
    {
        public long Id { get; set; }
        public string GoogleClaimUserId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureAddress { get; set; }
        public bool Deleted { get; set; }

        public ProfileData GetProfileData()
        {
            return this;
        }
    }
}