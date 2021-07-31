using Framework.ComponentModel;
using Pamaxie.ImagePreparation.PresentationObjects.Interfaces;

namespace Pamaxie.ImagePreparation.PresentationObjects
{
    public class PoImageData : NotifyPropertyChanges, IPoImageData
    {
        private string _name;
        private string _fileLocation;
        private string _assumedLabel;
        private string _fileExtension;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string FileLocation
        {
            get => _fileLocation;
            set => SetProperty(ref _fileLocation, value);
        }

        public string AssumedLabel
        {
            get => _assumedLabel;
            set => SetProperty(ref _assumedLabel, value);
        }

        public string FileExtension
        {
            get => _fileExtension;
            set => SetProperty(ref _fileExtension, value);
        }
    }
}
