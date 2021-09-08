namespace Pamaxie.MediaDetection.FileTypes
{
    public class DosExe : FileType
    {
        public DosExe() : base(1, "executable/exe", "exe", "MsDos, MsNE, MsPE", "DOS Executable and its descendants")
        {
        }
    }
}