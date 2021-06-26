using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using System;
using System.IO;
using System.Net;

namespace Pamaxie.Leecher.Image_Prep
{
    public static class ImagePreparation
    {
        private static readonly string TempImageDirectory =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Pamaxie\\temp\\";

        /// <summary>
        /// Overload for FileStream
        /// </summary>
        /// <param name="fileLocation"></param>
        /// <returns></returns>
        public static FileInfo PrepareFile(string fileLocation)
        {
            //If the stream is empty just return the file as null.
            Guid imageNumber = Guid.NewGuid();
            string fileName = $"{TempImageDirectory}\\{imageNumber}.jpg";
            Image img = Image.Load(Configuration.Default, fileLocation);
            img.Mutate(x => x
                .Resize(400, 400)
                .Grayscale());
            img.Save(fileName);

            FileInfo file = new(fileLocation);
            file.Delete();
            file = new FileInfo(fileName);
            return file;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="downloadLocation"></param>
        /// <returns></returns>
        #nullable enable
        public static FileInfo? DownloadFile(string downloadLocation)
        {
            Guid imageNumber = Guid.NewGuid();
            WebRequest req = WebRequest.Create(downloadLocation);
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
            FileInfo? file = new(fileName);
            return file;
        }
    #nullable disable
    }
}
