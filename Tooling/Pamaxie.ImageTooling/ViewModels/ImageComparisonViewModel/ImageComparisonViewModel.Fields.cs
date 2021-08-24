using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using MahApps.Metro.Controls.Dialogs;
using Pamaxie.ImageTooling.PresentationObjects;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <inheritdoc cref="ImageComparisonViewModel"/>
    public partial class ImageComparisonViewModel
    {
        private readonly CancellationTokenSource _workToken;
        private readonly IDialogCoordinator _dialogCoordinator;

        private ObservableCollection<PoImageData> _imageData;
        private string _sourceDirectoryName;
        private string _outputDirectoryName;
        private Stopwatch _processingTimeStopwatch;

        private bool _isBusy;
        private bool _isReady;
        private bool _processingFiles;
        private readonly PoComparisonSettings _comparisonSettings;
        private PoImageData _selectedItem;
        private Timer _progressUpdateTimer;
    }
}