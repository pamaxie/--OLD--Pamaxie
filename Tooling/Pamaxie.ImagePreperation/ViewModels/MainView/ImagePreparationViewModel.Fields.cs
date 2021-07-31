using System.Collections.ObjectModel;
using System.Threading;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImagePreparation.PresentationObjects;

namespace Pamaxie.ImagePreparation.ViewModels
{
    public partial class ImagePreparationViewModel : NotifyPropertyChanges
    {
        private CancellationTokenSource _workToken;
        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<PoImageData> _imageData;
        private string _sourceFolderName;
        private string _destinationFolderName;

        private bool _isBusy;
        private bool _isReady;
        private bool _loadingFiles;
        private PoProcessingSettings _processingSettings;
        private PoImageData _selectedItem;
        private ObservableCollection<PoImageData> _selectedGroup;
    }
}