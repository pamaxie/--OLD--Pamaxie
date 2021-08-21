using System.Collections.ObjectModel;
using System.Threading;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.PresentationObjects;
using Pamaxie.Wpf.Command;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class ImageComparisonViewModel : NotifyPropertyChanges
    {
        public ImageComparisonViewModel(IDialogCoordinator instance)
        {
            _workToken = new CancellationTokenSource();
            _dialogCoordinator = instance;

            CancelImagePerpetrationAsync = new AsyncDelegateCommand(CancelImagePreparationCommandCallback);

            //Don't allow changing anything relating to loading files if we are already doing this.
            SelectOutputDirectoryCommand = new AsyncDelegateCommand(token => SelectDirectoryDialogAsyncCommandCallback(token, true),
                a => !ProcessingFiles);
            LoadImagesFromDataSourceAsync = new AsyncDelegateCommand(LoadImagesFromDataSourceAsyncCommandCallback,
                a => !ProcessingFiles);

            SelectInputDirectoryCommand =
                new AsyncDelegateCommand(token => SelectDirectoryDialogAsyncCommandCallback(token, false), a => !ProcessingFiles || IsBusy);

            StartImagePerpetrationAsync = new AsyncDelegateCommand(StartImagePerpetrationAsyncCommandCallback);

            ComparisonSettings = new PoComparisonSettings();
            LoadedImages = new ObservableCollection<PoImageData>();
            LoadedImages.CollectionChanged += (_, _) =>
            {
                IsReady = CheckIfReady();
            };
        }
    }
}