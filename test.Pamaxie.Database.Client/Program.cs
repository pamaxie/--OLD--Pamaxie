using System;
using System.Net;
using Pamaxie.Database.Extensions.Client;

namespace Pamaxie.Database.Client.Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var service = new DatabaseService(new PamaxieDataContext("http://localhost", null),
                new NetworkCredential("testAccount", "123456"));
            service.Connect();
        }
    }
}
