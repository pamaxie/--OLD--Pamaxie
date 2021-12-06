namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// Class for the redirection of logout events
    /// </summary>
    public partial class RedirectToLogout
    {
        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo($"/Logout?returnUrl=~/{ReturnUrl}", true);
        }
    }
}