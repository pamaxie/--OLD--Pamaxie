using System.Collections.ObjectModel;
using Pamaxie.ImageTooling.PresentationObjects;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <inheritdoc cref="ImagePreparationViewModel"/>
    public partial class ImagePreparationViewModel
    {
        /// <summary>
        /// <inheritdoc cref="PoProcessingSettings"/>
        /// </summary>
        public PoProcessingSettings ProcessingSettings
        {
            get => _processingSettings;
            private init => SetProperty(ref _processingSettings, value);
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
        public string SourceFolderName
        {
            get => _sourceFolderName;
            set => SetProperty(ref _sourceFolderName, value);
        }

        /// <summary>
        /// name of the output location
        /// </summary>
        public string DestinationFolderName
        {
            get => _destinationFolderName;
            set
            {
                SetProperty(ref _destinationFolderName, value);
                IsReady = CheckIfReady();
            }
        }

        /// <summary>
        /// <see cref="bool"/> if files are currently being loaded
        /// </summary>
        public bool LoadingFiles
        {
            get => _loadingFiles;
            set => SetProperty(ref _loadingFiles, value);
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