using System.Threading;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class MainViewModel : NotifyPropertyChanges
    {
        private CancellationTokenSource _workToken;
        private readonly IDialogCoordinator _dialogCoordinator;
    }
}