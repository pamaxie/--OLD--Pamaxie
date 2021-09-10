using System;
using System.Collections.Generic;
using System.Linq;

namespace Test.Base
{
    /// <summary>
    /// Contains all data members that will be used for inline data in the testing methods.
    /// </summary>
    public static class MemberData
    {
        /// <summary>
        /// Contains a list of all testing users
        /// </summary>
        public static IEnumerable<object[]> AllUsers =>
            (from user in TestUserData.ListOfUsers where !user.Deleted select new object[] { user.Key }).AsEnumerable();

        /// <summary>
        /// Contains a list of all testing users that have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllVerifiedUsers =>
            (from user in TestUserData.ListOfUsers
                where !user.Deleted && user.EmailVerified
                select new object[] { user.Key })
            .AsEnumerable();

        /// <summary>
        /// Contains a list of all testing users that does not have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers =>
            (from user in TestUserData.ListOfUsers
                where !user.Deleted && !user.EmailVerified
                select new object[] { user.Key })
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
        public static IEnumerable<object[]> RandomUsers
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

        /// <summary>
        /// Contains a list of unused user keys which can be used to create new users
        /// </summary>
        public static IEnumerable<object[]> RandomApplications
        {
            get
            {
                List<object[]> list = new List<object[]>();
                for (int i = 0; i < 6; i++)
                {
                    Random rnd = new Random();
                    string ownerKey = TestUserData.ListOfUsers[rnd.Next(TestUserData.ListOfUsers.Count - 1)].Key;
                    string applicationName = RandomService.GenerateRandomName();
                    string authorizationToken = RandomService.GenerateRandomName();

                    list.Add(new object[] { ownerKey, applicationName, authorizationToken });
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of file links
        /// </summary>
        public static IEnumerable<object[]> FileLinks =>
            TestFileLinkData.ListOfFileLinks.AsEnumerable().Select(link => new[] { link[0] });

        /// <summary>
        /// Contains a list of file links with expected hash
        /// </summary>
        public static IEnumerable<object[]> FileLinksWithHash =>
            TestFileLinkData.ListOfFileLinks.AsEnumerable().Select(link => new[] { link[0], link[1] });

        /// <summary>
        /// Contains a list of file links with file type
        /// </summary>
        public static IEnumerable<object[]> FileLinksWithFileType => TestFileLinkData.ListOfFileLinks.AsEnumerable()
            .Select(link => new[] { link[0], link[2] });
    }
}