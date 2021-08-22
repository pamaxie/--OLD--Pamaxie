using System;
using Pamaxie.Data;

namespace Pamaxie.Website.Models
{
    public interface IBody
    {
        EmailPurpose Purpose { get; }
        DateTime Expiration { get; set; }
        IPamaxieUser User { get; }
    }
}