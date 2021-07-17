namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class JpegCiff : FileSpecification
    {
        public JpegCiff()
            : base(3, new byte[] {0xFF, 0xD8, 0xFF, 0xE2}, 0, "CIFF")
        {
        }
    }
}