using System.Collections.Generic;
using System.Linq;
using Pamaxie.Data;

namespace Test.Base
{
    /// <summary>
    /// Contains all data members that will be used for inline data in the testing methods.
    /// </summary>
    public static class MemberData
    {
        /// <summary>
        /// TODO
        /// </summary>
        public static IEnumerable<object[]> AllUsers => TestUserData.ListOfUsers.Select(_ => _.Key)
            .Select(googleUserId => new object[] {googleUserId}).AsEnumerable();

        /// <summary>
        /// TODO
        /// </summary>
        public static IEnumerable<object[]> AllVerifiedUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();
                foreach (string googleUserId in TestUserData.ListOfUsers.Select(_ => _.Key))
                {
                    SqlDbContext sqlDbContext = MockSqlDbContext.Mock();
                    UserExtensions.DbContext = sqlDbContext;
                    if (!UserExtensions.GetUser(googleUserId).EmailVerified) continue;
                    list.Add(new object[]
                    {
                        googleUserId
                    });
                }

                return list.AsEnumerable();
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();
                foreach (string googleUserId in TestUserData.ListOfUsers.Select(_ => _.Key))
                {
                    SqlDbContext sqlDbContext = MockSqlDbContext.Mock();
                    UserExtensions.DbContext = sqlDbContext;
                    if (UserExtensions.GetUser(googleUserId).EmailVerified) continue;
                    list.Add(new object[]
                    {
                        googleUserId
                    });
                }

                return list.AsEnumerable();
            }
        }

        /// <summary>
        /// Contains the ID of the personal testing user
        /// </summary>
        public static IEnumerable<object[]> PersonalUser =>
            new List<object[]> {new object[] {"101963629560135630792"}}.AsEnumerable();

        /// <summary>
        /// TODO
        /// </summary>
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