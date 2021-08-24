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
    /// ViewModel for <see cref="ImageComparisonControl"/>
    /// </summary>
    public partial class ImageComparisonViewModel : NotifyPropertyChanges
    {
        public ImageComparisonViewModel(IDialogCoordinator instance)
        {
            _workToken = new CancellationTokenSource();
            _dialogCoordinator = instance;
            
            CancelImageComparisonAsync = new AsyncDelegateCommand(CancelImageComparisonCommandCallback);

            //Don't allow changing anything relating to loading files if we are already doing this.
            SelectOutputDirectoryCommand = new AsyncDelegateCommand(
                token => SelectDirectoryDialogAsyncCommandCallback(true, token),
                _ => !ProcessingFiles);
            LoadImagesFromDataSourceAsync = new AsyncDelegateCommand(LoadImagesFromDataSourceAsyncCommandCallback,
                _ => !ProcessingFiles);

            SelectInputDirectoryCommand =
                new AsyncDelegateCommand(token => SelectDirectoryDialogAsyncCommandCallback(false, token),
                    _ => !ProcessingFiles || IsBusy);

            StartImageComparisonAsync = new AsyncDelegateCommand(StartImageComparisonAsyncCommandCallback);

            ComparisonSettings = new PoComparisonSettings();
            LoadedImages = new ObservableCollection<PoImageData>();
            LoadedImages.CollectionChanged += (_, _) => { IsReady = CheckIfReady(); };
        }
    }
}