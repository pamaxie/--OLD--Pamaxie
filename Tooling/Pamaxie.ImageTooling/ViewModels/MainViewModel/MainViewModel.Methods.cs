using System.Diagnostics;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <inheritdoc cref="MainViewModel"/>
    public partial class MainViewModel
    {
        private static void OpenHelpPageCommandCallback(object obj)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://wiki.pamaxie.com/en/tooling/image-preperation",
                UseShellExecute = true
            });
        }
    }
}