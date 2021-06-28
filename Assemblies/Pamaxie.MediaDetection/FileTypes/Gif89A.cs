using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    public class Gif89A : Image
    {
        public Gif89A() : base(new List<byte[]> {new byte[] {0x47, 0x49, 0x46, 0x38, 0x39, 0x61}}, "image/gif89a", "gif", 0, "Graphics Interchange Format 89A", "EOG; Windows Photo Viewer; Preview"){}
    }
}