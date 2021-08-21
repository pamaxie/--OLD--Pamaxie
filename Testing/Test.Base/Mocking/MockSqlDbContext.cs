using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pamaxie.Data;
using Pamaxie.Database.Sql;
using Pamaxie.Extensions.Sql;

namespace Test.Base
{
    /// <summary>
    /// Class containing method for mocking SqlDbContext.
    /// </summary>
    public static class MockSqlDbContext
    {
        /// <summary>
        /// Mocks the Sql Database and adds test data.
        /// </summary>
        /// <returns>Mocked SqlDbContext</returns>
        public static SqlDbContext Mock()
        {
            Application application1 = new()
            {

            };
            
            Mock<SqlDbContext> mockSqlDbContext = new();
            
            //Add test users to the Db
            mockSqlDbContext.Setup(_ => _.Users).Returns(MockDbSet(TestUserData.ListOfUsers, mockSqlDbContext));
            
            //Add test applications to the Db
            mockSqlDbContext.Setup(_ => _.Applications).Returns(MockDbSet(new List<Application>(), mockSqlDbContext));
            
            SqlDbContext mockedSqlDbContext = mockSqlDbContext.Object;
            ApplicationExtensions.DbContext = mockedSqlDbContext;

            foreach (Application application in TestApplicationData.ListOfApplications)
            {
                ApplicationExtensions.CreateApplication(application, out Application app);
            }
            
            return mockSqlDbContext.Object;
        }
        
        private static DbSet<T> MockDbSet<T>(ICollection<T> entities, Mock<SqlDbContext> mockSqlDbContext) where T : class
        {
            List<T> list = new();
            list.AddRange(entities);
            IQueryable<T> query = entities.AsQueryable();
            
            Mock<DbSet<T>> mockedDbSet = new();
            mockedDbSet.As<IQueryable>().Setup(_ => _.Provider).Returns(query.Provider);
            mockedDbSet.As<IQueryable<T>>().Setup(_ => _.Expression).Returns(query.Expression);
            mockedDbSet.As<IQueryable<T>>().Setup(_ => _.ElementType).Returns(query.ElementType);
            mockedDbSet.As<IEnumerable<T>>().Setup(_ => _.GetEnumerator()).Returns(() => query.GetEnumerator());

            //Expand this to on how you use the DbSet collection.
            mockedDbSet.Setup(_ => _.Add(It.IsAny<T>())).Callback<T>(entities.Add);
            
            return mockedDbSet.Object;
        }
    }
}