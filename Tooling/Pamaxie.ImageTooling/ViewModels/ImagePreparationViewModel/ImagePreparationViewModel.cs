using System.Collections.ObjectModel;
using System.Threading;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.PresentationObjects;
using Pamaxie.ImageTooling.Views;
using Pamaxie.Wpf.Command;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <summary>
    /// ViewModel for <see cref="ImagePreparationControl"/>
    /// </summary>
    public partial class ImagePreparationViewModel : NotifyPropertyChanges
    {
        public ImagePreparationViewModel(IDialogCoordinator instance)
        {
            _workToken = new CancellationTokenSource();
            _dialogCoordinator = instance;

            CancelImagePreparationAsync = new AsyncDelegateCommand(CancelImagePreparationCommandCallback);

            //Don't allow changing anything relating to loading files if we are already doing this.
            SelectOutputDirectoryCommand = new AsyncDelegateCommand(
                token => SelectDirectoryDialogAsyncCommandCallback(true, token),
                _ => !LoadingFiles);
            LoadImagesFromDataSourceAsync = new AsyncDelegateCommand(LoadImagesFromDataSourceAsyncCommandCallback,
                _ => !LoadingFiles);

            SelectInputDirectoryCommand =
                new AsyncDelegateCommand(token => SelectDirectoryDialogAsyncCommandCallback(false, token),
                    _ => !LoadingFiles || IsBusy);

            StartImagePreparationAsync = new AsyncDelegateCommand(StartImagePerpetrationAsyncCommandCallback);

            ProcessingSettings = new PoProcessingSettings();
            LoadedImages = new ObservableCollection<PoImageData>();
            LoadedImages.CollectionChanged += (_, _) => { IsReady = CheckIfReady(); };
        }
    }
}