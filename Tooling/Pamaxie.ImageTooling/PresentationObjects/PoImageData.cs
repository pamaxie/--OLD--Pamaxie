using Framework.ComponentModel;
using Pamaxie.ImageTooling.PresentationObjects.Interfaces;

namespace Pamaxie.ImageTooling.PresentationObjects
{
    /// <inheritdoc cref="IPoImageData"/>
    public class PoImageData : NotifyPropertyChanges, IPoImageData
    {
        private string _name;
        private string _fileLocation;
        private string _assumedLabel;

        /// <inheritdoc/>
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        /// <inheritdoc/>
        public string FileLocation
        {
            get => _fileLocation;
            set => SetProperty(ref _fileLocation, value);
        }

        /// <inheritdoc/>
        public string AssumedLabel
        {
            get => _assumedLabel;
            set => SetProperty(ref _assumedLabel, value);
        }
    }
}