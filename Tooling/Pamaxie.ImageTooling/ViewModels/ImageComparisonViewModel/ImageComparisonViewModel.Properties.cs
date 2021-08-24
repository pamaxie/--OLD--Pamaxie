using System.Collections.ObjectModel;
using Pamaxie.ImageTooling.PresentationObjects;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <inheritdoc cref="ImageComparisonViewModel"/>
    public partial class ImageComparisonViewModel
    {
        /// <summary>
        /// <inheritdoc cref="PoComparisonSettings"/>
        /// </summary>
        public PoComparisonSettings ComparisonSettings
        {
            get => _comparisonSettings;
            private init => SetProperty(ref _comparisonSettings, value);
        }

        /// <summary>
        /// Selected item in DataGrid
        /// </summary>
        public PoImageData SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        /// <summary>
        /// List of all loaded images which is shown in DataGrid
        /// </summary>
        public ObservableCollection<PoImageData> LoadedImages
        {
            get => _imageData;
            private set => SetProperty(ref _imageData, value);
        }

        /// <summary>
        /// Name of the input location
        /// </summary>
        public string SourceDirectoryName
        {
            get => _sourceDirectoryName;
            set => SetProperty(ref _sourceDirectoryName, value);
        }

        /// <summary>
        /// name of the output location
        /// </summary>
        public string OutputDirectoryName
        {
            get => _outputDirectoryName;
            set
            {
                SetProperty(ref _outputDirectoryName, value);
                IsReady = CheckIfReady();
            }
        }

        /// <summary>
        /// <see cref="bool"/> if files are currently being processed
        /// </summary>
        public bool ProcessingFiles
        {
            get => _processingFiles;
            set => SetProperty(ref _processingFiles, value);
        }

        /// <summary>
        /// <see cref="bool"/> if currently busy
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        /// <summary>
        /// <see cref="bool"/> if ready
        /// </summary>
        public bool IsReady
        {
            get => _isReady;
            set => SetProperty(ref _isReady, value);
        }
    }
}