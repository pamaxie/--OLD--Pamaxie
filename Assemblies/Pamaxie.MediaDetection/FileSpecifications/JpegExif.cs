namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class JpegExif : FileSpecification
    {
        public JpegExif()
            : base(3, new byte[] {0xFF, 0xD8, 0xFF, 0xE1}, 0, "exif")
        {
        }
    }
}