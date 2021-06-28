using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    public class Png : Image
    {
        
        public Png() : base(new List<byte[]> {new byte[] {0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A}}, "image/png", "png", 0, "Portable Network Graphics", "EOG; Windows Photo Viewer; Preview"){}
    }
}