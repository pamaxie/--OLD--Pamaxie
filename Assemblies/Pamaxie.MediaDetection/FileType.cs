using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace Pamaxie.MediaDetection
{
    public abstract class FileType : IEquatable<FileType>
    {
        /// <summary>
        /// Initializes a new instance of the FileType class which has the specified signature and media type.
        /// </summary>
        /// <param name="signatures">The header signature of the type.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the format.</param>
        protected FileType(IList<byte[]> signatures, string mediaType, string extension) : this(signatures, mediaType, extension, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FileType class which has the specified signature and media type.
        /// </summary>
        /// <param name="signatures">The header signature of the type.</param>
        /// <param name="headerLength">The number of bytes required to determine the type.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the type.</param>
        /// <param name="offset">The offset at which the signature is located.</param>
        protected FileType(IList<byte[]> signatures, string mediaType, string extension, int offset) : this(
            signatures, mediaType, extension, offset, String.Empty, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FileType class which has the specified signature and media type.
        /// </summary>
        /// <param name="signatures">The header signature of the type.</param>
        /// <param name="headerLength">The number of bytes required to determine the type.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the type.</param>
        /// <param name="offset">The offset at which the signature is located.</param>
        /// <param name="name">The normally used name of the file format</param>
        protected FileType(IList<byte[]> signatures, string mediaType, string extension, int offset,
            string name) : this(signatures, mediaType, extension, offset, name, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the FileType class which has the specified signature and media type.
        /// </summary>
        /// <param name="signatures">The header signature of the type.</param>
        /// <param name="headerLength">The number of bytes required to determine the type. Determined via the <param name="signatures"></param> if zero.</param>
        /// <param name="mediaType">The media type of the type.</param>
        /// <param name="extension">The appropriate file extension for the type.</param>
        /// <param name="offset">The offset at which the signature is located.</param>
        /// <param name="name">The normally used name of the file format</param>
        /// <param name="software">Software examples that this format is normally used with</param>
        protected FileType(IList<byte[]> signatures, string mediaType, string extension, int offset,
            string name, string software)
        {
            if (signatures == null)
            {
                throw new ArgumentNullException(nameof(signatures),
                    "The Signature of the filetype cannot be null or empty, please enter a signature for the new FileType");
            }

            if (string.IsNullOrEmpty(mediaType))
            {
                throw new ArgumentNullException(nameof(mediaType),
                    "The type of media cannot be null, please enter a media type for the new FileType");
            }

            if (Signatures == null)
                Signatures = new List<ReadOnlyCollection<byte>>();
            
            foreach (var signature in signatures)
            {
                Signatures.Add(new ReadOnlyCollection<byte>(signature));
            }
            
            HeaderOffset = offset;
            Extension = extension;
            MediaType = mediaType;
            Name = name;
            Software = software;
        }

        /// <summary>
        /// Byte signature to identify the file type
        /// </summary>
        public IList<ReadOnlyCollection<byte>> Signatures { get; } = new List<ReadOnlyCollection<byte>>();

        /// <summary>
        /// The extension of the file format
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// The Name of the file format
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Software that supports this format
        /// </summary>
        public string Software { get; }

        /// <summary>
        /// The Type of media 
        /// </summary>
        public string MediaType { get; }

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
            //BUG: This needs to be rewritten completely

            var matched = false;
            
            foreach (var signature in Signatures)
            {
                if (stream == null || (stream.Length < signature.Count && signature.Count < int.MaxValue) ||
                    HeaderOffset > stream.Length)
                {
                    continue;
                }
                
                stream.Position = HeaderOffset;
                for (var i = 0; i < signature.Count; i++)
                {
                    var b = stream.ReadByte();
                    if (b == signature[i]) 
                        matched = true;
                    matched = false;
                    break;
                }

                matched = true;
            }

            return matched;
        }

        /// <summary>
        /// Gets the Hash Function of the Signature
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Signatures, Extension, Name, Software, MediaType, HeaderOffset);
        }

        /// <summary>
        /// Gets the Name of the filetype
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name)) return Name;
            if (!string.IsNullOrEmpty(Extension)) return Extension;
            if (string.IsNullOrEmpty(MediaType)) return MediaType;

            return string.Empty;
        }

        /// <summary>
        /// Determines if the format is equal to this FileFormat
        /// </summary>
        /// <param name="obj">The format to compare</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj is not FileType type) return false;
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
            if (GetType() != fileType.GetType()) return false;
            return fileType.Signatures.SequenceEqual(Signatures);
        }
    }
}