namespace Pamaxie.ImageTooling.PresentationObjects.Interfaces
{
    /// <summary>
    /// Image class
    /// </summary>
    public interface IPoImageData
    {
        /// <summary>
        /// Name of image
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Location of image
        /// </summary>
        public string FileLocation { get; set; }

        /// <summary>
        /// Assumed label for image
        /// </summary>
        public string AssumedLabel { get; set; }
    }
}