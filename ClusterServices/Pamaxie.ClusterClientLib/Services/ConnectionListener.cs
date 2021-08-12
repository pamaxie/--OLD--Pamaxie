using System;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pamaxie.ClusterManager;

namespace Pamaxie.ClusterClient.Services
{
    public class ConnectionListener
    {
        /// <summary>
        /// Creates a new connection listener that will wait for UDP broadcasts of a Host to establish a new connection
        /// </summary>
        public ConnectionListener()
        {
            
        }

        public event EventHandler ConnectionFound;

        /// <summary>
        /// Holds the connection service that is used by the listener
        /// </summary>
        public ConnectionService ConnectionService;

        /// <summary>
        /// Token that is responsible for ensuring that the Client knows the Host
        /// </summary>
        public string ConnectionToken { get; set; }
        
        /// <summary>
        /// Unique ID of the client to identify itself. Usually is auto generated through the hardware.
        /// </summary>
        public string ClientId { get; set; }

        protected virtual void OnConnectionFound(EventArgs e)
        {
            var handler = ConnectionFound;
            handler?.Invoke(this, e);
        }

        /// <summary>
        /// Listens for new tcp connections
        /// </summary>
        /// <returns></returns>
        public async Task<bool> ListenForConnection()
        {
            int PORT = 9876;
            UdpClient udpClient = new UdpClient();
            udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));

            var from = new IPEndPoint(0, 0);
            await Task.Run(() =>
            {
                while (true)
                {
                    var recvBuffer = udpClient.Receive(ref from);
                    var recvString = Encoding.UTF8.GetString(recvBuffer);
                    var data = recvString.Split('\0');

                    foreach (var key in data)
                    {
                        
                    }
                    Console.WriteLine();
                }
            });



           //Added a return value to temporarily fix build issues - PKoldborg
            return true;
        }
    }
}
