using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using Pamaxie.Data;
using Pamaxie.Database.Sql;

namespace Test.Pamaxie.Website
{
    internal static class MockSqlDbContext
    {
        internal static SqlDbContext Mock(IConfiguration configuration)
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