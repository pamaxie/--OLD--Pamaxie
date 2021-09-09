using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
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
        private static List<string[]> ListOfEverything
        {
            get
            {
                List<string[]> list = TestApplicationData.ListOfApplications.Select(application =>
                    new[] { application.Key, JsonConvert.SerializeObject(application) }).ToList();
                list.AddRange(TestUserData.ListOfUsers.Select(user =>
                    new[] { user.Key, JsonConvert.SerializeObject(user) }));
                return list;
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

            //Setup for StringGet
            mockDatabase.Setup(_ => _.StringGet(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) =>
                    (RedisValue)ListOfEverything.FirstOrDefault(value => value[0] == key)?[1]);

            //Setup for StringSet
            mockDatabase.Setup(_ => _.StringSet(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(),
                It.IsAny<When>(), It.IsAny<CommandFlags>()))
                .Callback<RedisKey, RedisValue, TimeSpan?, When, CommandFlags>((key, value, _, _, _) =>
                {
                    if (ListOfEverything.Any(_ => _[0] == key))
                        ListOfEverything.Where(_ => _[0] == key).ToList()
                            .ForEach(_ => ListOfEverything[ListOfEverything.IndexOf(_)] = new []{ key.ToString(), value.ToString() });
                    else
                        ListOfEverything.Add(new []{ key.ToString(), value.ToString() });
                })
                .Returns(true);

            //Setup for KeyExists
            mockDatabase.Setup(_ => _.KeyExists(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Returns<RedisKey, CommandFlags>((key, _) => ListOfEverything.Any(value => value[0] == key));

            //Setup for KeyDelete
            mockDatabase.Setup(_ => _.KeyDelete(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                .Callback<RedisKey, CommandFlags>((key, _) =>
                {
                    string[] item = ListOfEverything.FirstOrDefault(value => value[0] == key);
                    ListOfEverything.Remove(item);
                }).Returns<RedisKey, CommandFlags>((key, _) => ListOfEverything.All(value => value[0] != key));
            
            mockConnectionMultiplexer.Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);
            return mockConnectionMultiplexer.Object;
        }
    }
}