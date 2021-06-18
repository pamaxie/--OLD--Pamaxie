using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pamaxie.Blazor.Authentication.Data
{
    public interface IProfileData
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string ProfilePictureAddress { get; set; }

        public ProfileData GetProfileData();
    }
}
