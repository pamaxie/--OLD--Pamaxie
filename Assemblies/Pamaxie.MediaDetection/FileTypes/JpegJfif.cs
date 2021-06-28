using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    public class JpegJfif : Image
    {
        
        public JpegJfif() : base(new List<byte[]> {new byte[] {0xFF, 0xD8, 0xFF, 0xE0}}, "image/jpeg/jfif", "jpg", 0, "Joint Photographic Experts Group Image Format (JPEG File Interchange Format)", "EOG; Windows Photo Viewer; Preview"){}
    }
}