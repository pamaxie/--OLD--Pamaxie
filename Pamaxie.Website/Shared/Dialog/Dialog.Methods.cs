using MudBlazor;

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
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