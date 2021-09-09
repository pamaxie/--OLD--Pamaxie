namespace Pamaxie.MediaDetection.FileSpecifications
{
    public sealed class Zip : FileSpecification
    {
        public Zip() : base(6, new byte[] { 0x50, 0x4B, 0x03, 0x04 })
        {
        }
    }
}