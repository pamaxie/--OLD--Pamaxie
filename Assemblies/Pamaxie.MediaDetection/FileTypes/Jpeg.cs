namespace Pamaxie.MediaDetection.FileTypes
{
    public sealed class Jpeg : FileType
    {
        public Jpeg() : base(3, "image/jpeg", "jpg", "EOG; Windows Photo Viewer; Preview",
            "Joint Photographic Experts Group Image Format (Unknown or unspecified version)")
        {
        }
    }
}