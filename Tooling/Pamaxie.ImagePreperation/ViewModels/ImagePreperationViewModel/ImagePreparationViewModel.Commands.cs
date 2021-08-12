using Framework.ComponentModel;
using Pamaxie.Wpf.Command;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class ImagePreparationViewModel : NotifyPropertyChanges
    {
        public AsyncDelegateCommand SelectOutputDirectoryCommand { get; set; }
        public AsyncDelegateCommand SelectInputDirectoryCommand { get; set; }
        public AsyncDelegateCommand LoadImagesFromDataSourceAsync { get; set; }
        public AsyncDelegateCommand CancelImagePerpetrationAsync { get; set; }
        public AsyncDelegateCommand StartImagePerpetrationAsync { get; set; }
    }
}
