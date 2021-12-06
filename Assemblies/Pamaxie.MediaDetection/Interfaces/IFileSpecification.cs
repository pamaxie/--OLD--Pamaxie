using System;

namespace Pamaxie.MediaDetection
{
    public interface IFileSpecification : IComparable<IFileSpecification>
    {
        /// <summary>
        /// Special identifier of a file type
        /// </summary>
        string SpecialTypeIdentifier { get; set; }

        /// <summary>
        /// The Id of the File type this specification is referencing
        /// </summary>
        ulong ReferenceTypeId { get; set; }

        /// <summary>
        /// The Signature of the file specification
        /// </summary>
        byte[] Signature { get; set; }

        /// <summary>
        /// The Header offset of the <see cref="Signature"/>
        /// </summary>
        int HeaderOffset { get; set; }

        /// <summary>
        /// Gets <see cref="SpecialTypeIdentifier"/>
        /// </summary>
        /// <returns></returns>
        string ToString();
    }
}