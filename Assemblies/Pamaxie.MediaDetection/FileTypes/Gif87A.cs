using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    public class Gif87A : Image
    {
        public Gif87A() : base(new List<byte[]> {new byte[] {0x47, 0x49, 0x46, 0x38, 0x37, 0x61}}, "image/gif87a", "gif", 0, "Graphics Interchange Format 87A", "EOG; Windows Photo Viewer; Preview"){}
    }
}