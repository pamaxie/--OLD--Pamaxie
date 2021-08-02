using System.Collections.ObjectModel;
using System.Threading;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImagePreparation.PresentationObjects;
using Pamaxie.Wpf.Command;

namespace Pamaxie.ImagePreparation.ViewModels
{
    public partial class ImagePreparationViewModel : NotifyPropertyChanges
    {
        public ImagePreparationViewModel(IDialogCoordinator instance)
        {
            _workToken = new CancellationTokenSource();
            _dialogCoordinator = instance;

            CancelImagePerpetrationAsync = new AsyncDelegateCommand(CancelImagePreparationCommandCallback);
            
            //Don't allow changing anything relating to loading files if we are already doing this.
            SelectOutputDirectoryCommand = new AsyncDelegateCommand(token => SelectDirectoryDialogAsyncCommandCallback(token, true),
                a => !LoadingFiles);
            LoadImagesFromDataSourceAsync = new AsyncDelegateCommand(LoadImagesFromDataSourceAsyncCommandCallback,
                a => !LoadingFiles);

            SelectInputDirectoryCommand =
                new AsyncDelegateCommand(token => SelectDirectoryDialogAsyncCommandCallback(token, false), a => !LoadingFiles || IsBusy);

            OpenHelpPageCommand = new DelegateCommand(OpenHelpPageCommandCallback);
            StartImagePerpetrationAsync = new AsyncDelegateCommand(StartImagePerpetrationAsyncCommandCallback);

            ProcessingSettings = new PoProcessingSettings();
            LoadedImages = new ObservableCollection<PoImageData>();
            LoadedImages.CollectionChanged += (_, _) =>
            {
                IsReady = CheckIfReady();
            };
            
        }
    }
}