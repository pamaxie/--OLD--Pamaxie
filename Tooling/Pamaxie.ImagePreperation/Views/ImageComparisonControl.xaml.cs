using System.Windows.Controls;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.ViewModels;

namespace Pamaxie.ImageTooling.Views
{
    /// <summary>
    /// Interaction logic for ImagePreperationControl.xaml
    /// </summary>
    public partial class ImageComparisonControl : UserControl
    {
        public ImageComparisonControl()
        {
            InitializeComponent();

            // Set the DataContext for your View
            this.DataContext = new ImageComparisonViewModel(DialogCoordinator.Instance);
        }
    }
}
