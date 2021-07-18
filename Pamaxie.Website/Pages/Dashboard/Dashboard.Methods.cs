using Pamaxie.Data;
using Pamaxie.Extensions.Sql;
using Pamaxie.Website.Authentication;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Pamaxie.Website.Pages
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class Dashboard
    {
        protected override Task OnInitializedAsync()
        {
            ClaimsPrincipal? user = HttpContextAccessor.HttpContext?.User;
            if (user == null)
                return Task.CompletedTask;

            Profile = user.GetGoogleAuthData(out bool hasAccount)?.GetProfileData();
            //Something weird is going on. Logout the user to make sure we are not de-syncing or anything.
            if (Profile == null)
            {
                NavigationManager.NavigateTo("/Logout", true);
                return Task.CompletedTask;
            }

            //Has no Account has to create one first before accessing this.
            if (!hasAccount)
                return Task.CompletedTask;

            Applications = ApplicationExtensions.GetApplications(Profile.Id).ToList();
            StateHasChanged();
            return Task.CompletedTask;
        }

        private async Task DeleteApplication(Application application)
        {
            bool? result = await DialogService.ShowMessageBox(
                $"Do you really wanna delete {application.ApplicationName}?",
                "Deleting your applications can't be undone! It will join the dark side of our data and be gone forever! " +
                "(Like seriously we delete it entirely from all of our services and wipe it from existence, there is nothing we can do once its gone.)",
                "Jep", cancelText: "Nope");
            if (result is true && Profile is not null)
            {
                application.DeleteApplication();
                Applications = ApplicationExtensions.GetApplications(Profile.Id).ToList();
                StateHasChanged();
            }
        }

        private async Task SwitchIfApplicationEnabled(Application application)
        {
            bool? result = await DialogService.ShowMessageBox(
                $"Do you really wanna disable {application.ApplicationName}?",
                "Disabling your application will block anyone from accessing it. You can re-enable it any time however! " +
                "This will just block access to it if you suspect issues with your token and want to validate everything is ok.",
                "Jep", cancelText: "Nope");
            if (result is true)
            {
                application.SetApplicationStatus(application.Disabled);

                if (Profile != null)
                    Applications = ApplicationExtensions.GetApplications(Profile.Id).ToList();
                StateHasChanged();
            }
        }

        private static IEnumerable<string> PasswordStrength(string pw)
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

        private string? DoPasswordsMatch(string arg)
        {
            return PwField1.Value != arg ? "Passwords don't match" : null;
        }

        private void CreateEmptyApplication()
        {
            if (Profile == null)
                return;

            NewApplication = new Application()
            {
                ApplicationId = ApplicationExtensions.GetLastIndex(),
                UserId = Profile.Id,
                Disabled = false,
                LastAuth = DateTime.Now,
                RateLimited = false
            };
        }

        private void CreateApplication()
        {
            if (NewApplication == null)
                return;

            NewApplication.AppToken = PwField1.Value;
            ApplicationExtensions.CreateApplication(NewApplication, out Application createdApp);
            Applications.Add(createdApp);
            NewApplication = null;
        }
    }
}