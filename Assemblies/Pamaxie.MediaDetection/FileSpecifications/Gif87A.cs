namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class Gif87A : FileSpecification
    {
        public Gif87A()
            : base(2, new byte[] { 0x47, 0x49, 0x46, 0x38, 0x37, 0x61 }, 0, "87a")
        {
        }
    }
}