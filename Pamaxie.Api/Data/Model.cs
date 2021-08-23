using System.IO;

namespace Pamaxie.Api.Data
{
    /// <summary>
    /// Authentication Location Model
    /// </summary>
    public class UserAuthDataLocation
    {
        /// <summary>
        /// Location of the authentication file
        /// </summary>
        public static string AuthDataLocation = Directory.GetCurrentDirectory() + "/data/admins.json";
        
        /// <summary>
        /// Name of the Authentication folder
        /// </summary>
        public static string AuthDataFolder = "data";
    }
}