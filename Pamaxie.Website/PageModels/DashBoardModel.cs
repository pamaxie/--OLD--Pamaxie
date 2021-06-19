using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Pamaxie.Database.Extensions.Data;
using Pamaxie.Database.Sql.DataClasses;
using Pamaxie.Extensions;

namespace Pamaxie.Website.PageModels
{
    public class DashBoardModel : ComponentBase
    {

        protected List<Application> Applications { get; set; } = new List<Application>();
        protected Application NewApplication { get; set; }
        protected ProfileData Profile { get; set; }
        protected bool AcceptedTerms { get; set; }
        protected bool AcceptedTos { get; set; }
        protected MudTextField<string> pwField1;


        protected IEnumerable<string> PasswordStrength(string pw)
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

        protected string PasswordMatch(string arg)
        {
            return pwField1.Value != arg ? "Passwords don't match" : null;
        }
        
        //Create a new application
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

        protected void CreateApplication()
        {
            NewApplication.AppToken = pwField1.Value;
            ApplicationExtensions.CreateApplication(NewApplication, out var createdApp);
            Applications.Add(createdApp);
            NewApplication = null;
        }


    }
}
