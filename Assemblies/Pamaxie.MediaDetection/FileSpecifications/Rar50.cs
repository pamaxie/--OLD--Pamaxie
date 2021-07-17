namespace Pamaxie.MediaDetection.FileSpecifications
{
    /// <summary>
    /// 
    /// </summary>
    public class Rar50 : FileSpecification
    {
        public Rar50() : base(5, new byte[] {0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00}, 0, "5.0")
        {
        }
    }
}