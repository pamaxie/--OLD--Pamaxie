using Microsoft.AspNetCore.Components;
using Pamaxie.Extensions.Sql;
using Pamaxie.Website.Authentication;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Pamaxie.Website.Shared
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class NavMenu
    {
        private void DrawerToggle()
         {
             _drawerOpen = !_drawerOpen;
         }
     
         protected override Task OnInitializedAsync()
         {
             bool hasAccount = false;
             ClaimsPrincipal? user = HttpContextAccessor.HttpContext?.User;
             Profile = user?.GetGoogleAuthData(out hasAccount)?.GetProfileData();
             UserHasAccount = hasAccount;
             return base.OnInitializedAsync();
         }
     
         protected override Task OnAfterRenderAsync(bool firstRender)
         {
             if (Profile == null)
                 return Task.CompletedTask;
             if (!UserHasAccount || Profile is {Deleted: true })
             {
                 ShowCreateAccount = true;
             }
             else
             {
                 ShowCreateAccount = false;
             }
             StateHasChanged();
             return base.OnAfterRenderAsync(firstRender);
         }
     
         private void Login()
         {
             NavigationManager.NavigateTo("/Login", true);
         }
     
         private void Logout()
         {
             NavigationManager.NavigateTo("/Logout", true);
         }
     
         private void AccountManagement()
         {
             NavigationManager.NavigateTo("/account", true);
         }
     
         private void CreateAccount()
         {
             UserExtensions.CreateUser(Profile);
     
         //Forcefully reload after creating the new user to make sure everything is a - ok
             NavigationManager.NavigateTo(NavigationManager.Uri, true);
             UserHasAccount = true;
         }
    }
}