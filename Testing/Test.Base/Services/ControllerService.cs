using System.IO;
using Newtonsoft.Json;

namespace Test.TestBase
{
    /// <summary>
    /// Contains methods for to help with controller calls
    /// </summary>
    public static class ControllerService
    {
        /// <summary>
        /// Creates a Stream from a object
        /// </summary>
        /// <param name="body">Any object that will be written into a <see cref="Stream"/></param>
        /// <returns>A <see cref="Stream"/> from the <see cref="object"/></returns>
        public static Stream CreateStream(object body)
        {
            MemoryStream ms = new();
            StreamWriter sw = new(ms);

            string json = JsonConvert.SerializeObject(body);

            sw.Write(json);
            sw.Flush();

            ms.Position = 0;

            return ms;
        }
    }
}