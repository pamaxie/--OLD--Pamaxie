namespace Pamaxie.MediaDetection.FileSpecifications
{
    public class DosZmExe : FileSpecification
    {
        public DosZmExe()
            : base(1, new byte[]{0x5a, 0x4d},0, "Mz"){}
    }
}