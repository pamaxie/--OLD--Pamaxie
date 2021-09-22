using System;
using System.Collections.Generic;
using System.Security.Claims;
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
        public static IEnumerable<object[]> AllUserKeys
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestUserData.ListOfUsers.Count; i++)
                {
                    PamaxieUser user = TestUserData.ListOfUsers[i];

                    if (user.Deleted)
                    {
                        continue;
                    }

                    list.Add(new object[] { user.Key });
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of all testing <see cref="PamaxieUser"/>s
        /// </summary>
        public static IEnumerable<object[]> AllUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestUserData.ListOfUsers.Count; i++)
                {
                    PamaxieUser user = TestUserData.ListOfUsers[i];

                    if (!user.Deleted)
                    {
                        list.Add(new object[] { user });
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of all testing <see cref="PamaxieUser"/>s that does not have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestUserData.ListOfUsers.Count; i++)
                {
                    PamaxieUser user = TestUserData.ListOfUsers[i];

                    if (!user.Deleted && !user.EmailVerified)
                    {
                        list.Add(new object[] { user });
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of all testing Google Claim users that does not have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllVerifiedGoogleClaimUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestUserData.ListOfUsers.Count; i++)
                {
                    PamaxieUser user = TestUserData.ListOfUsers[i];
                    if (!user.EmailVerified)
                    {
                        for (int j = 0; j < TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.Count; j++)
                        {
                            Claim[] claimUser = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims[j];

                            if (user.Key == claimUser[0].Value)
                            {
                                list.Add(new object[] { claimUser });
                            }
                        }
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of all testing Google Claim users that does not have their email verified
        /// </summary>
        public static IEnumerable<object[]> AllUnverifiedGoogleClaimUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestUserData.ListOfUsers.Count; i++)
                {
                    PamaxieUser user = TestUserData.ListOfUsers[i];
                    if (!user.EmailVerified)
                    {
                        for (int j = 0; j < TestGoogleClaimData.ListOfGoogleUserPrincipleClaims.Count; j++)
                        {
                            Claim[] claimUser = TestGoogleClaimData.ListOfGoogleUserPrincipleClaims[j];

                            if (user.Key == claimUser[0].Value)
                            {
                                list.Add(new object[] { claimUser });
                            }
                        }
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Contains the personal testing <see cref="PamaxieUser"/>
        /// </summary>
        public static IEnumerable<object[]> PersonalUser
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestUserData.ListOfUsers.Count; i++)
                {
                    PamaxieUser user = TestUserData.ListOfUsers[i];

                    if (user.Key == "101963629560135630792" && !user.Deleted)
                    {
                        list.Add(new object[] { user });
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of all <see cref="PamaxieApplication"/> keys
        /// </summary>
        public static IEnumerable<object[]> AllApplicationKeys
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestApplicationData.ListOfApplications.Count; i++)
                {
                    PamaxieApplication application = TestApplicationData.ListOfApplications[i];

                    if (!application.Deleted)
                    {
                        list.Add(new object[] { application.Key });
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of all <see cref="PamaxieApplication"/>s
        /// </summary>
        public static IEnumerable<object[]> AllApplications
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestApplicationData.ListOfApplications.Count; i++)
                {
                    PamaxieApplication application = TestApplicationData.ListOfApplications[i];

                    if (!application.Deleted)
                    {
                        list.Add(new object[] { application });
                    }
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of random test <see cref="PamaxieUser"/>s
        /// </summary>
        public static IEnumerable<object[]> RandomUsers
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < 5; i++)
                {
                    PamaxieUser user = new PamaxieUser
                    {
                        UserName = RandomService.GenerateRandomName(),
                        FirstName = RandomService.GenerateRandomName(),
                        LastName = RandomService.GenerateRandomName(),
                        ProfilePictureAddress =
                            "https://lh3.googleusercontent.com/--uodKwFP09o/YTBmgn0JnUI/AAAAAAAAAOw/vPRY_cexRuQnj8du8aFuuqJWn1fZAPW3gCMICGAYYCw/s96-c"
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

                for (int i = 0; i < 5; i++)
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
        public static IEnumerable<object[]> FileLinks
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestFileLinkData.ListOfFileLinks.Count; i++)
                {
                    object[] fileLink = TestFileLinkData.ListOfFileLinks[i];
                    list.Add(new[] { fileLink[0] });
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of file links with expected hash
        /// </summary>
        public static IEnumerable<object[]> FileLinksWithHash
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestFileLinkData.ListOfFileLinks.Count; i++)
                {
                    object[] fileLink = TestFileLinkData.ListOfFileLinks[i];
                    list.Add(new[] { fileLink[0], fileLink[1] });
                }

                return list;
            }
        }

        /// <summary>
        /// Contains a list of file links with file type
        /// </summary>
        public static IEnumerable<object[]> FileLinksWithFileType
        {
            get
            {
                List<object[]> list = new List<object[]>();

                for (int i = 0; i < TestFileLinkData.ListOfFileLinks.Count; i++)
                {
                    object[] fileLink = TestFileLinkData.ListOfFileLinks[i];
                    list.Add(new[] { fileLink[0], fileLink[2] });
                }

                return list;
            }
        }
    }
}