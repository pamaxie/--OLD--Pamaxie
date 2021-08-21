namespace Pamaxie.ClusterClientLib
{
    public class ConnectionData
    {
        /// <summary>
        /// The Address of the Host
        /// </summary>
        public string HostAddress { get; set; }

        /// <summary>
        /// The Port the Host uses
        /// </summary>
        public short HostPort { get; set; }

        /// <summary>
        /// The Shared secret between host and client to ensure communication
        /// is kept private once a connection was made
        /// </summary>
        public string SharedKey { get; set; }

    }
}
