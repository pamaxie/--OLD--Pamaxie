using System.Runtime.InteropServices;

namespace Pamaxie.MediaDetection
{
    public abstract class FileType : IFileType
    {
        /// <summary>
        /// Creates a new FileType for use in correspondence with FileSpec
        /// </summary>
        /// <param name="id">The Id of the object</param>
        /// <param name="name">The Name of the filetype (for example Portable Image Graphics)</param>
        /// <param name="extension">The extension of the filetype (for example .png)</param>
        /// <param name="software">The software that can be used to open the filetype</param>
        /// <param name="mediaType">The MediaType is a specific subtype specification</param>
        protected FileType(ulong id, string name, string extension, [Optional] string software,
            [Optional] string mediaType)
        {
            Id = id;
            Name = name;
            Extension = extension;
            Software = software;
            MediaType = mediaType;
        }

        public override string ToString() => Name;

        public ulong Id { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public string Software { get; set; }
        public string MediaType { get; set; }

        public bool Equals(IFileType fileType)
        {
            if (fileType == null) return false;
            if (ReferenceEquals(this, fileType)) return true;
            if (GetType() != fileType.GetType()) return false;
            return Id != fileType.Id;
        }
    }
}