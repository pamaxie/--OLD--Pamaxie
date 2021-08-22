using System;
using System.Collections.Generic;
using System.Globalization;
using Pamaxie.Data;

namespace Test.Base
{
    public static class TestApplicationData
    {
        /// <summary>
        /// List of Users for the SqlDbContext.
        /// </summary>
        public static readonly List<PamaxieApplication> ListOfApplications = new()
        {
            new PamaxieApplication()
            {
                Key = "64922",
                OwnerKey = "102617494281791801620",
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
                //AppToken = "Apple"
            },
            new PamaxieApplication()
            {
                Key = "53324",
                OwnerKey = "102617494281791801620",
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
                //AppToken = "Pie"
            },
            new PamaxieApplication()
            {
                Key = "51080",
                OwnerKey = "104669818103955818761",
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
                //AppToken = "Orange"
            },
            new PamaxieApplication()
            {
                Key = "65779",
                OwnerKey = "104669818103955818761",
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
                //AppToken = "Pear"
            },
            new PamaxieApplication()
            {
                Key = "60105",
                OwnerKey = "104669818103955818761",
                ApplicationName = "",
                LastAuth = DateTime.Parse("15 05 2008", new CultureInfo("de-DE")),
                RateLimited = false,
                Disabled = false,
                Deleted = false
                //AppToken = "Cake"
            }
        };
    }
}