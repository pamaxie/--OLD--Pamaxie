namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// Class for the redirection of login events
    /// </summary>
    public partial class RedirectToLogin
    {
        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo($"/Login?returnUrl=~/{ReturnUrl}", true);
        }
    }
}