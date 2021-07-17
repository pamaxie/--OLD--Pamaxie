using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Diagnostics.CodeAnalysis;

namespace Pamaxie.Website.Shared
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class Dialog
    {
        [CascadingParameter] internal MudDialogInstance? MudDialog { get; set; }
        [Parameter] public string ContentText { get; set; } = string.Empty;
        [Parameter] public string ButtonText { get; set; } = string.Empty;
        [Parameter] public Color Color { get; set; }
    }
}