using System.Collections.ObjectModel;
using Framework.ComponentModel;
using Pamaxie.ImagePreparation.PresentationObjects;

namespace Pamaxie.ImagePreparation.ViewModels
{
    public partial class ImagePreparationViewModel : NotifyPropertyChanges
    {
        public PoProcessingSettings ProcessingSettings
        {
            get => _processingSettings;
            set => SetProperty(ref _processingSettings, value);
        }

        public PoImageData SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ObservableCollection<PoImageData> SelectedGroup
        {
            get => _selectedGroup;
            set => SetProperty(ref _selectedGroup, value);
        }

        public ObservableCollection<PoImageData> LoadedImages
        {
            get => _imageData;
            set => SetProperty(ref _imageData, value);
        }

        public string SourceFolderName
        {
            get => _sourceFolderName;
            set
            {
                SetProperty(ref _sourceFolderName, value);
            }
        }

        public string DestinationFolderName
        {
            get => _destinationFolderName;
            set
            {
                SetProperty(ref _destinationFolderName, value);
                IsReady = CheckIfReady();
            }
        }

        public bool LoadingFiles
        {
            get => _loadingFiles;
            set => SetProperty(ref _loadingFiles, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public bool IsReady
        {
            get => _isReady;
            set => SetProperty(ref _isReady, value);
        }
    }
}