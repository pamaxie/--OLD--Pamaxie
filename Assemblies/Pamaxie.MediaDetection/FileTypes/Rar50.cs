using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class Rar50 : Archive
    {
       
        public Rar50() : base( new List<byte[]> {new byte[] {0x52, 0x61, 0x72, 0x21, 0x1A, 0x07, 0x01, 0x00}}, "archive/rar5.0", "rar", 0, "Roshal Archive Version 5.0 Onwards", "WinRar; 7Zip;"){}
    }
}