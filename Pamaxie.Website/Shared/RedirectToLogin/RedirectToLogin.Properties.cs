using Microsoft.AspNetCore.Components;

#pragma warning disable 8618

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// <inheritdoc cref="RedirectToLogin"/>
    /// </summary>
    public partial class RedirectToLogin
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        
        /// <summary>
        /// Page to return to after logged in
        /// </summary>
        [Parameter] public string ReturnUrl {get; set;}
    }
}