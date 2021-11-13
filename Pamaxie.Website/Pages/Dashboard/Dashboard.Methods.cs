using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pamaxie.Data;
using Pamaxie.Database.Extensions.Client;
using Pamaxie.Website.Authentication;

namespace Pamaxie.Website.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// Class for the Dashboard page
    /// </summary>
    public partial class Dashboard
    {
        protected override Task OnInitializedAsync()
        {
            ClaimsPrincipal? user = HttpContextAccessor.HttpContext?.User;
            if (user == null)
                return Task.CompletedTask;

            User = user.GetGoogleAuthData(out bool hasAccount);
            //Something weird is going on. Logout the user to make sure we are not de-syncing or anything.
            if (User == null)
            {
                NavigationManager.NavigateTo("/Logout", true);
                return Task.CompletedTask;
            }

            //Has no Account has to create one first before accessing this.
            if (!hasAccount)
                return Task.CompletedTask;

            Applications = User.GetAllApplications().ToList();
            StateHasChanged();
            return Task.CompletedTask;
        }

        private async Task DeleteApplication(PamaxieApplication pamaxieApplication)
        {
            bool? result = await DialogService.ShowMessageBox(
                $"Do you really wanna delete {pamaxieApplication.ApplicationName}?",
                "Deleting your applications can't be undone! It will join the dark side of our data and be gone forever! " +
                "(Like seriously we delete it entirely from all of our services and wipe it from existence, there is nothing we can do once its gone.)",
                "Jep", cancelText: "Nope");
            if (result is true && User is not null)
            {
                pamaxieApplication.Delete();
                Applications = User.GetAllApplications().ToList();
                StateHasChanged();
            }
        }

        private async Task EnableOrDisableApplication(PamaxieApplication pamaxieApplication)
        {
            bool? result = true;
            if (!pamaxieApplication.Disabled)
                result = await DialogService.ShowMessageBox(
                    $"Do you really wanna disable {pamaxieApplication.ApplicationName}?",
                    "Disabling your application will block anyone from accessing it. You can re-enable it any time however! " +
                    "This will just block access to it if you suspect issues with your token and want to validate everything is ok.",
                    "Jep", cancelText: "Nope");
            if (result is true)
            {
                pamaxieApplication.EnableOrDisable();

                if (User != null)
                    Applications = User.GetAllApplications().ToList();
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
            if (User == null)
                return;

            if (!UserService.IsEmailOfCurrentUserVerified())
            {
                UserService.ShowVerifyEmailDialog();
                return;
            }

            NewApplication = new PamaxieApplication()
            {
                OwnerKey = User.UniqueKey,
                Disabled = false,
                LastAuth = DateTime.Now,
                RateLimited = false
            };
        }

        private void CreateApplication()
        {
            if (NewApplication == null)
                return;

            NewApplication.Credentials.AuthorizationToken = PwField1.Value;
            PamaxieApplication createdApp = NewApplication.Create();
            Applications.Add(createdApp);
            NewApplication = null;
        }
    }
}