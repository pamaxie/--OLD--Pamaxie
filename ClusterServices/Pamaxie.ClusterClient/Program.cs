using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Pamaxie.ClusterManager;

namespace Pamaxie.ClusterClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var input = new HelloRequest() {Name = "Tim"};
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);

            var reply = await client.SayHelloAsync(input);
            Console.WriteLine(reply);
        }
    }
}
