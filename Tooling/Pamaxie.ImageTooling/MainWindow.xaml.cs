using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.ViewModels;

namespace Pamaxie.ImageTooling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            // Set the DataContext for your View
            this.DataContext = new MainViewModel(DialogCoordinator.Instance);
        }
    }
}
