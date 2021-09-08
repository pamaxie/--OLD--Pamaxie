using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Pamaxie.MediaDetection;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace Pamaxie.ImageProcessing
{
    public static class ImageProcessing
    {
        private static readonly string TempImageDirectory = Path.GetTempPath();

        /// <summary>
        /// Downloads a file to the local cache directory
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <returns></returns>
#nullable enable
        public static FileInfo DownloadFile(string downloadUrl)
        {
            Guid imageNumber = Guid.NewGuid();
            WebRequest req = WebRequest.Create(downloadUrl);
            HttpWebRequest request = (HttpWebRequest)req;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            KeyValuePair<FileSpecification, FileType>? spec = stream.DetermineFileType();
            if (spec == null)
                throw new ArgumentException("Unknown filetype detected. Can't analyze filetypes without a specification");

            //var fileFormat = FileDetection.DetermineFileType(stream);
            string fileName = $"{TempImageDirectory}\\{imageNumber}.jpg";

            Image img = Image.Load(Configuration.Default, stream);
            img.Mutate(x => x
                 .Resize(400, 400)
                 .Grayscale());
            img.Save(fileName);
            stream.Close();
            FileInfo file = new(fileName);
            return file;
        }
#nullable disable

        /// <summary>
        /// Get the file hash of a url file
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetFileHash(string url)
        {
            WebRequest req = WebRequest.Create(url);
            HttpWebRequest request = (HttpWebRequest)req;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            return await GetHashAsync<SHA256CryptoServiceProvider>(stream);
        }

        private static async Task<string> GetHashAsync<T>(Stream stream) where T : HashAlgorithm, new()
        {
            using T algo = new();
            byte[] buffer = new byte[8192];
            int bytesRead;

            // compute the hash on 8KiB blocks
            while ((bytesRead = await stream.ReadAsync(buffer.AsMemory(0, buffer.Length))) != 0)
                algo.TransformBlock(buffer, 0, bytesRead, buffer, 0);
            algo.TransformFinalBlock(buffer, 0, bytesRead);

            // build the hash string
            StringBuilder sb = new(algo.HashSize / 4);
            if (algo.Hash != null)
                foreach (byte b in algo.Hash)
                    sb.AppendFormat("{0:x2}", b);
            return sb.ToString();
        }
    }
}
