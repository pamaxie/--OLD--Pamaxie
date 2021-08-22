namespace Pamaxie.Database.Extensions.DatabaseExtensions
{
    public class DatabaseService : IDatabaseService
    {
        private string _redisInstances;
        private string _password;
        private int _reconnectionAttempts;

        string IDatabaseService.RedisInstances
        {
            get => _redisInstances;
            set => _redisInstances = value;
        }

        string IDatabaseService.Password
        {
            get => _password;
            set => _password = value;
        }

        int IDatabaseService.ReconnectionAttempts
        {
            get => _reconnectionAttempts;
            set => _reconnectionAttempts = value;
        }

        public bool Connect()
        {
            throw new System.NotImplementedException();
        }

        public bool IsDatabaseAvailable()
        {
            throw new System.NotImplementedException();
        }

        public ushort DatabaseLatency()
        {
            throw new System.NotImplementedException();
        }
    }
}