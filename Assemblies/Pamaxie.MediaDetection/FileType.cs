using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Pamaxie.MediaDetection
{
    public abstract class FileType : IEquatable<FileType>
    {
        /// <summary>
        /// Initializes a new instance of the FileFormat class which has the specified signature and media type.
        /// </summary>
        /// <param name="signature">The header signature of the type.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the format.</param>
        protected FileType(byte[] signature, string mediaType, string extension) : 
            this(signature, signature == null ? 0 : signature.Length, mediaType, extension, 0){ }

        /// <summary>
        /// Initializes a new instance of the FileType class which has the specified signature and media type.
        /// </summary>
        /// <param name="signature">The header signature of the type.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the type.</param>
        /// <param name="offset">The offset at which the signature is located.</param>
        protected FileType(byte[] signature, string mediaType, string extension, int offset) : 
            this(signature, signature == null ? offset : signature.Length + offset, mediaType, extension, offset) { }

        /// <summary>
        /// Initializes a new instance of the FileType class which has the specified signature and media type.
        /// </summary>
        /// <param name="signature">The header signature of the type.</param>
        /// <param name="headerLength">The number of bytes required to determine the type.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the format.</param>
        protected FileType(byte[] signature, int headerLength, string mediaType, string extension)
            : this(signature, headerLength, mediaType, extension, 0) { }

        /// <summary>
        /// Initializes a new instance of the FileType class which has the specified signature and media type.
        /// </summary>
        /// <param name="signature">The header signature of the type.</param>
        /// <param name="headerLength">The number of bytes required to determine the type.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the type.</param>
        /// <param name="offset">The offset at which the signature is located.</param>
        protected FileType(byte[] signature, int headerLength, string mediaType, string extension, int offset)
            : this(signature, headerLength, mediaType, extension, offset, String.Empty, String.Empty) { }
        
        /// <summary>
        /// Initializes a new instance of the FileType class which has the specified signature and media type.
        /// </summary>
        /// <param name="signature">The header signature of the type.</param>
        /// <param name="headerLength">The number of bytes required to determine the type.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the type.</param>
        /// <param name="offset">The offset at which the signature is located.</param>
        /// <param name="name">The normally used name of the file format</param>
        protected FileType(byte[] signature, int headerLength, string mediaType, string extension, int offset, string name)
            : this(signature, headerLength, mediaType, extension, offset, name, String.Empty) { }


        /// <summary>
        /// Initializes a new instance of the FileType class which has the specified signature and media type.
        /// </summary>
        /// <param name="signature">The header signature of the type.</param>
        /// <param name="headerLength">The number of bytes required to determine the type. Determined via the <param name="signature"></param> if zero.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the type.</param>
        /// <param name="offset">The offset at which the signature is located.</param>
        /// <param name="name">The normally used name of the file format</param>
        /// <param name="software">Software examples that this format is normally used with</param>
        protected FileType(byte[] signature, int headerLength, string mediaType, string extension, int offset,
            string name, string software)
        {
            if (signature == null || signature.Length == 0)
            {
                throw new ArgumentNullException(nameof(signature), "The Signature of the filetype cannot be null or empty, please enter a signature for the new FileType");
            }

            if (string.IsNullOrEmpty(mediaType))
            {
                throw new ArgumentNullException(nameof(mediaType), "The type of media cannot be null, please enter a media type for the new FileType");
            }

            if (headerLength == 0)
            {
                headerLength = signature.Length + offset;
            }
            
            Signature = new ReadOnlyCollection<byte>(signature);
            HeaderLength = headerLength;
            HeaderOffset = offset;
            Extension = extension;
            MediaType = mediaType;
            Name = name;
            Software = software;
        }
        
        /// <summary>
        /// Byte signature to identify the file type
        /// </summary>
        public ReadOnlyCollection<byte> Signature { get; }

        /// <summary>
        /// The extension of the file format
        /// </summary>
        public string Extension { get;  }
        
        /// <summary>
        /// The Name of the file format
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Software that supports this format
        /// </summary>
        public string Software { get; set; }
        
        /// <summary>
        /// The Type of media 
        /// </summary>
        public string MediaType { get; }
        
        /// <summary>
        /// The length of the FileType header
        /// </summary>
        public int HeaderLength { get; }
        
        /// <summary>
        /// The Offset of the Header
        /// </summary>
        public int HeaderOffset { get; }
        
        /// <summary>
        ///  Returns a value if the format matches the file header
        /// </summary>
        /// <returns></returns>
        public virtual bool IsMatch(Stream stream)
        {
            if (stream == null || (stream.Length < HeaderLength && HeaderLength < int.MaxValue) ||
                HeaderOffset > stream.Length)
            {
                return false;
            }

            stream.Position = HeaderOffset;

            for (int i = 0; i < Signature.Count; i++)
            {
                var b = stream.ReadByte();
                if (b == Signature[i])
                    return true;
                return false;
            }

            return true;
        }



        /// <summary>
        /// Gets the Hash Function of the Signature
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Signature, Extension, Name, Software, MediaType, HeaderLength, HeaderOffset);
        }

        /// <summary>
        /// Gets the Name of the filetype
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
                return Name;
            if (!string.IsNullOrEmpty(Extension))
                return Extension;
            if (string.IsNullOrEmpty(MediaType))
                return MediaType;

            return string.Empty;
        }
        
        /// <summary>
        /// Determines if the format is equal to this FileFormat
        /// </summary>
        /// <param name="obj">The format to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is not FileType type)
                return false;
            return Equals(type);
        }

        /// <summary>
        /// Determines if the format is equal to this FileFormat
        /// </summary>
        /// <param name="fileType">The format to compare</param>
        /// <returns></returns>
        public bool Equals(FileType fileType)
        {
            if (fileType == null) return false;
            if (ReferenceEquals(this, fileType)) return true;
            if (GetType() != fileType.GetType())
                return false;
            return fileType.Signature.SequenceEqual(Signature);
        }
    }
}