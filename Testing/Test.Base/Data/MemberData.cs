using System;
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
        /// Contains a list of all testing <see cref="PamaxieUser"/> keys
        /// </summary>
        public static IEnumerable<object[]> AllUserKeys =>
            TestUserData.ListOfUsers.Where(_ => _.Deleted == false)
                .Select(user => new object[] { user.Key });

        /// <summary>
        /// Contains a list of all testing <see cref="PamaxieUser"/>s
        /// </summary>
        public static IEnumerable<object[]> AllUsers =>
            TestUserData.ListOfUsers.Select(user => new object[] { user });

        /// <summary>
        /// Contains a list of all testing <see cref="PamaxieUser"/> keys that have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllVerifiedUserKeys =>
            TestUserData.ListOfUsers.Where(_ => _.Deleted == false && _.EmailVerified)
                .Select(user => new object[] { user.Key });

        /// <summary>
        /// Contains a list of all testing <see cref="PamaxieUser"/>s that have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllVerifiedUsers =>
            TestUserData.ListOfUsers.Where(_ => _.Deleted == false && _.EmailVerified)
                .Select(user => new object[] { user });

        /// <summary>
        /// Contains a list of all testing <see cref="PamaxieUser"/> keys that does not have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUserKeys =>
            TestUserData.ListOfUsers.Where(_ => _.Deleted == false && _.EmailVerified == false)
                .Select(user => new object[] { user.Key });

        /// <summary>
        /// Contains a list of all testing  <see cref="PamaxieUser"/>s that does not have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers =>
            TestUserData.ListOfUsers.Where(_ => _.Deleted == false && _.EmailVerified == false)
                .Select(user => new object[] { user });

        /// <summary>
        /// Contains the ID of the personal testing <see cref="PamaxieUser"/>
        /// </summary>
        public static IEnumerable<object[]> PersonalUserKey =>
            new List<object[]> { new object[] { "101963629560135630792" } };

        /// <summary>
        /// Contains the personal testing <see cref="PamaxieUser"/>
        /// </summary>
        public static IEnumerable<object[]> PersonalUser =>
            TestUserData.ListOfUsers.Where(_ => _.Key == "101963629560135630792")
                .Select(user => new object[] { user });

        /// <summary>
        /// Contains a list of all <see cref="PamaxieApplication"/> keys
        /// </summary>
        public static IEnumerable<object[]> AllApplicationKeys =>
            TestApplicationData.ListOfApplications.Where(_ => _.Deleted == false)
                .Select(application => new object[] { application.Key });

        /// <summary>
        /// Contains a list of all <see cref="PamaxieApplication"/>s
        /// </summary>
        public static IEnumerable<object[]> AllApplications =>
            TestApplicationData.ListOfApplications.Where(_ => _.Deleted == false)
                .Select(application => new object[] { application });

        /// <summary>
        /// Contains a list of random test <see cref="PamaxieUser"/>s
        /// </summary>
        public static IEnumerable<object[]> RandomUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < 6; i++)
                {
                    PamaxieUser user = new PamaxieUser
                    {
                        UserName = RandomService.GenerateRandomName(),
                        FirstName = RandomService.GenerateRandomName(),
                        LastName = RandomService.GenerateRandomName(),
                        ProfilePictureAddress =
                            "https://lh3.googleusercontent.com/--uodKwFP09o/YTBmgn0JnUI/AAAAAAAAAOw/vPRY_cexRuQnj8du8aFuuqJWn1fZAPW3gCMICGAYYCw/s96-c",
                    };
                    user.EmailAddress = $"{user.FirstName}.{user.LastName}@fakemail.com";
                    list.Add(new object[] { user });
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of random test <see cref="PamaxieApplication"/>s
        /// </summary>
        public static IEnumerable<object[]> RandomApplications
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < 6; i++)
                {
                    Random rnd = new Random();
                    PamaxieApplication application = new PamaxieApplication
                    {
                        TTL = DateTime.Now,
                        Credentials = new AppAuthCredentials
                        {
                            AuthorizationToken = RandomService.GenerateRandomName(),
                            AuthorizationTokenCipher = "",
                            LastAuth = DateTime.Now
                        },
                        OwnerKey = TestUserData.ListOfUsers[rnd.Next(TestUserData.ListOfUsers.Count - 1)].Key,
                        ApplicationName = RandomService.GenerateRandomName(),
                        LastAuth = DateTime.Now
                    };
                    list.Add(new object[] { application });
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