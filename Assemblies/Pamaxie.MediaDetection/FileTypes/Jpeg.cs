using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    public class Jpeg : Image
    {
        public Jpeg() : base(new List<byte[]> {new byte[] {0xFF, 0xD8, 0xFF}}, "image/jpeg", "jpg", 0, "Joint Photographic Experts Group Image Format (Unknown or unspecified version)", "EOG; Windows Photo Viewer; Preview"){}
    }
}