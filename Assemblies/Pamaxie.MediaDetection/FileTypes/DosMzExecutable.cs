using System.Collections.Generic;
using Pamaxie.MediaDetection.FileTypes.Base;

namespace Pamaxie.MediaDetection.FileTypes
{
    public class DosMzExecutable : Executable
    {
        public DosMzExecutable() : base(new List<byte[]> {new byte[] {0x4D, 0x5A}}, "executable/exe", "exe", 0, "Dos MZ Executable File Format (including NE and PE)", "MsDos, MsNE, MsPE")
        { }
    }
}