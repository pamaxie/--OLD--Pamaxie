namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class ZipEmpty : FileSpecification
    {
        public ZipEmpty() : base(6, new byte[] { 0x50, 0x4B, 0x05, 0x06 })
        {
        }
    }
}