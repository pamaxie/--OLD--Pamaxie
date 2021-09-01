using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.TestBase
{
    /// <summary>
    /// Contains all data members that will be used for inline data in the testing methods.
    /// </summary>
    public static class MemberData
    {
        /// <summary>
        /// Contains a list of all testing users
        /// </summary>
        public static IEnumerable<object[]> AllUsers => TestUserData.ListOfUsers.Select(_ => _.Key)
            .Select(key => new object[] { key }).AsEnumerable();

        /// <summary>
        /// Contains a list of all testing users that have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllVerifiedUsers =>
            (from user in TestUserData.ListOfUsers where user.EmailVerified select new object[] { user.Key })
            .AsEnumerable();

        /// <summary>
        /// Contains a list of all testing users that does not have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers =>
            (from user in TestUserData.ListOfUsers where !user.EmailVerified select new object[] { user.Key })
            .AsEnumerable();

        /// <summary>
        /// Contains the ID of the personal testing user
        /// </summary>
        public static IEnumerable<object[]> PersonalUser =>
            new List<object[]> { new object[] { "101963629560135630792" } }.AsEnumerable();

        /// <summary>
        /// Contains a list of all applications
        /// </summary>
        public static IEnumerable<object[]> AllApplications => TestApplicationData.ListOfApplications.Select(_ => _.Key)
            .Select(key => new object[] { key }).AsEnumerable();

        /// <summary>
        /// Contains a list of unused user keys which can be used to create new users
        /// </summary>
        public static IEnumerable<object[]> RandomUserData
        {
            get
            {
                List<object[]> list = new List<object[]>();
                for (int i = 0; i < 6; i++)
                {
                    string userName = RandomService.GenerateRandomName();
                    string firstName = RandomService.GenerateRandomName();
                    string lastName = RandomService.GenerateRandomName();
                    string emailAddress = $"{firstName}.{lastName}@fakemail.com";
                    
                    list.Add(new object[] { userName, firstName, lastName, emailAddress });
                }

                return list;
            }
        }
    }
}