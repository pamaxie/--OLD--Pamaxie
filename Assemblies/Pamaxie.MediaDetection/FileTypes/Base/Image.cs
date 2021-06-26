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
        protected Image(byte[] signature, string mediaType, string extension) : base(signature, mediaType, extension)
        {
        }

        protected Image(byte[] signature, string mediaType, string extension, int offset) : base(signature, mediaType, extension, offset)
        {
        }

        protected Image(byte[] signature, int headerLength, string mediaType, string extension) : base(signature, headerLength, mediaType, extension)
        {
        }

        protected Image(byte[] signature, int headerLength, string mediaType, string extension, int offset) : base(signature, headerLength, mediaType, extension, offset)
        {
        }

        protected Image(byte[] signature, int headerLength, string mediaType, string extension, int offset, string name) : base(signature, headerLength, mediaType, extension, offset, name)
        {
        }

        protected Image(byte[] signature, int headerLength, string mediaType, string extension, int offset, string name, string software) : base(signature, headerLength, mediaType, extension, offset, name, software)
        {
        }
    }
}