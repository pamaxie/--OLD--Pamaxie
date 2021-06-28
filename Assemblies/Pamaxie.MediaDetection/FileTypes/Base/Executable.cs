using System.Collections.Generic;

namespace Pamaxie.MediaDetection.FileTypes.Base
{
    /// <summary>
    /// This is a specific format for an executable
    /// </summary>
    ///
    /// <remarks>
    /// Created to assist with executable file formats
    /// </remarks>
    public class Executable: FileType
    {
        public Executable(IList<byte[]> signatures, string mediaType, string extension) : base(signatures, mediaType, extension)
        {
        }

        public Executable(IList<byte[]> signatures, string mediaType, string extension, int offset) : base(signatures, mediaType, extension, offset)
        {
        }

        public Executable(IList<byte[]> signatures, string mediaType, string extension, int offset, string name) : base(signatures, mediaType, extension, offset, name)
        {
        }

        public Executable(IList<byte[]> signatures, string mediaType, string extension, int offset, string name, string software) : base(signatures, mediaType, extension, offset, name, software)
        {
        }
    }
}