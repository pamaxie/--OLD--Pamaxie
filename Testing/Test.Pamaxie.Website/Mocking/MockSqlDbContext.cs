using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pamaxie.Data;
using Pamaxie.Database.Sql;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Class containing method for mocking SqlDbContext.
    /// </summary>
    internal static class MockSqlDbContext
    {
        /// <summary>
        /// Mocks the Sql Database and adds test users from TestUserData.
        /// </summary>
        /// <returns>Mocked SqlDbContext</returns>
        internal static SqlDbContext Mock()
        {
            IQueryable<User> data = TestUserData.Users;

            Mock<DbSet<User>> mockDbSet = new();
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(data.Provider);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(data.Expression);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockDbSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            Mock<SqlDbContext> mockSqlDbContext = new();
            mockSqlDbContext.Setup(d => d.Users).Returns(mockDbSet.Object);

            return mockSqlDbContext.Object;
        }
    }
}