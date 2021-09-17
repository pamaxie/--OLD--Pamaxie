using System;
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
        private static List<string[]> _listOfEverything;

        /// <summary>
        /// Mocks the <see cref="IConnectionMultiplexer"/> for testing usage
        /// </summary>
        /// <returns>Mocked <see cref="IConnectionMultiplexer"/></returns>
        public static IConnectionMultiplexer Mock()
        {
            _listOfEverything = new List<string[]>(TestApplicationData.ListOfApplications.Select(application =>
                new[] { application.Key, JsonConvert.SerializeObject(application) }).ToList());
            _listOfEverything.AddRange(TestUserData.ListOfUsers.Select(user =>
                new[] { user.Key, JsonConvert.SerializeObject(user) }));

            Mock<IConnectionMultiplexer> mockConnectionMultiplexer = new Mock<IConnectionMultiplexer>();
            mockConnectionMultiplexer.Setup(_ => _.IsConnected).Returns(false);

            Mock<IDatabase> mockDatabase = new Mock<IDatabase>();

            //Setup for StringGet
            mockDatabase.Setup(_ => _.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) =>
                    (RedisValue)_listOfEverything.FirstOrDefault(value => value[0] == key)?[1]);

            //Setup for StringSet
            mockDatabase.Setup(_ => _.StringSet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(),
                    It.IsAny<When>(), It.IsAny<CommandFlags>()))
                .Callback<RedisKey, RedisValue, TimeSpan?, When, CommandFlags>((key, value, _, _, _) =>
                {
                    if (_listOfEverything.Any(_ => _[0] == key))
                        _listOfEverything[_listOfEverything.FindIndex(_ => _[0] == key)] =
                            new[] { key.ToString(), value.ToString() };
                    else
                        _listOfEverything.Add(new[] { key.ToString(), value.ToString() });
                })
                .Returns(true);

            //Setup for KeyExists
            mockDatabase.Setup(_ => _.KeyExists(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => _listOfEverything.Any(value => value[0] == key));

            //Setup for KeyDelete
            mockDatabase.Setup(_ => _.KeyDelete(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Callback<RedisKey, CommandFlags>((key, _) =>
                {
                    _listOfEverything.RemoveAt(_listOfEverything.FindIndex(i => i[0] == key));
                }).Returns<RedisKey, CommandFlags>((key, _) => _listOfEverything.All(value => value[0] != key));

            mockConnectionMultiplexer.Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);

            return mockConnectionMultiplexer.Object;
        }
    }
}