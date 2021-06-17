using System.IO;

namespace PamaxieML.Api.Data
{
    public class UserAuthDataLocation
    {
        public static string AuthDataLocation = Directory.GetCurrentDirectory() + "/data/admins.json";
        public static string AuthDataFolder = "data";
    }
}