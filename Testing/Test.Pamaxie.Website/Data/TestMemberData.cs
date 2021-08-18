using System.Collections.Generic;
using System.Linq;
using Pamaxie.Database.Sql;
using Pamaxie.Extensions.Sql;

namespace Test.Pamaxie.Website
{
    /// <summary>
    /// Contains all data members that will be used for inline data in the testing methods.
    /// </summary>
    public static class TestMemberData
    {
        public static IEnumerable<object[]> AllGoogleClaimUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();
                foreach (string googleUserId in TestUserData.Users.Select(_ => _.GoogleUserId))
                {
                    list.Add(new object[]
                    {
                        googleUserId
                    });
                }
                return list.AsEnumerable();
            }
        }
        
        public static IEnumerable<object[]> AllVerifiedGoogleClaimUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();
                foreach (string googleUserId in TestUserData.Users.Select(_ => _.GoogleUserId))
                {
                    SqlDbContext sqlDbContext = MockSqlDbContext.Mock();
                    UserExtensions.DbContext = sqlDbContext;
                    if(!UserExtensions.GetUser(googleUserId).EmailVerified) continue;
                    list.Add(new object[]
                    {
                        googleUserId
                    });
                }
                return list.AsEnumerable();
            }
        }
        
        public static IEnumerable<object[]> AllUnverifiedGoogleClaimUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();
                foreach (string googleUserId in TestUserData.Users.Select(_ => _.GoogleUserId))
                {
                    SqlDbContext sqlDbContext = MockSqlDbContext.Mock();
                    UserExtensions.DbContext = sqlDbContext;
                    if(UserExtensions.GetUser(googleUserId).EmailVerified) continue;
                    list.Add(new object[]
                    {
                        googleUserId
                    });
                }
                return list.AsEnumerable();
            }
        }
    }
}