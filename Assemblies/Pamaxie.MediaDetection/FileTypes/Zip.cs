using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    /// <summary>
    /// 
    /// </summary>
    public class Zip : Archive
    {
        public Zip() : base( new List<byte[]> {new byte[] {0x50, 0x4B, 0x03, 0x04, 0x50, 0x4B, 0x05, 0x06, 0x50, 0x4B, 0x07, 0x08}}, "archive/zip", "zip", 0, "ZIP archive", "WinZip; WinRar; 7Zip;"){}
    }
}