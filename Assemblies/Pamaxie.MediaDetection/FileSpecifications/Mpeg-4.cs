namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class Mpeg4 : FileSpecification
    {
        public Mpeg4() : base(8, new byte[] { 0x66, 0x74, 0x79, 0x70, 0x69, 0x73, 0x6F, 0x6D }, 0, "mp4")
        {
        }
    }
}