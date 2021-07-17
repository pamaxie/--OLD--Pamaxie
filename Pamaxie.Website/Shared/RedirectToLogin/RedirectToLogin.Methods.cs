using System.Diagnostics.CodeAnalysis;

namespace Pamaxie.Website.Shared
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class RedirectToLogin
    {
        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("Account/Login", true);
        }
    }
}