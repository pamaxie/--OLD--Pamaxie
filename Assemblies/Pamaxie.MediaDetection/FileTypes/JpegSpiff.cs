using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    public class JpegSpiff : Image
    {
        
        public JpegSpiff() : base(new List<byte[]> {new byte[] {0xFF, 0xD8, 0xFF, 0xE8}}, "image/jpeg/spiff", "jpg", 0, "Joint Photographic Experts Group Image Format (Still Picture Interchange File Format)", "EOG; Windows Photo Viewer; Preview"){}
    }
}