using Pamaxie.Database.Extensions.Sql.Data;

namespace Pamaxie.Website.Models
{
    public interface IBody
    {
        EmailPurpose Purpose { get; }
        ProfileData ProfileData { get; }
    }
}