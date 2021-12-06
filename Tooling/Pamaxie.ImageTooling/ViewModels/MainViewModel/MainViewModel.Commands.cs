using Pamaxie.Wpf.Command;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <inheritdoc cref="MainViewModel"/>
    public partial class MainViewModel
    {
        /// <summary>
        /// Command for opening the wiki page in a browser
        /// </summary>
        public DelegateCommand OpenHelpPageCommand { get; }
    }
}