using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;
using DnsClient;
using HtmlAgilityPack;
using Pamaxie.Leecher.Database;
using Pamaxie.Leecher.Image_Prep;

namespace Pamaxie.Leecher
{
    class Program
    {
        public static string OriginFile;
        public static string ImageDestinationDir;
        public static string Type;
        public static int OverallUrls;
        public static int CurrentUrlIdx;
        public static int FailedUrls = 0;
        public static int WorkerThreads = 10;
        public static bool ShowErrors = false;
        public static List<Thread> WorkerThreadList;
        public static Stopwatch ProgressStopwatch;
        public static string SweepingStep;

        static void Main(string[] args)
        {
            //Ask the user for a plaintext file to load the urls from
            while (true)
            {
                Console.WriteLine(@"Please enter a file including path to read urls from");
                var originFile = Console.ReadLine();
                if (File.Exists(originFile))
                {
                    OriginFile = originFile;
                    break;
                }

                Console.WriteLine(@"File could not be found please validate it exists");
            }

            //Ask the user if they want to clear out the database
            while (true)
            {
                Console.WriteLine(@"Clear out database before continuing? THIS CAN NOT BE UNDONE! (y/N)");
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y:
                        using (var dbcontext = new SqlDbContext())
                        {
                            var items = dbcontext.Label.ToList();
                            dbcontext.RemoveRange(items);
                            dbcontext.SaveChanges();
                        }
                        break;
                    case ConsoleKey.N:
                        break;
                    case ConsoleKey.Enter:
                        break;
                    default:
                        Console.WriteLine("Unexpected input detected please try again.");
                        continue;
                }

                break;
            }

            //Ask the User if they want to have error messages displayed or not
            while (true)
            {
                Console.WriteLine(
                    @"Display error messages (These may be unreadable because our Scraper is quite quick on his legs)? (y/N)");
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y:
                        ShowErrors = true;
                        break;
                    case ConsoleKey.Enter:
                        break;
                    default:
                        Console.WriteLine("Unexpected input detected please try again.");
                        continue;
                }

                break;
            }

            //Ask a user for a type of the url
            while (true)
            {
                Console.WriteLine("Please enter a Url type for the domains. The list of Urls Types are:\n" +
                                  "1: adult\n" +
                                  "2: marketing\n" +
                                  "3: banking\n" +
                                  "4: hacking\n" +
                                  "5: mixed-adult\n" +
                                  "6: advertising\n" +
                                  "7: other\n" +
                                  "Options 1-7: \n");
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.D1:
                        Type = "adult";
                        break;
                    case ConsoleKey.D2:
                        Type = "marketing";
                        break;
                    case ConsoleKey.D3:
                        Type = "banking";
                        break;
                    case ConsoleKey.D4:
                        Type = "hacking";
                        break;
                    case ConsoleKey.D5:
                        Type = "mixed-adult";
                        break;
                    case ConsoleKey.D6:
                        Type = "advertising";
                        break;
                    case ConsoleKey.D7:
                        while (true)
                        {
                            var type = Console.ReadLine();
                            if (string.IsNullOrEmpty(type))
                            {
                                Console.WriteLine("Type can not be empty please enter a valid type.");
                                continue;
                            }

                            Console.WriteLine($"Do you want to set the type to: {type}? (Y/n)");
                            var typeConfirm = Console.ReadKey();
                            switch (typeConfirm.Key)
                            {
                                case ConsoleKey.Y:
                                    Type = type;
                                    break;
                                case ConsoleKey.Enter:
                                    Type = type;
                                    break;
                                case ConsoleKey.N:
                                    type = string.Empty;
                                    break;
                                default:
                                    Console.WriteLine("Unexpected input detected please try again.");
                                    continue;
                            }
                        }

                        break;
                    default:
                        Console.WriteLine("Unknown type please enter a type from the list above");
                        continue;
                }

                break;
            }

            //Define the Thread to create to work on the url scraping.
            while (true)
            {
                Console.WriteLine(
                    "Please enter an amount of Worker-Threads to use. Please remember that we do not recommend this to exceed your actual Threads on your machine.\n");
                if (!(int.TryParse(Console.ReadLine(), out var num)))
                {
                    Console.WriteLine("Invalid number entered. Please enter a non floating point integer number");
                }

                WorkerThreads = num;
                break;
            }

            //Validate settings exist
            if (OriginFile == null)
            {
                Console.WriteLine(@"Hit unexpected problem. Please try again.");
                return;
            }

            var text = File.ReadAllLines(OriginFile).ToList();
            OverallUrls = text.Count;
            switch (OverallUrls)
            {
                //Has less than 1 Url so basically the file is empty.
                case < 1:
                    Console.WriteLine("No text found in file. Press any key to exit...");
                    Console.ReadKey();
                    Environment.Exit(1);
                    break;

                //Ask user if they want to attempt to separate the data via CSV separation cause 1 URL seems quite low...
                case 1:
                {
                    var attemptCsvSeperation = false;
                    while (true)
                    {
                        Console.WriteLine("Only found a single line attempt to separate over commas (CSV File)? (Y/n)");
                        var doCsvConfirm = Console.ReadKey();
                        switch (doCsvConfirm.Key)
                        {
                            case ConsoleKey.Y:
                                attemptCsvSeperation = true;
                                break;
                            case ConsoleKey.N:
                                Console.WriteLine("Not doing CSV separation and continuing with scraping progress..");
                                break;
                            case ConsoleKey.Enter:
                                break;
                            default:
                                Console.WriteLine("Unexpected input detected please try again.");
                                continue;
                        }

                        break;
                    }

                    if (!attemptCsvSeperation)
                    {
                        Console.WriteLine(
                            "Attempting to separate over commas. We do not recommend feeding CSV files into here.");
                        List<string> newTextList = new List<string>();
                        foreach (var line in text.ToList())
                        {
                            newTextList.AddRange(line.Split(","));
                        }

                        while (true)
                        {
                            Console.WriteLine(
                                $"Found: {newTextList.Count} lines after separation. Continue or exit to re-validate data? (Y/n)");
                            var doCsvConfirm = Console.ReadKey();
                            switch (doCsvConfirm.Key)
                            {
                                case ConsoleKey.Y:
                                    Console.WriteLine("Press any key to exit...");
                                    Console.ReadKey();
                                    Environment.Exit(1);
                                    break;
                                case ConsoleKey.N:
                                    Console.WriteLine($"Continuing with {newTextList.Count} Urls");
                                    break;
                                case ConsoleKey.Enter:
                                    Console.WriteLine($"Continuing with {newTextList.Count} Urls");
                                    break;
                                default:
                                    Console.WriteLine("Unexpected input detected please try again.");
                                    continue;
                            }

                            break;
                        }
                    }

                    break;
                }
            }

            bool skipSweep = false;
            //Ask the user if they want to do a sweeping of Urls before grabbing their content images as well
            while (true)
            {
                Console.WriteLine(@"Sweep urls (Validate their Records exist and they are reachable) before Scraping their content? This results in less errors but makes scraping on large lists a lot slower... (n/Y)");
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y:
                        break;
                    case ConsoleKey.N:
                        skipSweep = true;
                        break;
                    case ConsoleKey.Enter:
                        break;
                    default:
                        Console.WriteLine("Unexpected input detected please try again.");
                        continue;
                }

                break;
            }

            var backgroundThreadRunning = true;
            Thread uiThread;
            var sweepedUrls = new List<string>();
            if (!skipSweep)
            {
                ProgressStopwatch = new Stopwatch();
                ProgressStopwatch.Start();
                


                Console.WriteLine("Starting multi step sweep");
                
                for (int i = 0; i < 6; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }

                uiThread = new Thread((() =>
                {
                    Console.Clear();
                    while (backgroundThreadRunning)
                    {
                        DrawSweepProgress();
                    }
                }));
                uiThread.Start();

                //Check if DNS records exist
                SweepingStep = "checking each domains DNS Records";
                Parallel.ForEach(text, (domain) =>
                {
                    var isIp = false;
                    CurrentUrlIdx++;

                    if (IPAddress.TryParse(domain, out _))
                        isIp = true;

                    if (!isIp)
                    {
                        try
                        {
                            var client = new LookupClient();

                            var hostEntry = client.GetHostEntry(domain);

                            //Host has no DNS entries thus it will not be looked up.
                            if (hostEntry == null || hostEntry.AddressList.Length < 1)
                            {
                                FailedUrls++;
                                return;
                            }

                            //Add if there are host entries
                            sweepedUrls.Add(domain);
                        }
                        catch (DnsResponseException)
                        {
                            FailedUrls++;
                        }

                    }

                    //Add if there are no host entries.
                    sweepedUrls.Add(domain);
                });

                //Reset progress for next step
                text.Clear();
                text = sweepedUrls;
                OverallUrls = text.Count;
                CurrentUrlIdx = 0;
                FailedUrls = 0;

                //Check if Domains can be reached over http or https
                SweepingStep = "checking which domains are reachable";
                Parallel.ForEach(text, (domain) =>
                {
                    var exists = false;
                    CurrentUrlIdx++;

                    try
                    {
                        exists = UrlInteraction.UrlExists($"http://{domain}", out var resUri);
                        if (exists)
                        {
                            sweepedUrls.Add(resUri);
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    FailedUrls++;
                });
                ProgressStopwatch.Stop();
                backgroundThreadRunning = false;

                //Waiting for threads to join again
                Thread.Sleep(500);
                Console.WriteLine("Starting scraping now");
                for (int i = 0; i < 6; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(500);
                }
                Thread.Sleep(500);

                //Reset out Progress and the Worker Thread List
                ProgressStopwatch.Reset();
                CurrentUrlIdx = 0;
                OverallUrls = sweepedUrls.Count;
                text.Clear();
                text.AddRange(sweepedUrls);
            }
            else
            {
                foreach (var url in text)
                {
                    sweepedUrls.Add($"http://{url}");
                }

                ProgressStopwatch = new Stopwatch();
                ProgressStopwatch.Start();
            }

            OverallUrls = sweepedUrls.Count;
            var lists = new List<List<string>>();
            var listSize = OverallUrls / WorkerThreads;
            if (listSize == 0) listSize = 1;
            WorkerThreadList = new List<Thread>();
            lists.Clear();


            for (var i = 0; i < OverallUrls; i += listSize)
            {
                lists.Add(sweepedUrls.GetRange(i, Math.Min(listSize, OverallUrls - i)));
            }
            WorkerThreadList.Clear();
            backgroundThreadRunning = true;


            //Runs a thread pool for the split up lists to basically gather all URLs and their respective website text.
            foreach (var list in lists)
            {
                var t = new Thread(() =>
                {
                    //Parallel.ForEach(text, (url) =>
                    foreach (var url in list)
                    {
                        var newurl = "https://youporn.com";


                        CurrentUrlIdx++;


                        var webText = UrlInteraction.GrabText(newurl);

                        if (string.IsNullOrEmpty(webText))
                        {
                            if (ShowErrors)
                                Console.WriteLine(
                                    @"Text is empty assuming website is down.",
                                    Color.Red);
                            continue;
                        }

                        try
                        {
                            using var dbContext = new SqlDbContext();
                            dbContext.Label.Add(new LabelData() {Content = webText, Url = url, UrlType = Type});
                            dbContext.SaveChangesAsync();
                        }
                        catch (ObjectDisposedException)
                        {
                        }
                    } //);
                });
                t.Start();
                WorkerThreadList.Add(t);
            }

            uiThread = new Thread((() =>
            {
                Console.Clear();
                while (backgroundThreadRunning)
                {
                    DrawOverallProgress();
                }
            }));
            uiThread.Start();

            //Join threads back into main thread
            foreach (var thread in WorkerThreadList)
            {
                do
                {
                    thread.Join();
                } while (thread.IsAlive);
            }
            backgroundThreadRunning = false;

            //Waiting for everything to rejoin together and update threads to stop
            Thread.Sleep(500);
            ProgressStopwatch.Stop();
            Console.WriteLine("Finished scraping URLs. Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Displays the current sweeping progress to attach http and https to the urls
        /// </summary>
        private static void DrawSweepProgress()
        {
            if (CurrentUrlIdx == 0) return;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            var tickPerObject = ProgressStopwatch.ElapsedTicks / CurrentUrlIdx;
            var remainingTime = TimeSpan.FromTicks(tickPerObject * (OverallUrls - CurrentUrlIdx));
            var progressBar = GetProgress((((float)CurrentUrlIdx / (float)OverallUrls) * 100) *2, 200);
            Console.WriteLine("Overall sweeping progress of Urls\n" +
                              $"Currently we are: {SweepingStep}\n" +
                              $"Failed to validate: {((float)FailedUrls / (float)OverallUrls) * 100}%\n" +
                              $"Remaining minutes: {remainingTime:c}\n" +
                              $"Sweeped {CurrentUrlIdx}/{OverallUrls} so far.\n" +
                              $"{progressBar}\n");
        }

        /// <summary>
        /// Draws the overall progress of scraping the website content.
        /// </summary>
        public static void DrawOverallProgress()
        {
            if (CurrentUrlIdx == 0) return;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            var progressBar = GetProgress((((float)CurrentUrlIdx / (float)OverallUrls) * 100) * 2, 200);
            var tickPerObject = ProgressStopwatch.ElapsedTicks / CurrentUrlIdx;
            var remainingTime = TimeSpan.FromTicks(tickPerObject * (OverallUrls - CurrentUrlIdx));
            var runningThreads = WorkerThreadList.Count(x => x.IsAlive);
            var progressStopWatch = ProgressStopwatch.Elapsed.ToString("g");
            Console.WriteLine("Overall scraping progress\n" +
                              $"Overall runtime is: {progressStopWatch}\n" +
                              $"Runtime per object is: {tickPerObject} Ticks/Url\n" +
                              $"Remaining Time is: {remainingTime:c}\n" +
                              $"Still running worker threads: {runningThreads}\n" +
                              $"Scraped {CurrentUrlIdx}/{OverallUrls} so far.\n" +
                              $"{progressBar}\n");
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