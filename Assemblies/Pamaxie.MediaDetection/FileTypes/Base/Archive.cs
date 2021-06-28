using System.Collections.Generic;

namespace Pamaxie.MediaDetection.FileTypes.Base
{
    /// <summary>
    /// This is a specific format for an Archives
    /// </summary>
    ///
    /// <remarks>
    /// Created to assist with archive formats
    /// </remarks>
    public abstract class Archive: FileType
    {
        protected Archive(IList<byte[]> signatures, string mediaType, string extension) : base(signatures, mediaType, extension)
        {
        }

        protected Archive(IList<byte[]> signatures, string mediaType, string extension, int offset) : base(signatures, mediaType, extension, offset)
        {
        }

        protected Archive(IList<byte[]> signatures, string mediaType, string extension, int offset, string name) : base(signatures, mediaType, extension, offset, name)
        {
        }

        protected Archive(IList<byte[]> signatures, string mediaType, string extension, int offset, string name, string software) : base(signatures, mediaType, extension, offset, name, software)
        {
        }
    }
}