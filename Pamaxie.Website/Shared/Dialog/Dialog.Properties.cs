using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public partial class Dialog
    {
        [CascadingParameter] internal MudDialogInstance? MudDialog { get; set; }
        [Parameter] public string ContentText { get; set; } = string.Empty;
        [Parameter] public string ButtonText { get; set; } = string.Empty;
        [Parameter] public Color Color { get; set; }
    }
}