namespace Pamaxie.MediaDetection.FileSpecifications
{
    public sealed class DosMzExe : FileSpecification
    {
        public DosMzExe()
            : base(1, new byte[] { 0x4d, 0x5a }, 0, "Mz")
        {
        }
    }
}