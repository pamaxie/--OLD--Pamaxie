namespace Pamaxie.MediaDetection.FileSpecifications
{
    public sealed class PngSpec : FileSpecification
    {
        public PngSpec() : base(4, new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A })
        {
        }
    }
}