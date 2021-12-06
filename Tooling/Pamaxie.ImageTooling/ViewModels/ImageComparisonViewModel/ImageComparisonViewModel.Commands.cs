using Pamaxie.Wpf.Command;

namespace Pamaxie.ImageTooling.ViewModels
{
    /// <inheritdoc cref="ImageComparisonViewModel"/>
    public partial class ImageComparisonViewModel
    {
        /// <summary>
        /// Command for setting the output location
        /// </summary>
        public AsyncDelegateCommand SelectOutputDirectoryCommand { get; }

        /// <summary>
        /// Command for setting the input location
        /// </summary>
        public AsyncDelegateCommand SelectInputDirectoryCommand { get; }

        /// <summary>
        /// Command for loading images from location
        /// </summary>
        public AsyncDelegateCommand LoadImagesFromDataSourceAsync { get; }
        
        /// <summary>
        /// Command for starting Image Preparation
        /// </summary>
        public AsyncDelegateCommand StartImageComparisonAsync { get; }
        
        /// <summary>
        /// Command for canceling Image Preparation
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private AsyncDelegateCommand CancelImageComparisonAsync { get; }
    }
}