using System;
using Pamaxie.Database.Extensions.Sql.Data;

namespace Pamaxie.Website.Models
{
    public interface IBody
    {
        EmailPurpose Purpose { get; }
        DateTime Expiration { get; set; }
        ProfileData ProfileData { get; }
    }
}