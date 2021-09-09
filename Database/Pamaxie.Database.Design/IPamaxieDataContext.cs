namespace Pamaxie.Database.Design
{
    /// <summary>
    /// TODO
    /// </summary>
    public interface IPamaxieDataContext
    {
        /// <summary>
        /// Creates a Connection string through the options reached in.
        /// </summary>
        /// <returns></returns>
        public string ConnectionString();

        /// <summary>
        /// Defines the available instances for data connection
        /// </summary>
        public string DataInstances { get; }

        /// <summary>
        /// Defines the password to use for the instances
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// How many attempts should be made to connect to the service
        /// </summary>
        public int ReconnectionAttempts { get; }
    }
}