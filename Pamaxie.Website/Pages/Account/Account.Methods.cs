using System.Security.Claims;
using System.Threading.Tasks;
using Pamaxie.Database.Extensions.Client;
using Pamaxie.Website.Authentication;

namespace Pamaxie.Website.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// Class for the Account page
    /// </summary>
    public partial class Account
    {
        private async void OnDeleteButtonClicked()
        {
            bool? result = await DialogService.ShowMessageBox(
                "Warning",
                "Deleting your account can't be undone! It will join the dark side of our data and be gone forever! " +
                "(Like seriously we delete it entirely from all of our services and wipe it from existence, there is nothing we can do once its gone.)",
                "DO IT!", cancelText: "No!");

            if (result != null && result.Value)
            {
                User.Delete();
                NavigationManager.NavigateTo("/Logout", true);
            }
            StateHasChanged();
        }

        protected override Task OnInitializedAsync()
        {
            if (HttpContextAccessor.HttpContext?.User == null)
            {
                NavigationManager.NavigateTo("/Login", true);
            }
            ClaimsPrincipal? user = HttpContextAccessor.HttpContext?.User;
            if (user == null)
                return Task.CompletedTask;
        
            User = user.GetGoogleAuthData(out bool hasAccount);

            //Disable the delete button. Not like it would affect anything but no need to have it active either.
            if (!hasAccount)
            {
                DisableDeleteData = true;
            }
            StateHasChanged();
            return Task.CompletedTask;
        }
    }
}