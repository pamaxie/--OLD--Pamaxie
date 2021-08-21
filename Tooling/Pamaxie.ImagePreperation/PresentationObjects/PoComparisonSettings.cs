using System.Text.RegularExpressions;
using Framework.ComponentModel;
using Pamaxie.ImageTooling.PresentationObjects.Interfaces;

namespace Pamaxie.ImageTooling.PresentationObjects
{
    public class PoComparisonSettings : NotifyPropertyChanges, IPoComparisonSettings
    {
        private bool _hashComparison;
        private bool _stopOnError;
        private bool _percentileFolders;

        public bool HashComparison
        {
            get => _hashComparison;
            set => SetProperty(ref _hashComparison, value);
        }

        public bool StopOnError
        {
            get => _stopOnError;
            set => SetProperty(ref _stopOnError, value);
        }

        public bool PercentileFolders
        {
            get => _percentileFolders;
            set => SetProperty(ref _percentileFolders, value);
        }

        private bool ValidateIfText(string value)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(value);
        }
    }
}
