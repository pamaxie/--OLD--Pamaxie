using Microsoft.AspNetCore.Components;

#pragma warning disable 8618

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class RedirectToLogin
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Parameter] public string ReturnUrl {get; set;}
    }
}