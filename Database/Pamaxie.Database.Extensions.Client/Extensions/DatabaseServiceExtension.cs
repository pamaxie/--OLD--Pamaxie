using System.Net.Http;
using System.Threading.Tasks;

namespace Pamaxie.Database.Extensions.Client.Extensions
{
    internal static class DatabaseServiceExtension
    {
        internal static HttpResponseMessage SendRequestMessage(this DatabaseService service, HttpRequestMessage message)
            => SendRequestMessageAsync(service, message).Result;

        private static async Task<HttpResponseMessage> SendRequestMessageAsync(DatabaseService service,
            HttpRequestMessage message) => await Task.Run(() => service.Service.SendAsync(message));
    }
}