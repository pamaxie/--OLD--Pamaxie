namespace Pamaxie.MediaDetection.FileSpecifications
{
    public sealed class JpegJfif : FileSpecification
    {
        public JpegJfif()
            : base(3, new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 }, 0, "jfif")
        {
        }
    }
}