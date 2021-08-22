using System;

namespace Pamaxie.Website.Models
{
    public interface IBody
    {
        EmailPurpose Purpose { get; }
        DateTime Expiration { get; set; }
        ProfileData ProfileData { get; }
    }
}