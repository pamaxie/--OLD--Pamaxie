using System.Collections.ObjectModel;
using Framework.ComponentModel;
using Pamaxie.ImageTooling.PresentationObjects;

namespace Pamaxie.ImageTooling.ViewModels
{
    public partial class ImageComparisonViewModel : NotifyPropertyChanges
    {
        public PoComparisonSettings ComparisonSettings
        {
            get => _comparisonSettings;
            set => SetProperty(ref _comparisonSettings, value);
        }

        public PoImageData SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
        }

        public ObservableCollection<PoImageData> LoadedImages
        {
            get => _imageData;
            set => SetProperty(ref _imageData, value);
        }

        public string SourceDirectoryName
        {
            get => _sourceDirectoryName;
            set => SetProperty(ref _sourceDirectoryName, value);
        }

        public string OutputDirectoryName
        {
            get => _outputDirectoryName;
            set
            {
                SetProperty(ref _outputDirectoryName, value);
                IsReady = CheckIfReady();
            }
        }

        public bool ProcessingFiles
        {
            get => _processingFiles;
            set => SetProperty(ref _processingFiles, value);
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