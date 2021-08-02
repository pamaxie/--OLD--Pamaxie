using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImagePreparation.ViewModels;

namespace Pamaxie.ImagePreparation
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
            
            this.DataContext = new ImagePreparationViewModel(DialogCoordinator.Instance);
        }
    }
}
