using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// <inheritdoc cref="Dialog"/>
    /// </summary>
    public partial class Dialog
    {
        /// <summary>
        /// Instance of the popup <see cref="MudDialog"/>
        /// </summary>
        [CascadingParameter] internal MudDialogInstance? MudDialog { get; set; }
        
        /// <summary>
        /// Text for the content on the popup dialog
        /// </summary>
        [Parameter] public string ContentText { get; set; } = string.Empty;
        
        /// <summary>
        /// Text for the button on the popup dialog
        /// </summary>
        [Parameter] public string ButtonText { get; set; } = string.Empty;
        
        /// <summary>
        /// Color of the popup dialog
        /// </summary>
        [Parameter] public Color Color { get; set; }
    }
}