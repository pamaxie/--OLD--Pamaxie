using System;

namespace Pamaxie.MediaDetection
{
    public interface IFileType : IEquatable<IFileType>
    {
        /// <summary>
        /// Unique ID we designated to the filetype
        /// </summary>
        ulong Id { get; set; }
        
        /// <summary>
        /// The commonly known name for the filetype 
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// The extension the filetype uses
        /// </summary>
        string Extension { get; set; }
        
        /// <summary>
        /// Software that can view or run files of this type
        /// </summary>
        string Software { get; set; }
        
        /// <summary>
        /// The specific media type and designation
        /// </summary>
        string MediaType { get; set; }
        
        /// <summary>
        /// Gets the name of the filetype
        /// </summary>
        /// <returns><see cref="Name"/></returns>
        string ToString();
    }
}