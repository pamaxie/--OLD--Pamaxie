using DnsClient;
using Pamaxie.Leecher.Database;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Pamaxie.Leecher
{
    public static class Program
    {
        private static string _originFile;
        internal static readonly string ImageDestinationDir = string.Empty;
        private static string _type;
        private static int _overallUrls;
        private static int _currentUrlIdx;
        private static int _failedUrls = 0;
        private static int _workerThreads = 10;
        private static bool _showErrors = false;
        private static List<Thread> _workerThreadList;
        private static Stopwatch _progressStopwatch;
        private static string _sweepingStep;

        public static void Main(string[] args)
        {
            //Ask the user for a plaintext file to load the urls from
            while (true)
            {
                Console.WriteLine(@"Please enter a file including path to read urls from");
                string originFile = Console.ReadLine();
                if (File.Exists(originFile))
                {
                    _originFile = originFile;
                    break;
                }

                Console.WriteLine(@"File could not be found please validate it exists");
            }

            //Ask the user if they want to clear out the database
            while (true)
            {
                Console.WriteLine(@"Clear out database before continuing? THIS CAN NOT BE UNDONE! (y/N)");
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key)
                {
                    case {Key: ConsoleKey.Y}:
                        using (SqlDbContext dbContext = new())
                        {
                            List<LabelData> items = dbContext.Label.ToList();
                            dbContext.RemoveRange(items);
                            dbContext.SaveChanges();
                        }
                        break;
                    case {Key: ConsoleKey.N}:
                        break;
                    case {Key: ConsoleKey.Enter}:
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
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key)
                {
                    case {Key: ConsoleKey.Y}:
                        _showErrors = true;
                        break;
                    case {Key: ConsoleKey.Enter}:
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
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key)
                {
                    case {Key: ConsoleKey.D1}:
                        _type = "adult";
                        break;
                    case {Key: ConsoleKey.D2}:
                        _type = "marketing";
                        break;
                    case {Key: ConsoleKey.D3}:
                        _type = "banking";
                        break;
                    case {Key: ConsoleKey.D4}:
                        _type = "hacking";
                        break;
                    case {Key: ConsoleKey.D5}:
                        _type = "mixed-adult";
                        break;
                    case {Key: ConsoleKey.D6}:
                        _type = "advertising";
                        break;
                    case {Key: ConsoleKey.D7}:
                        while (true)
                        {
                            string type = Console.ReadLine();
                            if (string.IsNullOrEmpty(type))
                            {
                                Console.WriteLine("Type can not be empty please enter a valid type.");
                                continue;
                            }

                            Console.WriteLine($"Do you want to set the type to: {type}? (Y/n)");
                            ConsoleKeyInfo typeConfirm = Console.ReadKey();
                            switch (typeConfirm)
                            {
                                case {Key: ConsoleKey.Y}:
                                    _type = type;
                                    break;
                                case {Key: ConsoleKey.Enter}:
                                    _type = type;
                                    break;
                                case {Key: ConsoleKey.N}:
                                    type = string.Empty;
                                    break;
                                default:
                                    Console.WriteLine("Unexpected input detected please try again.");
                                    continue;
                            }
                        }
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
                if (!(int.TryParse(Console.ReadLine(), out int num)))
                {
                    Console.WriteLine("Invalid number entered. Please enter a non floating point integer number");
                }
                _workerThreads = num;
                break;
            }

            //Validate settings exist
            if (_originFile == null)
            {
                Console.WriteLine(@"Hit unexpected problem. Please try again.");
                return;
            }

            List<string> text = File.ReadAllLines(_originFile).ToList();
            _overallUrls = text.Count;
            switch (_overallUrls)
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
                    bool attemptCsvSeparation = false;
                    while (true)
                    {
                        Console.WriteLine("Only found a single line attempt to separate over commas (CSV File)? (Y/n)");
                        ConsoleKeyInfo doCsvConfirm = Console.ReadKey();
                        switch (doCsvConfirm)
                        {
                            case {Key: ConsoleKey.Y}:
                                attemptCsvSeparation = true;
                                break;
                            case {Key: ConsoleKey.N}:
                                Console.WriteLine("Not doing CSV separation and continuing with scraping progress..");
                                break;
                            case {Key: ConsoleKey.Enter}:
                                break;
                            default:
                                Console.WriteLine("Unexpected input detected please try again.");
                                continue;
                        }
                        break;
                    }

                    if (!attemptCsvSeparation)
                    {
                        Console.WriteLine(
                            "Attempting to separate over commas. We do not recommend feeding CSV files into here.");
                        List<string> newTextList = new();
                        foreach (string line in text.ToList())
                        {
                            newTextList.AddRange(line.Split(","));
                        }

                        while (true)
                        {
                            Console.WriteLine(
                                $"Found: {newTextList.Count} lines after separation. Continue or exit to re-validate data? (Y/n)");
                            ConsoleKeyInfo doCsvConfirm = Console.ReadKey();
                            switch (doCsvConfirm)
                            {
                                case {Key: ConsoleKey.Y}:
                                    Console.WriteLine("Press any key to exit...");
                                    Console.ReadKey();
                                    Environment.Exit(1);
                                    break;
                                case {Key: ConsoleKey.N}:
                                    Console.WriteLine($"Continuing with {newTextList.Count} Urls");
                                    break;
                                case {Key: ConsoleKey.Enter}:
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
                Console.WriteLine(
                    @"Sweep urls (Validate their Records exist and they are reachable) before Scraping their content? This results in less errors but makes scraping on large lists a lot slower... (n/Y)");
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key)
                {
                    case {Key: ConsoleKey.Y}:
                        break;
                    case {Key: ConsoleKey.N}:
                        skipSweep = true;
                        break;
                    case {Key: ConsoleKey.Enter}:
                        break;
                    default:
                        Console.WriteLine("Unexpected input detected please try again.");
                        continue;
                }
                break;
            }

            bool backgroundThreadRunning = true;
            Thread uiThread;
            List<string> sweptUrls = new();
            if (!skipSweep)
            {
                _progressStopwatch = new Stopwatch();
                _progressStopwatch.Start();
                
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
                _sweepingStep = "checking each domains DNS Records";
                Parallel.ForEach(text, (domain) =>
                {
                    bool isIp = false;
                    _currentUrlIdx++;

                    if (IPAddress.TryParse(domain, out _)) isIp = true;

                    if (!isIp)
                    {
                        try
                        {
                            LookupClient client = new();

                            IPHostEntry hostEntry = client.GetHostEntry(domain);

                            //Host has no DNS entries thus it will not be looked up.
                            if (hostEntry == null || hostEntry.AddressList.Length < 1)
                            {
                                _failedUrls++;
                                return;
                            }

                            //Add if there are host entries
                            sweptUrls.Add(domain);
                        }
                        catch (DnsResponseException)
                        {
                            _failedUrls++;
                        }

                    }

                    //Add if there are no host entries.
                    sweptUrls.Add(domain);
                });

                //Reset progress for next step
                text.Clear();
                text = sweptUrls;
                _overallUrls = text.Count;
                _currentUrlIdx = 0;
                _failedUrls = 0;

                //Check if Domains can be reached over http or https
                _sweepingStep = "checking which domains are reachable";
                Parallel.ForEach(text, (domain) =>
                {
                    _currentUrlIdx++;

                    try
                    {
                        bool exists = UrlInteraction.UrlExists($"http://{domain}", out string resUri);
                        if (exists)
                        {
                            sweptUrls.Add(resUri);
                            return;
                        }
                    }
                    catch
                    {
                        // ignored
                    }

                    _failedUrls++;
                });
                _progressStopwatch.Stop();
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
                _progressStopwatch.Reset();
                _currentUrlIdx = 0;
                _overallUrls = sweptUrls.Count;
                text.Clear();
                text.AddRange(sweptUrls);
            }
            else
            {
                for (int index = 0; index < text.Count; index++)
                {
                    string url = text[index];
                    sweptUrls.Add($"http://{url}");
                }

                _progressStopwatch = new Stopwatch();
                _progressStopwatch.Start();
            }

            _overallUrls = sweptUrls.Count;
            List<List<string>> lists = new();
            int listSize = _overallUrls / _workerThreads;
            if (listSize == 0) listSize = 1;
            _workerThreadList = new List<Thread>();
            lists.Clear();

            for (int i = 0; i < _overallUrls; i += listSize)
            {
                lists.Add(sweptUrls.GetRange(i, Math.Min(listSize, _overallUrls - i)));
            }
            _workerThreadList.Clear();
            backgroundThreadRunning = true;

            //Runs a thread pool for the split up lists to basically gather all URLs and their respective website text.
            foreach (List<string> list in lists)
            {
                Thread t = new(() =>
                {
                    //Parallel.ForEach(text, (url) =>
                    foreach (string url in list)
                    {
                        const string newUrl = "https://youporn.com";
                        _currentUrlIdx++;
                        string webText = UrlInteraction.GrabText(newUrl);

                        if (string.IsNullOrEmpty(webText))
                        {
                            if (_showErrors)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine( @"Text is empty assuming website is down.");
                                Console.ForegroundColor = ConsoleColor.Black;
                            }
                            continue;
                        }

                        try
                        {
                            using SqlDbContext dbContext = new();
                            dbContext.Label.Add(new LabelData() {Content = webText, Url = url, UrlType = _type});
                            dbContext.SaveChangesAsync();
                        }
                        catch (ObjectDisposedException) { }
                    } //);
                });
                t.Start();
                _workerThreadList.Add(t);
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
            foreach (Thread thread in _workerThreadList)
            {
                do
                {
                    thread.Join();
                } while (thread.IsAlive);
            }
            backgroundThreadRunning = false;

            //Waiting for everything to rejoin together and update threads to stop
            Thread.Sleep(500);
            _progressStopwatch.Stop();
            Console.WriteLine("Finished scraping URLs. Press any key to exit...");
            Console.ReadKey();
        }
        
        /// <summary>
        /// Displays the current sweeping progress to attach http and https to the urls
        /// </summary>
        private static void DrawSweepProgress()
        {
            if (_currentUrlIdx == 0) return;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            long tickPerObject = _progressStopwatch.ElapsedTicks / _currentUrlIdx;
            TimeSpan remainingTime = TimeSpan.FromTicks(tickPerObject * (_overallUrls - _currentUrlIdx));
            string progressBar = GetProgress((((float)_currentUrlIdx / (float)_overallUrls) * 100) *2, 200);
            Console.WriteLine("Overall sweeping progress of Urls\n" +
                              $"Currently we are: {_sweepingStep}\n" +
                              $"Failed to validate: {((float)_failedUrls / (float)_overallUrls) * 100}%\n" +
                              $"Remaining minutes: {remainingTime:c}\n" +
                              $"Swept {_currentUrlIdx}/{_overallUrls} so far.\n" +
                              $"{progressBar}\n");
        }

        /// <summary>
        /// Draws the overall progress of scraping the website content.
        /// </summary>
        private static void DrawOverallProgress()
        {
            if (_currentUrlIdx == 0) return;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            string progressBar = GetProgress(((float)_currentUrlIdx / _overallUrls) * 100 * 2, 200);
            long tickPerObject = _progressStopwatch.ElapsedTicks / _currentUrlIdx;
            TimeSpan remainingTime = TimeSpan.FromTicks(tickPerObject * (_overallUrls - _currentUrlIdx));
            int runningThreads = _workerThreadList.Count(x => x.IsAlive);
            string progressStopWatch = _progressStopwatch.Elapsed.ToString("g");
            Console.WriteLine("Overall scraping progress\n" +
                              $"Overall runtime is: {progressStopWatch}\n" +
                              $"Runtime per object is: {tickPerObject} Ticks/Url\n" +
                              $"Remaining Time is: {remainingTime:c}\n" +
                              $"Still running worker threads: {runningThreads}\n" +
                              $"Scraped {_currentUrlIdx}/{_overallUrls} so far.\n" +
                              $"{progressBar}\n");
        }

        /// <summary>
        /// Displays Progress
        /// </summary>
        /// <param name="currentProgress"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        private static string GetProgress(double currentProgress, int width)
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