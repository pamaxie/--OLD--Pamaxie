namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class RedirectToLogin
    {
        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo($"/Login?returnUrl=~/{ReturnUrl}", true);
        }
    }
}