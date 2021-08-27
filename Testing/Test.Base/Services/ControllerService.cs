using System.IO;
using Newtonsoft.Json;

namespace Test.Base
{
    public static class ControllerService
    {
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