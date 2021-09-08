namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class ZipSpanned : FileSpecification
    {
        public ZipSpanned() : base(6, new byte[] { 0x50, 0x4B, 0x07, 0x08 })
        {
        }
    }
}