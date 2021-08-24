using System.Text.RegularExpressions;
using Framework.ComponentModel;
using Pamaxie.ImageTooling.PresentationObjects.Interfaces;

namespace Pamaxie.ImageTooling.PresentationObjects
{
    /// <inheritdoc cref="IPoProcessingSettings"/>
    public class PoProcessingSettings : NotifyPropertyChanges, IPoProcessingSettings
    {
        private bool _colorChange;
        private bool _mirrorImages;
        private bool _stopOnError;
        private string _width;
        private string _height;

        /// <inheritdoc/>
        public bool ColorChange
        {
            get => _colorChange;
            set => SetProperty(ref _colorChange, value);
        }

        /// <inheritdoc/>
        public bool MirrorImages
        {
            get => _mirrorImages;
            set => SetProperty(ref _mirrorImages, value);
        }

        /// <inheritdoc/>
        public bool StopOnError
        {
            get => _stopOnError;
            set => SetProperty(ref _stopOnError, value);
        }

        /// <inheritdoc/>
        public string Width
        {
            get => _width;
            set
            {
                if (ValidateIfText(value))
                {
                    return;
                }

                SetProperty(ref _width, value);
            }
        }

        /// <inheritdoc/>
        public string Height
        {
            get => _height;
            set
            {
                if (ValidateIfText(value))
                {
                    return;
                }

                SetProperty(ref _height, value);
            }
        }

        private static bool ValidateIfText(string value)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(value);
        }
    }
}