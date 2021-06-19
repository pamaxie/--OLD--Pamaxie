using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pamaxie.Database.Extensions.Data;

namespace Pamaxie.Website.Authentication.Data
{
    public class GoogleAuthData : IProfileData
    {
        public long Id { get; set; }
        public string GoogleClaimUserId { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureAddress { get; set; }
        public bool Deleted { get; set; }

        public ProfileData GetProfileData()
        {
            return new ProfileData()
            {
                Id = this.Id,
                GoogleClaimUserId = this.GoogleClaimUserId,
                UserName = this.UserName,
                EmailAddress = this.EmailAddress,
                ProfilePictureAddress = this.ProfilePictureAddress
            };
        }
    }
}
