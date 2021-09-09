using System;
using Pamaxie.Database.Design;
using Pamaxie.Database.Extensions.Server;
using StackExchange.Redis;
using Test.Base;

namespace Test.Pamaxie.Database.Api_Test
{
    public class FakeDatabaseService : DatabaseService
    {
        public FakeDatabaseService(IPamaxieDataContext dataContext) : base(dataContext)
        {
        }

        public override bool Connect()
        {
            Service = MockIConnectionMultiplexer.Mock();
            if (Service == null)
                return false;
            IDatabase database = Service.GetDatabase();
            
            ConnectionSuccess = true;
            LastConnectionSuccess = DateTime.Now;
            return true;
        }
    }
}