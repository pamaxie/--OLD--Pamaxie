using MudBlazor;

namespace Pamaxie.Website.Shared
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// Class for the popup dialog
    /// </summary>
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