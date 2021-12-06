using Framework.ComponentModel;
using Pamaxie.Wpf.Command;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="MainWindow"/>
    /// </summary>
    public partial class MainViewModel : NotifyPropertyChanges
    {
        public MainViewModel()
        {
            OpenHelpPageCommand = new DelegateCommand(OpenHelpPageCommandCallback);
        }
    }
}