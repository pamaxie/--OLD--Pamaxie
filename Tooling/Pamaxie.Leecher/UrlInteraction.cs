using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using HtmlAgilityPack;
using Pamaxie.Leecher.Image_Prep;
using static System.String;

namespace Pamaxie.Leecher
{
    public static class UrlInteraction
    {
        /// <summary>
        /// Grabs all the plaintext from a Url
        /// </summary>
        /// <param name="url">Url to grab plaintext from</param>
        /// <returns></returns>
        public static string GrabText(string url)
        {
            try
            {
                using WebClient client = new();
                if (!UrlExists(url, out url)) return Empty;
                string htmlContent = client.DownloadString(url);
                return GetTextsFromHtml(htmlContent);
            }
            catch (WebException)
            {
                return Empty;
            }
        }

        /// <summary>
        /// Checks if an URL exists really
        /// </summary>
        /// <param name="url"></param>
        /// <param name="redirectUri"></param>
        /// <returns></returns>
        public static bool UrlExists(string url, out string redirectUri)
        {
            redirectUri = Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.AllowAutoRedirect = true;
            //Ignore certificate errors.
            req.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => errors == SslPolicyErrors.None;
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            redirectUri = res.ResponseUri.AbsoluteUri;
            return res.StatusCode == HttpStatusCode.OK;
        }

        /// <summary>
        /// Gets the Text from a HTML formatted string
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string GetTextsFromHtml(string html)
        {
            if (IsNullOrEmpty(html)) return "";
            HtmlDocument htmlDoc = new();
            htmlDoc.LoadHtml(html);
            return GetTextsFromNode(htmlDoc.DocumentNode.ChildNodes);
        }

        /// <summary>
        /// Iterates over each node to extract the plaintext from them.
        /// </summary>
        /// <param name="nodes"><see cref="HtmlNodeCollection"/> of nodes to load from</param>
        /// <returns></returns>
        private static string GetTextsFromNode(HtmlNodeCollection nodes)
        {
            string texts = "";
            foreach (HtmlNode node in nodes)
            {
                if (node.Name.ToLowerInvariant() == "style") continue;
                if (node.HasChildNodes)
                {
                    texts += GetTextsFromNode(node.ChildNodes);
                }
                else
                {
                    string innerText = node.InnerText;
                    if (IsNullOrWhiteSpace(innerText)) continue;
                    if (node.Name.ToLowerInvariant() == "span")

                        texts = texts + " " + node.InnerText + "\n";
                    else
                        texts += node.InnerText;
                }
            }

            return texts;
        }

        /// <summary>
        /// Grabs all Images from a url
        /// </summary>
        /// <param name="url"></param>
        public static void GrabAllImages(string url)
        {
            // declare html document
            HtmlDocument document = new HtmlWeb().Load(url);

            // now using LINQ to grab/list all images from website
            string[] imageUrls = document.DocumentNode.Descendants("img")
                .Select(e => e.GetAttributeValue("src", null))
                .Where(s => !IsNullOrEmpty(s)).ToArray();

            if (imageUrls.Length == 0) return;
            // now showing all images from web page one by one
            foreach (string item in imageUrls)
            {
                FileInfo file = ImagePreparation.DownloadFile(item);
                FileInfo preppedFile = ImagePreparation.PrepareFile(file?.FullName);
                preppedFile.MoveTo(Program.ImageDestinationDir + "/" + preppedFile.Name + "." + preppedFile.Extension);
            }
        }
    }
}
