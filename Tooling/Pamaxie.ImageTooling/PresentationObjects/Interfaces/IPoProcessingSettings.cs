namespace Pamaxie.ImageTooling.PresentationObjects.Interfaces
{
    /// <summary>
    /// Settings for Preparation view
    /// </summary>
    public interface IPoProcessingSettings
    {
        /// <summary>
        /// <see cref="bool"/> if image should have a different colored version
        /// </summary>
        public bool ColorChange { get; set; }

        /// <summary>
        /// <see cref="bool"/> if image should have a mirrored version
        /// </summary>
        public bool MirrorImages { get; set; }

        /// <summary>
        /// <see cref="bool"/> if processing should stop when a error occur
        /// </summary>
        public bool StopOnError { get; set; }

        /// <summary>
        /// Width of output images
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Height of output images
        /// </summary>
        public string Height { get; set; }
    }
}