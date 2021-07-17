namespace Pamaxie.MediaDetection.FileSpecifications
{
    /// <summary>
    /// 
    /// </summary>
    public class ZipSpecification : FileSpecification
    {
        public ZipSpecification() : base(6,
            new byte[] {0x50, 0x4B, 0x03, 0x04, 0x50, 0x4B, 0x05, 0x06, 0x50, 0x4B, 0x07, 0x08})
        {
        }
    }
}