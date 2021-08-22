using System.Collections.Generic;
using System.Linq;

namespace Test.Base
{
    /// <summary>
    /// Contains all data members that will be used for inline data in the testing methods.
    /// </summary>
    public static class MemberData
    {
        public static IEnumerable<object[]> AllGoogleClaimUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();
                foreach (string googleUserId in TestUserData.ListOfUsers.Select(_ => _.Key))
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
                foreach (string googleUserId in TestUserData.ListOfUsers.Select(_ => _.Key))
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
                foreach (string googleUserId in TestUserData.ListOfUsers.Select(_ => _.Key))
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
        
        public static IEnumerable<object[]> AllApplications
        {
            get
            {
                List<object[]> list = TestApplicationData.ListOfApplications.Select(_ => _.Key)
                    .Select(applicationId => new object[] {applicationId}).ToList();
                return list.AsEnumerable();
            }
        }
    }
}