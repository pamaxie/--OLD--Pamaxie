using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    public class DosZmExecutable : Executable
    {
        public DosZmExecutable() : base( new List<byte[]> {new byte[] {0x5A, 0x4D}}, "executable/exe", "exe", 0, "Dos ZM Executable File Format and its descendants (rarely used)", "MsDos, MsNE, MsPE"){}
    }
}