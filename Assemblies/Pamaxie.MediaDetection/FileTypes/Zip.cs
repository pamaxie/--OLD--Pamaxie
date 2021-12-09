namespace Pamaxie.MediaDetection.FileTypes
{
    public sealed class Zip : FileType
    {
        public Zip() : base(6, "ZIP archive", "zip", "WinZip; WinRar; 7Zip;", "archive/zip")
        {
        }
    }
}