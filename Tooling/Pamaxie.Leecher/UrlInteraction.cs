using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Pamaxie.Leecher.Image_Prep;

namespace Pamaxie.Leecher
{
    public class UrlInteraction
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
                using System.Net.WebClient client = new System.Net.WebClient();
                if (!UrlExists(url, out url)) return string.Empty;
                var htmlContent = client.DownloadString(url);
                return GetTextsFromHtml(htmlContent);
            }
            catch (WebException)
            {
                return string.Empty;
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
            redirectUri = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            req.AllowAutoRedirect = true;
            //Ignore certificate errors.
            req.ServerCertificateValidationCallback += (sender, certificate, chain, errors) =>
            {
                // local dev, just approve all certs
                return errors == SslPolicyErrors.None;
            };
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            redirectUri = res.ResponseUri.AbsoluteUri;
            if (res.StatusCode == HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Gets the Text from a HTML formatted string
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string GetTextsFromHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                return "";
            var htmlDoc = new HtmlDocument();
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
            foreach (var node in nodes)
            {
                if (node.Name.ToLowerInvariant() == "style")
                    continue;
                if (node.HasChildNodes)
                {
                    texts = texts + GetTextsFromNode(node.ChildNodes);
                }
                else
                {
                    var innerText = node.InnerText;
                    if (!string.IsNullOrWhiteSpace(innerText))
                    {
                        if (node.Name.ToLowerInvariant() == "span")

                            texts = texts + " " + node.InnerText + "\n";
                        else
                            texts = texts + node.InnerText;
                    }
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
            var document = new HtmlWeb().Load(url);

            // now using LINQ to grab/list all images from website
            var imageUrls = document.DocumentNode.Descendants("img")
                .Select(e => e.GetAttributeValue("src", null))
                .Where(s => !String.IsNullOrEmpty(s)).ToArray();

            if (imageUrls.Length == 0) return;
            // now showing all images from web page one by one
            foreach (var item in imageUrls)
            {
                var file = ImagePreparation.DownloadFile(item);
                var preppedFile = ImagePreparation.PrepareFile(file?.FullName);
                preppedFile.MoveTo(Program.ImageDestinationDir + "/" + preppedFile.Name + "." + preppedFile.Extension);
            }
        }
    }
}
