using System;
using System.IO;

namespace Pamaxie.MediaDetection
{
    /// <inheritdoc />
    public abstract class FileSpecification : IFileSpecification
    {
        protected FileSpecification(ulong referenceTypeId, byte[] signature, int headerOffset = 0, string specialTypeIdentifier = "")
        {
            ReferenceTypeId = referenceTypeId;
            Signature = signature;
            HeaderOffset = headerOffset;
            SpecialTypeIdentifier = specialTypeIdentifier;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public string SpecialTypeIdentifier { get; set; }
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ulong ReferenceTypeId { get; set; }
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public byte[] Signature { get; set; }
        
        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public int HeaderOffset { get; set; }

        /// <summary>
        /// <inheritdoc cref="IFileSpecification.ToString" />
        /// </summary>
        public override string ToString()
        {
            return SpecialTypeIdentifier;
        }
        
        /// <summary>
        /// Checks if the stream matches the specification
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public bool IsMatch(Stream stream)
        {
            if (!stream.CanSeek || !stream.CanRead)
                return false;

            if (stream.Length < Signature.Length)
                return false;
            
            stream.Position = HeaderOffset;
            for (var i = 0; i < Signature.Length; i++)
            {
                int b = stream.ReadByte();
                if (b != Signature[i])
                    return false;
            }

            return true;
        }
        
        /// <summary>
        /// Compare two file specifications for sorting
        /// </summary>
        /// <param name="fileSpecification">The file specification to compare to this spec</param>
        /// <returns>The result of the comparison of the two file specs</returns>
        public int CompareTo(IFileSpecification fileSpecification)
        {
            if (fileSpecification.Signature == null)
                throw new ArgumentNullException(nameof(fileSpecification), "The signature of the filetype that should be compared was not defined");
            if (fileSpecification.Signature.Length == 0)
                throw new ArgumentException("Please make sure the signature for the compared file type is valid");

            return 1;
        }
        
    }
}