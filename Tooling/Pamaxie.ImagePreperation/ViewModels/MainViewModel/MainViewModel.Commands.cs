using Framework.ComponentModel;
using Pamaxie.Wpf.Command;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class MainViewModel : NotifyPropertyChanges
    {
        public DelegateCommand OpenHelpPageCommand { get; set; }
    }
}
