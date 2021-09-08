namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class Gif89A : FileSpecification
    {
        public Gif89A()
            : base(2, new byte[] { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61 }, 0, "89a")
        {
        }
    }
}