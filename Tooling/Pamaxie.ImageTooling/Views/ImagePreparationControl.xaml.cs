using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.ViewModels;

namespace Pamaxie.ImageTooling.Views
{
    /// <summary>
    /// Interaction logic for ImagePreparationControl.xaml
    /// </summary>
    public partial class ImagePreparationControl
    {
        public ImagePreparationControl()
        {
            InitializeComponent();

            // Set the DataContext for your View
            DataContext = new ImagePreparationViewModel(DialogCoordinator.Instance);
        }
    }
}