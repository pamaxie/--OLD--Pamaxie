using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pamaxie.MediaDetection
{
    /// <summary>
    /// Class responsible for detecting the file type of a stream
    /// </summary>
    public static class FileDetection
    {
        /// <summary>
        /// Stores a sorted set of File Types
        /// </summary>
        private static readonly Dictionary<ulong, FileType> FileTypes = new Dictionary<ulong, FileType>();

        /// <summary>
        /// Stores a sorted set of File Specifications
        /// </summary>
        private static readonly SortedSet<FileSpecification> FileSpecifications = new SortedSet<FileSpecification>();

        /// <summary>
        /// Initialises the Format inspector with the included file formats
        /// </summary>
        static FileDetection()
        {
            AddFileTypes(null);
            AddFileSpecifications(null);
        }

        /// <summary>
        /// Detects the Filetype from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown is stream was reached in or if it was null</exception>
        /// <exception cref="ArgumentException">Thrown if the stream was not seekable or empty</exception>
        public static KeyValuePair<FileSpecification, FileType>? DetermineFileType(this Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream),
                    "We require a stream to scan. Please ensure it is not null.");

            //Attempt to make stream seekable if it is not already.
            if (stream.CanRead && !stream.CanSeek)
            {
                MemoryStream ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Position = 0;
                stream = ms;
            }

            //Stream is still not seekable. just throw an exception at this point
            if (!stream.CanSeek)
                throw new ArgumentException(
                    "The passed stream is not seekable and couldn't be made seekable automatically or empty");

            if (stream.Length == 0)
                return null;

            SortedSet<FileSpecification> fileTypeMatches = stream.FindFileTypes();
            fileTypeMatches.Reverse();

            foreach (FileSpecification item in fileTypeMatches.Select(t => fileTypeMatches.FirstOrDefault())
                .Where(item => item != null))
            {
                (_, FileType value) = FileTypes.FirstOrDefault(x => x.Key == item.ReferenceTypeId);
                if (value != null)
                    return new KeyValuePair<FileSpecification, FileType>(item, value);
            }

            return null;
        }

        /// <summary>
        /// Add a number of file types to the list
        /// </summary>
        /// <param name="fileTypes"></param>
        private static void AddFileTypes(IList<FileType> fileTypes)
        {
            List<FileType> types = fileTypes != null
                ? fileTypes.Concat(FileTypeLocator.GetFileTypes()).ToList()
                : FileTypeLocator.GetFileTypes().ToList();

            foreach (FileType type in types)
            {
                if (type.Id == default)
                    throw new Exception(
                        "We could find a filetype without an Id being defined, this should never happen");
                FileTypes.Add(type.Id, type);
            }
        }

        /// <summary>
        /// Add a list of specifications 
        /// </summary>
        /// <param name="specifications"></param>
        private static void AddFileSpecifications(IList<FileSpecification> specifications)
        {
            if (FileSpecifications.Any())
                specifications = specifications.Concat(FileSpecifications).ToList();
            else if (specifications != null)
                specifications = specifications.Concat(FileSpecLocator.GetFileSpecs()).ToList();
            else
                specifications = FileSpecLocator.GetFileSpecs().ToList();

            specifications = specifications.OrderBy(x => x.Signature.Length).ToList();
            if (specifications.Any(fileSpecification => !FileSpecifications.Add(fileSpecification)))
                throw new Exception("Failed adding one of the filetypes to list");
        }

        /// <summary>
        /// Determine the possible file specifications of a stream
        /// </summary>
        /// <param name="stream">The stream to find the specifications of</param>
        /// <returns></returns>
        private static SortedSet<FileSpecification> FindFileTypes(this Stream stream)
        {
            SortedSet<FileSpecification> fileTypes = new SortedSet<FileSpecification>();
            foreach (FileSpecification specification in FileSpecifications.ToList()
                .Where(specification => specification.IsMatch(stream)))
                fileTypes.Add(specification);

            stream.Position = 0;
            return fileTypes;
        }
    }
}