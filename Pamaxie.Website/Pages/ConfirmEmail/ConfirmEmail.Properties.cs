using Microsoft.AspNetCore.Components;
using Pamaxie.Website.Services;

#pragma warning disable 8618

namespace Pamaxie.Website.Pages
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// <inheritdoc cref="ConfirmEmail"/>
    /// </summary>
    public partial class ConfirmEmail
    {
        [Inject] private NavigationManager NavigationManager { get; set; }
        [Inject] private UserService UserService { get; set; }
        
        /// <summary>
        /// Email confirmation token
        /// </summary>
        [Parameter] public string Token {get; set;}
    }
}