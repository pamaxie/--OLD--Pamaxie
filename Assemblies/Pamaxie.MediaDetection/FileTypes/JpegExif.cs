using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    public class JpegExif : Image
    {
        public JpegExif() : base(new List<byte[]> {new byte[] {0xFF, 0xD8, 0xFF, 0xE1}}, "image/jpeg/exif", "jpg", 0, "Joint Photographic Experts Group Image Format (Exchangeable Image File Format Version)", "EOG; Windows Photo Viewer; Preview"){}
    }
}