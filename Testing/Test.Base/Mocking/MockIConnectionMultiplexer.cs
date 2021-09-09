using System.Collections.Generic;
using System.Linq;
using Moq;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Test.Base
{
    /// <summary>
    /// Class containing method for mocking <see cref="IConnectionMultiplexer"/>.
    /// </summary>
    public static class MockIConnectionMultiplexer
    {
        private static IEnumerable<string[]> ListOfEverything
        {
            get
            {
                List<string[]> list = TestApplicationData.ListOfApplications.Select(application =>
                    new[] { application.Key, JsonConvert.SerializeObject(application) }).ToList();
                list.AddRange(TestUserData.ListOfUsers.Select(user =>
                    new[] { user.Key, JsonConvert.SerializeObject(user) }));
                return list.AsEnumerable();
            }
        }

        /// <summary>
        /// Mocks the <see cref="IConnectionMultiplexer"/> for testing usage
        /// </summary>
        /// <returns>Mocked IHttpContextAccessor</returns>
        public static IConnectionMultiplexer Mock()
        {
            Mock<IConnectionMultiplexer> mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            mockConnectionMultiplexer.Setup(_ => _.IsConnected).Returns(false);

            Mock<IDatabase> mockDatabase = new Mock<IDatabase>();
            mockDatabase.Setup(_ => _.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) =>
                    (RedisValue)ListOfEverything.FirstOrDefault(value => value[0] == key)?[1]);

            mockConnectionMultiplexer.Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);
            return mockConnectionMultiplexer.Object;
        }
    }
}