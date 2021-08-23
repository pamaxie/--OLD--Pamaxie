using Pamaxie.ImageTooling.ViewModels;

namespace Pamaxie.ImageTooling
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            // Set the DataContext for your View

            DataContext = new MainViewModel();
        }
    }
}