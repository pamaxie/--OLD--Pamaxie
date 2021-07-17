using MudBlazor;
using System.Diagnostics.CodeAnalysis;

namespace Pamaxie.Website.Shared
{
    [SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
    public partial class Dialog
    {
        private void Submit()
        {
            MudDialog?.Close(DialogResult.Ok(true));
        }

        private void Cancel()
        {
            MudDialog?.Cancel();
        }
    }
}