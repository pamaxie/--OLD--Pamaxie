using System.Collections.ObjectModel;
using System.Threading;
using Framework.ComponentModel;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.PresentationObjects;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class ImageComparisonViewModel : NotifyPropertyChanges
    {
        private CancellationTokenSource _workToken;
        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<PoImageData> _imageData;
        private string _sourceDirectoryName;
        private string _outputDirectoryName;


        private bool _isBusy;
        private bool _isReady;
        private bool _processingFiles;
        private PoComparisonSettings _comparisonSettings;
        private PoImageData _selectedItem;
        private ObservableCollection<PoImageData> _selectedGroup;
        private System.Threading.Timer _progressUpdateTimer;
    }
}