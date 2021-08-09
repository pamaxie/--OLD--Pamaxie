using System.Threading;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.Wpf.Command;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class MainViewModel : NotifyPropertyChanges
    {
        public MainViewModel(IDialogCoordinator instance)
        {
            _workToken = new CancellationTokenSource();
            _dialogCoordinator = instance;

            OpenHelpPageCommand = new DelegateCommand(OpenHelpPageCommandCallback);
        }
    }
}