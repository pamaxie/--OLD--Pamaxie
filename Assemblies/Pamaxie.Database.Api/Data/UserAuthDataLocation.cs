using System.IO;

namespace Pamaxie.Api.Data
{
    /// <summary>
    /// BUG: Constant varialbels for User Authentication, should be changed to use something like a Database later on!
    /// </summary>
    public class UserAuthDataLocation
    {
        public static readonly string AuthDataLocation = Directory.GetCurrentDirectory() + "/data/admins.json";
        public static readonly string AuthDataFolder = "data";
    }
}