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
        /// Stores the file types that we created
        /// </summary>
        private static readonly List<FileType> _fileTypes = new List<FileType>();

        /// <summary>
        /// Initialises the Format inspector with the included file formats
        /// </summary>
        static FileDetection()
        {
            AddFileTypes(null);
        }

        /// <summary>
        /// Add a number of file types to the list
        /// </summary>
        /// <param name="fileTypes"></param>
        public static void AddFileTypes(IList<FileType> fileTypes)
        {
            var types = FileTypeLocator.GetFileTypes();
            _fileTypes.AddRange(types);
            if (fileTypes == null || fileTypes.Count == 0) return;
            _fileTypes.AddRange(fileTypes);
        }

        /// <summary>
        /// Detects the Filetype from a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">Thrown is stream was reached in or if it was null</exception>
        /// <exception cref="ArgumentException">Thrown if the stream was not seekable or empty</exception>
        public static FileType DetermineFileType(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream),
                    "We require a stream to scan. Please ensure it is not null.");
            }

            //Attempt to make stream seekable if it is not already.
            if (stream.CanRead && !stream.CanSeek)
            {
                var ms = new MemoryStream();
                stream.CopyTo(ms);
                ms.Position = 0;
                stream = ms;
            }

            //Stream is still not seekable. just throw an exception at this point
            if (!stream.CanSeek)
            {
                throw new ArgumentException("The passed stream is not seekable and couldn't be made seekable automatically or empty");
            }

            if (stream.Length == 0)
            {
                return null;
            }
            
            var fileTypeMatches = stream.FindFileTypes();

            if (fileTypeMatches.Count > 1)
            {
                RemoveBaseTypes(fileTypeMatches);
            }

            if (fileTypeMatches.Count > 0)
            {
                return fileTypeMatches.OrderByDescending(t => t.Signatures.OrderByDescending(x => x.Count)).FirstOrDefault();
            }

            return null;
        }

        /// <summary>
        /// Determine the possible file formats of a stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static List<FileType> FindFileTypes(this Stream stream)
        {
            var types = new List<FileType>();
            
            foreach (var type in _fileTypes.OrderBy(t => t.Signatures.OrderBy(x => x.Count)).ToList())
            {
                if (!type.IsMatch(stream)) continue;
                types.Add(type);
            }

            stream.Position = 0;
            return types;
        }

        private static void RemoveBaseTypes(List<FileType> candidates)
        {
            for (var i = 0; i < candidates.Count; i++)
            {
                for (var j = 0; j < candidates.Count; j++)
                {
                    if (i != j && candidates[j].GetType().IsAssignableFrom(candidates[i].GetType()))
                    {
                        candidates.RemoveAt(j);
                        i--;
                        j--;
                    }
                }
            }
        }
    }
}