namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class RedirectToLogout
    {
        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo($"/Logout?returnUrl={ReturnUrl}", true);
        }
    }
}