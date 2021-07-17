using Pamaxie.ImageCrawler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImageCrawler
{
    public static class Program
    {
        internal static string ImageDestinationDir;
        internal static string BaseUrl;
        private static int _maxImages;
        internal static int CurrentImgCount;
        internal static HashSet<string> DownloadedImageUrls = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        private static void Main(string[] args)
        {
            //Ask the user where to save images if they want to save them
            while (true)
            {
                Console.WriteLine(@"Please enter a destination for the images of websites to be copied to");
                string imageDestination = Console.ReadLine();
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
                string imageDestination = Console.ReadLine();
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
                string imageAmount = Console.ReadLine();
                bool result = int.TryParse(imageAmount, out int intImageAmount);
                if (result)
                {
                    _maxImages = intImageAmount;
                    break;
                }
                Console.WriteLine(@"Entered value is not a correct integer. Please make sure its a positive int");
            }

            List<string> links = new();
            Console.WriteLine("Doing an initial sweep to see how many links and other things we can find on the main page.");
            try
            {
                links = UrlInteraction.ParseLinks(BaseUrl);
                UrlInteraction.GrabAllImages(BaseUrl);
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(@"Download of images over http failed reattempting with https");
                Console.ForegroundColor = ConsoleColor.Black;
            }

            Parallel.ForEach(links, (link) =>
            {
                Queue<string> inputList = new();
                inputList.Enqueue(link);

                while(inputList.Any())
                {
                    if (CurrentImgCount == _maxImages) return;
                    string currentLink = inputList.Dequeue();
                    CrawlUrl(currentLink, inputList);
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
        private static void CrawlUrl(string url, Queue<string> inputList)
        {
            List<string> links = UrlInteraction.ParseLinks(url);
            foreach (string link in links.Where(link => !inputList.Contains(link)))
            {
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
        /// <param name="width"></param>
        /// <returns></returns>
        public static string GetProgress(double currentProgress, int width)
        {
            string progressBar = string.Empty;
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
