using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pamaxie.MediaDetection
{
    /// <summary>
    /// Class responsible for detecting the file type of a stream
    /// </summary>
    public class FileDetection
    {
        /// <summary>
        /// Stores the file types that we created
        /// </summary>
        private readonly IEnumerable<FileType> _fileTypes;

        /// <summary>
        /// Initialises the Format inspector with the included file formats
        /// </summary>
        public FileDetection() : this(FileTypeLocator.GetFileTypes()) { }

        public FileDetection(IEnumerable<FileType> fileTypes)
        {
            _fileTypes = fileTypes ?? throw new ArgumentNullException(nameof(fileTypes), "File types cannot be null");
        }

        /// <summary>
        /// Detects the Filetype from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown is stream was reached in or if it was null</exception>
        /// <exception cref="ArgumentException">Thrown if the stream was not seekable or empty</exception>
        public FileType DetermineFileType(Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream),
                    "We require a stream to scan. Please ensure it is not null.");
            }

            if (!stream.CanSeek || stream.Length == 0)
            {
                throw new ArgumentException("The passed stream is not seekable or empty");
            }

            var fileTypeMatches = FindFileTypes(stream);

            if (fileTypeMatches.Count > 1)
            {
                RemoveBaseTypes(fileTypeMatches);
            }

            if (fileTypeMatches.Count > 0)
            {
                return fileTypeMatches.OrderByDescending(t => t.HeaderLength).FirstOrDefault();
            }

            return null;
        }


        /// <summary>
        /// Determine the possible file formats of a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal List<FileType> FindFileTypes(Stream stream)
        {
            var types = _fileTypes.OrderBy(t => t.HeaderLength).ToList();
            for (int i = 0; i < types.Count; i++)
            {
                if (!types[i].IsMatch(stream))
                {
                    types.RemoveAt(i);
                    i--;
                }
            }

            if (types.Count > 1)
            {
                var fileReaders = types.OfType<IFileTypeReader>().ToList();

                if (fileReaders.Any())
                {
                    var file = fileReaders[0].Read(stream);
                    foreach (var reader in fileReaders)
                    {
                        if (!reader.IsMatch(file))
                        {
                            types.Remove((FileType) reader);
                        }
                    }
                }
            }

            stream.Position = 0;
            return types;
        }

        private static void RemoveBaseTypes(List<FileType> canidates)
        {
            for (var i = 0; i < canidates.Count; i++)
            {
                for (var j = 0; j < canidates.Count; j++)
                {
                    if (i != j && canidates[j].GetType().IsAssignableFrom(canidates[i].GetType()))
                    {
                        canidates.RemoveAt(j);
                        i--;
                        j--;
                    }
                }
            }
        }
        
    }
}