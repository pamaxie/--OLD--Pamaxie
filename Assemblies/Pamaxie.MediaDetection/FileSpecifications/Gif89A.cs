namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class Gif89A : FileSpecification
    {
        public Gif89A()
            : base(2, new byte[]{47, 49, 46, 38, 39, 61},0, "89a"){}
    }
}