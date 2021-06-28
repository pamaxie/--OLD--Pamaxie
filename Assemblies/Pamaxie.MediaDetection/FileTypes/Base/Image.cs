using System.Collections.Generic;

namespace Pamaxie.MediaDetection.FileTypes.Base
{
    /// <summary>
    /// This is a specific format for an Image
    /// </summary>
    ///
    /// <remarks>
    /// Created to assist with image formats
    /// </remarks>
    public abstract class Image : FileType
    {
        protected Image(IList<byte[]> signatures, string mediaType, string extension) : base(signatures, mediaType, extension)
        {
        }

        protected Image(IList<byte[]> signatures, string mediaType, string extension, int offset) : base(signatures, mediaType, extension, offset)
        {
        }

        protected Image(IList<byte[]> signatures, string mediaType, string extension, int offset, string name) : base(signatures, mediaType, extension, offset, name)
        {
        }

        protected Image(IList<byte[]> signatures, string mediaType, string extension, int offset, string name, string software) : base(signatures, mediaType, extension, offset, name, software)
        {
        }
    }
}