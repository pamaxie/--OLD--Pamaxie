using MudBlazor;

namespace Pamaxie.Website.Shared.Dialog
{
    public partial class Dialog
    {
        private void Submit()
        {
            MudDialog.Close(DialogResult.Ok(true));
        }

        private void Cancel()
        {
            MudDialog.Cancel();
        }
    }
}