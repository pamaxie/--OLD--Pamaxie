using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pamaxie.Database.Extensions.Data;
using Pamaxie.Database.Sql.DataClasses;
using Pamaxie.Extensions;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Pamaxie.Website.PageModels
{
    public partial class DashBoard
    {
        private List<Application> Applications { get; set; } = new();
        private Application NewApplication { get; set; }
        private ProfileData Profile { get; set; }
        private bool AcceptedTerms { get; set; }
        private bool AcceptedTos { get; set; }
        private MudTextField<string> PwField1;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pw"></param>
        /// <returns></returns>
        protected static IEnumerable<string> PasswordStrength(string pw)
        {
            if (string.IsNullOrWhiteSpace(pw))
            {
                yield return "Password is required!";
                yield break;
            }
            if (pw.Length < 8)
                yield return "Password must be at least of length 8";
            if (!Regex.IsMatch(pw, @"[A-Z]"))
                yield return "Password must contain at least one capital letter";
            if (!Regex.IsMatch(pw, @"[a-z]"))
                yield return "Password must contain at least one lowercase letter";
            if (!Regex.IsMatch(pw, @"[0-9]"))
                yield return "Password must contain at least one digit";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected string PasswordMatch(string arg)
        {
            return PwField1.Value != arg ? "Passwords don't match" : null;
        }
        
        /// <summary>
        /// Create a new application
        /// </summary>
        protected void CreateEmptyApplication()
        {
            NewApplication = new Application()
            {
                ApplicationId = ApplicationExtensions.GetLastIndex(),
                UserId = Profile.Id,
                Disabled = false,
                LastAuth = DateTime.Now,
                RateLimited = false
            };
        }

        /// <summary>
        /// 
        /// </summary>
        protected void CreateApplication()
        {
            NewApplication.AppToken = PwField1.Value;
            ApplicationExtensions.CreateApplication(NewApplication, out Application createdApp);
            Applications.Add(createdApp);
            NewApplication = null;
        }
    }
}
