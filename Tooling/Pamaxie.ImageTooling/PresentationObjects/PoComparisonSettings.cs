using Framework.ComponentModel;
using Pamaxie.ImageTooling.PresentationObjects.Interfaces;

namespace Pamaxie.ImageTooling.PresentationObjects
{
    /// <inheritdoc cref="IPoComparisonSettings"/>
    public class PoComparisonSettings : NotifyPropertyChanges, IPoComparisonSettings
    {
        private bool _hashComparison;
        private bool _stopOnError;
        private bool _percentileFolders;

        /// <inheritdoc/>
        public bool HashComparison
        {
            get => _hashComparison;
            set => SetProperty(ref _hashComparison, value);
        }

        /// <inheritdoc/>
        public bool StopOnError
        {
            get => _stopOnError;
            set => SetProperty(ref _stopOnError, value);
        }

        /// <summary>
        /// <see cref="bool"/> if images should go into folders where the predicted label percentage is highest
        /// </summary>
        public bool PercentileFolders
        {
            get => _percentileFolders;
            set => SetProperty(ref _percentileFolders, value);
        }
    }
}