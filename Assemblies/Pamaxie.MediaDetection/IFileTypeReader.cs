#nullable enable
using System;
using System.IO;

namespace Pamaxie.MediaDetection
{
    public interface IFileTypeReader
    {
        /// <summary>
        /// Reads a stream and converts it between formats
        /// </summary>
        /// <param name="stream">The stream to read</param>
        /// <returns>The stream that should be converted</returns>
        IDisposable? Read(Stream stream);

        /// <summary>
        /// Returns if the type of the file is a match for the converted stream
        /// </summary>
        /// <param name="file">The stream to check</param>
        /// <returns></returns>
        bool IsMatch(IDisposable? file);
    }
}