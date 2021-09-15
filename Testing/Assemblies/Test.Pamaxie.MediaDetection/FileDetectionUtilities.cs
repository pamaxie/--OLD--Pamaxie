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
    internal static class FileDetectionUtilities
    {
        /// <summary>
        /// Get a <see cref="Stream"/> from a url string
        /// </summary>
        /// <param name="url">Url string that will be used to get the <see cref="Stream"/></param>
        /// <returns>Stream from url</returns>
        internal static Stream UrlToStream(string url)
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
        /// Get all <see cref="FileSpecification"/> from a file type
        /// </summary>
        /// <param name="fileType">The <see cref="FileType"/> to get <see cref="FileSpecification"/></param>
        /// <returns>A list of <see cref="FileSpecification"/> for the file type</returns>
        internal static IEnumerable<FileSpecification> GetFileSpecifications(this FileType fileType)
        {
            List<FileSpecification> fileSpecifications = new List<FileSpecification>();

            Type[] types =  fileType.GetType().Assembly.GetTypes();
            foreach (Type type in types)
            {
                if (typeof(FileSpecification).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                {
                    ConstructorInfo[] infos = type.GetConstructors();

                    foreach (ConstructorInfo info in infos)
                    {
                        if (info.GetParameters().Length == 0)
                        {
                            if (info.DeclaringType != null)
                            {
                                object instance  = Activator.CreateInstance(info.DeclaringType);
                                if (instance != null && ((FileSpecification)instance).ReferenceTypeId == fileType.Id)
                                {
                                    fileSpecifications.Add((FileSpecification)instance);
                                }
                            }
                        }
                    }
                }
            }

            return fileSpecifications;
        }

        /// <summary>
        /// Read exactly N bytes of stream
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> to read from</param>
        /// <param name="count">Amount of bytes to read</param>
        /// <param name="offset">Offset of stream position</param>
        /// <returns><see cref="byte"/> array</returns>
        internal static byte[] ReadExactly(this Stream stream, int count, int offset = 0)
        {
            byte[] buffer = new byte[count];
            stream.Position = 0;

            while (offset < count)
            {
                int read = stream.Read(buffer, offset, count - offset);

                if (read == 0)
                {
                    break;
                }

                offset += read;
            }

            return buffer;
        }

        /// <summary>
        /// Converts a byte array into a readable string of hex
        /// </summary>
        /// <param name="byteArr"><see cref="byte"/> array to convert into a string</param>
        /// <returns>Hex string</returns>
        internal static string ToHexString(this byte[] byteArr)
        {
            StringBuilder hex = new StringBuilder(byteArr.Length * 2);

            foreach (byte b in byteArr)
            {
                hex.AppendFormat("{0:x2} ", b);
            }

            return hex.ToString().ToUpper();
        }
    }
}