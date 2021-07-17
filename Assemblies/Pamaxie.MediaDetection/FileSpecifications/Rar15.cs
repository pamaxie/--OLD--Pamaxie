namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class Rar15 : FileSpecification
    {
        public Rar15() : base(5, new byte[] {0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x00}, 0, "1.5")
        {
        }
    }
}