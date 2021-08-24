using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.ViewModels;

namespace Pamaxie.ImageTooling.Views
{
    /// <summary>
    /// Interaction logic for ImageComparisonControl.xaml
    /// </summary>
    public partial class ImageComparisonControl
    {
        public ImageComparisonControl()
        {
            InitializeComponent();

            // Set the DataContext for your View
            DataContext = new ImageComparisonViewModel(DialogCoordinator.Instance);
        }
    }
}