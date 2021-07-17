namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class JpegSpiff : FileSpecification
    {
        public JpegSpiff()
            : base(3, new byte[] {0xFF, 0xD8, 0xFF, 0xE8}, 0, "spiff")
        {
        }
    }
}