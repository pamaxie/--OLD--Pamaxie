using System.IO;

namespace Pamaxie.Api.Data
{
    public class UserAuthDataLocation
    {
        public static string AuthDataLocation = Directory.GetCurrentDirectory() + "/data/admins.json";
        public static string AuthDataFolder = "data";
    }
}