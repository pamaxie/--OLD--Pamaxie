using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Pamaxie.ImageCrawler;

namespace ImageCrawler
{
    class Program
    {
        public static string ImageDestinationDir;
        public static string BaseUrl;
        public static int MaxImages;
        public static int CurrentImgCount;
        public static HashSet<string> DownloadedImageUrls = new HashSet<string>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            //Ask the user where to save images if they want to save them
            while (true)
            {
                Console.WriteLine(@"Please enter a destination for the images of websites to be copied to");
                var imageDestination = Console.ReadLine();
                if (Directory.Exists(imageDestination))
                {
                    ImageDestinationDir = imageDestination;
                    break;
                }

                Console.WriteLine(@"File could not be found please validate it exists");
            }

            //Get the Url to crawl
            while (true)
            {
                Console.WriteLine(@"Please enter a url to crawl");
                var imageDestination = Console.ReadLine();
                bool result = Uri.TryCreate(imageDestination, UriKind.Absolute, out _);

                if (result)
                {
                    BaseUrl = imageDestination;
                    break;
                }

                Console.WriteLine(@"Url could not be validated and is probably incorrect.");
            }

            //Get the Url to crawl
            while (true)
            {
                Console.WriteLine(@"Please enter the maximum amount of images to download");
                var imageAmount = Console.ReadLine();
                bool result = int.TryParse(imageAmount, out var intImageAmount);

                if (result)
                {
                    MaxImages = intImageAmount;
                    break;
                }

                Console.WriteLine(@"Entered value is not a correct integer. Please make sure its a positive int");
            }

            List<string> links = new List<string>();
            Console.WriteLine("Doing an initial sweep to see how many links and other things we can find on the main page.");
            try
            {
                links = UrlInteraction.ParseLinks(BaseUrl);
                UrlInteraction.GrabAllImages(BaseUrl);
            }
            catch (Exception ex)
            {

                Console.WriteLine(@"Download of images over http failed reattempting with https",
                    Color.Red);
            }

            Parallel.ForEach(links, (link) =>
            {
                Queue<string> inputList = new Queue<string>();
                inputList.Enqueue(link);

                while(inputList.Any())
                {
                    if (CurrentImgCount == MaxImages) return;
                    var currentlink = inputList.Dequeue();
                    CrawlUrl(currentlink, inputList);
                }
            });

            Console.WriteLine("Done the work");
            Console.ReadKey();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="inputList">THIS VALUE IS MODIFIED IN HERE!!!</param>
        public static void CrawlUrl(string url, Queue<string> inputList)
        {
            var links = UrlInteraction.ParseLinks(url);
            foreach (var link in links)
            {
                if (inputList.Contains(link)) continue;
                inputList.Enqueue(link);
            }
            UrlInteraction.GrabAllImages(url);
        }


        /// <summary>
        /// Draws the overall progress of scraping the website content.
        /// </summary>
        public static void DrawOverallProgress()
        {
            if (CurrentImgCount == 0) return;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;

        }


        /// <summary>
        /// Displays Progress
        /// </summary>
        /// <param name="currentProgress"></param>
        /// <returns></returns>
        public static string GetProgress(double currentProgress, int width)
        {
            var progressBar = string.Empty;

            for (int i = 0; i < width; i++)
            {
                if (currentProgress * 100 / width > 1)
                {
                    progressBar += "█";
                    currentProgress--;
                    continue;
                }

                progressBar += "░";
            }

            progressBar += string.Empty;

            return progressBar;
        }
    }
}
