using Microsoft.AspNetCore.Components;

#pragma warning disable 8618

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// <inheritdoc cref="RedirectToLogout"/>
    /// </summary>
    public partial class RedirectToLogout
    {
        [Inject] private NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Page to return to after logged out
        /// </summary>
        [Parameter]
        public string ReturnUrl { get; set; }
    }
}