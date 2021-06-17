using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Image = SixLabors.ImageSharp.Image;

namespace PamaxieML.Api
{
    public class ImageProcessing
    {

        public static readonly string TempImageDirectory =
            System.IO.Path.GetTempPath();

        /// <summary>
        /// Overload for FileStream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static FileInfo PrepareFile(string fileLocation)
        {
            FileInfo file = null;

            //If the stream is empty just return the file as null.
            Guid imageNumber = Guid.NewGuid();
            string fileName = $"{TempImageDirectory}\\{imageNumber}.jpg";
            Image img = Image.Load(Configuration.Default, fileLocation);
            img.Mutate(x => x
                .Resize(400, 400)
                .Grayscale());
            img.Save(fileName);

            file = new FileInfo(fileLocation);
            file.Delete();
            file = new FileInfo(fileName);
            return file;
        }

        /// <summary>
        /// Download a file and return its bitmap
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <returns></returns>
        public static byte[] BitmapDownloadFile(string downloadUrl)
        {
            FileInfo? file = null;
            Guid imageNumber = Guid.NewGuid();
            WebRequest req = WebRequest.Create(downloadUrl);
            HttpWebRequest request = (HttpWebRequest)req;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            BufferedStream buffer = new BufferedStream(stream);
            return new byte[12];
        }


        /// <summary>
        /// Downloads a file to the local cache directory
        /// </summary>
        /// <param name="downloadUrl"></param>
        /// <returns></returns>
#nullable enable
        public static FileInfo? DownloadFile(string downloadUrl)
        {
            FileInfo? file = null;
            Guid imageNumber = Guid.NewGuid();
            WebRequest req = WebRequest.Create(downloadUrl);
            HttpWebRequest request = (HttpWebRequest)req;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:38.0) Gecko/20100101 Firefox/38.0";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            request.Headers.Add("Accept-Encoding", "gzip, deflate");
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();

            string? fileName = $"{TempImageDirectory}\\{imageNumber}.jpg";

            Image img = Image.Load(Configuration.Default, stream);
            img.Mutate(x => x
                 .Resize(400, 400)
                 .Grayscale());
            img.Save(fileName);
            stream.Close();
            file = new FileInfo(fileName);
            return file;
        }
#nullable disable


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

        public static async Task<string> GetHashAsync<T>(Stream stream)
where T : HashAlgorithm, new()
        {
            StringBuilder sb;

            using (var algo = new T())
            {
                var buffer = new byte[8192];
                int bytesRead;

                // compute the hash on 8KiB blocks
                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    algo.TransformBlock(buffer, 0, bytesRead, buffer, 0);
                algo.TransformFinalBlock(buffer, 0, bytesRead);

                // build the hash string
                sb = new StringBuilder(algo.HashSize / 4);
                foreach (var b in algo.Hash)
                    sb.AppendFormat("{0:x2}", b);
            }

            return sb?.ToString();
        }
    }
}
