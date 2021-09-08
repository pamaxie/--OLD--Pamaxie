using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using Pamaxie.MediaDetection;
using Xunit;

namespace Test.Pamaxie.MediaDetection_Test
{
    /// <summary>
    /// Testing utilities for MediaDetection testing
    /// </summary>
    public static class FileDetectionUtilities
    {
        /// <summary>
        /// Get a <see cref="Stream"/> from a url string
        /// </summary>
        /// <param name="url">Url string that will be used to get the <see cref="Stream"/></param>
        /// <returns>Stream from url</returns>
        public static Stream UrlToStream(string url)
        {
            WebRequest req = WebRequest.Create(url);
            HttpWebRequest request = (HttpWebRequest)req;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            WebResponse response = request.GetResponse();
            Stream stream = new MemoryStream();
            using Stream responseStream = response.GetResponseStream();
            Assert.NotNull(responseStream);
            responseStream.CopyTo(stream);
            return stream;
        }
        
        /// <summary>
        /// Get all specifications from a file type
        /// </summary>
        /// <param name="fileType">The file type specifications</param>
        /// <returns>A list of specifications for the file type</returns>
        public static IEnumerable<FileSpecification> GetFileSpecifications(this FileType fileType)
        {
            return fileType.GetType().Assembly.GetTypes()
                .Where(t => typeof(FileSpecification).IsAssignableFrom(t))
                .Where(t => !t.GetTypeInfo().IsAbstract)
                .Where(t => t.GetConstructors().Any(c => c.GetParameters().Length == 0))
                .Select(Activator.CreateInstance)
                .OfType<FileSpecification>().Where(_ => _.ReferenceTypeId == fileType.Id);
        }
        
        /// <summary>
        /// Read exactly N bytes of stream
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> to read from</param>
        /// <param name="count">Amount of bytes to read</param>
        /// <param name="offset">Offset of stream position</param>
        /// <returns><see cref="byte"/> array</returns>
        public static byte[] ReadExactly(this Stream stream, int count, int offset = 0)
        {
            byte[] buffer = new byte[count];
            stream.Position = 0;
            
            while (offset < count)
            {
                int read = stream.Read(buffer, offset, count - offset);
                if (read == 0)
                    break;
                offset += read;
            }

            return buffer;
        }
        
        /// <summary>
        /// Converts a byte array into a readable string of hex
        /// </summary>
        /// <param name="byteArr"><see cref="byte"/> array to convert into a string</param>
        /// <returns>Hex string</returns>
        public static string ToHexString(this byte[] byteArr)
        {
            StringBuilder hex = new StringBuilder(byteArr.Length * 2);
            foreach (byte b in byteArr)
                hex.AppendFormat("{0:x2} ", b);
            return hex.ToString().ToUpper();
        }
    }
}