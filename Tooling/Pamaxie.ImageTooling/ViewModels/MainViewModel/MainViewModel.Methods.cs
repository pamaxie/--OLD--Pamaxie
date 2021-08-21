using System.Diagnostics;
using Framework.ComponentModel;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class MainViewModel : NotifyPropertyChanges
    {
        private void OpenHelpPageCommandCallback(object obj)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://wiki.pamaxie.com/en/tooling/image-preperation",
                UseShellExecute = true
            });
        }
    }
}
