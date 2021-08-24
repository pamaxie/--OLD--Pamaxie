using System.Collections.ObjectModel;
using System.Threading;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.PresentationObjects;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <inheritdoc cref="ImagePreparationViewModel"/>
    public partial class ImagePreparationViewModel
    {
        private readonly CancellationTokenSource _workToken;
        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<PoImageData> _imageData;
        private string _sourceFolderName;
        private string _destinationFolderName;

        private bool _isBusy;
        private bool _isReady;
        private bool _loadingFiles;
        private readonly PoProcessingSettings _processingSettings;
        private PoImageData _selectedItem;
        private Timer _progressUpdateTimer;
    }
}