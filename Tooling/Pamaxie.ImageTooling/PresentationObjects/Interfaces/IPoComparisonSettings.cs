namespace Pamaxie.ImageTooling.PresentationObjects.Interfaces
{
    /// <summary>
    /// Settings for Comparison View
    /// </summary>
    public interface IPoComparisonSettings
    {
        /// <summary>
        /// <see cref="bool"/> if images should be compared with it's hash
        /// </summary>
        public bool HashComparison { get; set; }

        /// <summary>
        /// <see cref="bool"/> if comparison should stop when a error occur
        /// </summary>
        public bool StopOnError { get; set; }
    }
}