using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CoenM.ImageHash;
using CoenM.ImageHash.HashAlgorithms;
using PamaxieML.Model;
using static PamaxieML.Model.OutputProperties;

namespace Pamaxie.ImageSorting
{
    public static class Program
    {
        private const string CopyFolderName = "Compared Files";
        private static readonly IEnumerable<ImagePredictionResult> OutputDirectories = Enum.GetValues(typeof(ImagePredictionResult)).Cast<ImagePredictionResult>();
        private static string _sourceDir = string.Empty;
        private static string _targetDir = string.Empty;
        internal static int CurrentFileIdx;
        internal static int OverallFiles;
        private static int _similarItems;
        internal static string SweepingStep;
        internal static Stopwatch Timer;
        
        ///Sorts out images beforehand by predicted label and accuracy for said label.
        public static void Main(string[] args)
        {
            bool compareFiles = false;

            while (true)
            {
                Console.WriteLine("Do you want to verify images are duplicates or not or do you want to go straight up to training?");
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key)
                {
                    case {Key: ConsoleKey.Y}:
                        compareFiles = true;
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

            if (compareFiles)
            {
                while (true)
                {
                    Console.WriteLine("Please enter the folder which contains the images you want to predict the values of.");
                    _sourceDir = Console.ReadLine();
                    if (Directory.Exists(_sourceDir)) break;
                    Console.WriteLine("Invalid or Unknown Folder was entered. Folder could not be found. Please try again.");
                }
            }

            while (true)
            {
                Console.WriteLine(compareFiles
                    ? "Please enter a target DIRECTORY. Please do NOT place this on your desktop directly. We will create a few folders so it may get quite crowded."
                    : "Please enter a target directory for the images to scan. We will move all images that will be used for the comparison over from the directory root into a folder called: \"Compared Files\"");
                _targetDir = Console.ReadLine();
                if (Directory.Exists(_targetDir)) break;
                Console.WriteLine("Target directory could not be found. Please ensure it exists and try again.");
            }

            //This may run for a long ass time.
            Console.WriteLine("Getting all files in the directory. Can take a very very long time.");
            string[] files = Directory.GetFiles(_sourceDir ?? string.Empty);

            List<Tuple<string, ulong>> items = compareFiles ? GetSimilarItems(files) : MoveToSubDir(files);
            Console.Clear();

            Console.Write("Finished work on detecting similar items.");
            for (int i = 0; i < 6; i++)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
            Console.Clear();

            bool copyItems = false;
            while (true)
            {
                Console.WriteLine("Do you want to copy the files to the output directory? (Highly recommended, if you abort this during the Neural Network process all data will be lost.)");
                ConsoleKeyInfo key = Console.ReadKey();
                switch (key)
                {
                    case {Key: ConsoleKey.Y}:
                        copyItems = true;
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

            if (copyItems)
            {
                items = CopyFiles(items, _targetDir);
                Console.Write("Finished copying items");
                for (int i = 0; i < 6; i++)
                {
                    Console.Write(".");
                    Thread.Sleep(250);
                }
            }

            Console.Write("Setting up folder structure now...");
            for (int i = 0; i < 6; i++)
            {
                Console.Write(".");
                Thread.Sleep(250);
            }

            Console.WriteLine("Creating Neural network folders");
            foreach (ImagePredictionResult directory in OutputDirectories)
            {
                Console.WriteLine("Creating Directory structure for label: " + directory);
                string newDir = _targetDir + "\\" + directory;
                if (Directory.Exists(newDir)) continue;
                Directory.CreateDirectory(newDir);
                for (int i = 0; i < 10; i++)
                {
                    Directory.CreateDirectory(newDir + "\\" + i * 10 + "%");
                }
            }


            Console.WriteLine("Finished creating directories.");
            bool doingWork = true;
            new Thread((() =>
            {
                Console.Clear();
                while (doingWork)
                {
                    Other.DrawOverallProgress();
                }
            })).Start();

            CurrentFileIdx = 0;
            OverallFiles = items.Count;
            Timer.Reset();
            Timer.Start();
            foreach ((string item1, _) in items)
            {
                CurrentFileIdx++;

                // Add input data
                ModelInput input = new ModelInput
                {
                    ImageSource = item1
                };
                // Load model and predict output of sample data
                ConsumeModel.Predict(input, out OutputProperties labelResult);
                string folder = labelResult.PredictedLabel switch
                {
                    ImagePredictionResult.Gore =>
                        GetLikelihoodFolder(labelResult.GoreLikelihood) + "%",
                    ImagePredictionResult.None =>
                        GetLikelihoodFolder(labelResult.NoneLikelihood) + "%",
                    ImagePredictionResult.Pornographic => GetLikelihoodFolder(labelResult
                        .PornographicLikelihood) + "%",
                    ImagePredictionResult.Racy =>
                        GetLikelihoodFolder(labelResult.RacyLikelihood) + "%",
                    ImagePredictionResult.Violence => GetLikelihoodFolder(labelResult
                        .ViolenceLikelihood) + "%",
                    _ => throw new ArgumentOutOfRangeException()
                };

                if (CurrentFileIdx < 4)
                {
                    Console.Clear();
                }
                FileInfo fi = new(item1);
                fi.CopyTo(_targetDir + "\\" + labelResult.PredictedLabel + "\\" + folder + "\\" + fi.Name, true);
            }

            Timer.Stop();
            doingWork = false;
            Console.Clear();
            Console.WriteLine("Finished work. Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Get the folder where to put the items in depending on likelihood.
        /// </summary>
        /// <param name="likelihood"></param>
        /// <returns></returns>
        private static string GetLikelihoodFolder(float likelihood)
        {
            likelihood *= 100;
            float num = likelihood  - likelihood % 10;
            if (num >= 100)
            {
                num -= 10;
            }
            return num.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Moves the already existing files into a sub directory to make sure the folder structure stays consistent
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private static List<Tuple<string, ulong>> MoveToSubDir(string[] files)
        {
            List<Tuple<string, ulong>> items = new();
            foreach (string file in files)
            {
                //TODO: implement this
                throw new NotImplementedException();
            }
            return items;
        }

        /// <summary>
        /// Filters out similar images from an array of files locations
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private static List<Tuple<string, ulong>> GetSimilarItems(string[] files)
        {
            //Calculate MD5 Hashes
            #region GetMd5Hashes
            Console.Write("Starting to compute MD5 hashes for all files");
            for (int i = 0; i < 6; i++)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
            DifferenceHash hashAlgorithm = new();

            SweepingStep = "Calculating MD5 Hashes";
            OverallFiles = files.Length;
            CurrentFileIdx = 0;
            Timer = new Stopwatch();
            Timer.Start();
            bool doingWork = true;
            new Thread((() =>
            {
                Console.Clear();
                while (doingWork)
                {
                    Other.DrawSweepProgress();
                }
            })).Start();

            ConcurrentBag<Tuple<string, ulong>> fileWithHash = new();
            Parallel.ForEach(files, file =>
            {
                try
                {
                    CurrentFileIdx++;
                    using FileStream stream = File.OpenRead(file);
                    ulong hash = hashAlgorithm.Hash(stream);
                    fileWithHash.Add(new Tuple<string, ulong>(file, hash));
                }
                catch
                {
                    // ignored
                }
            });
            doingWork = false;

            Timer.Stop();
            Console.Clear();
            Console.WriteLine("Going through roughly " + files.Length + " took around: " + Timer.Elapsed.ToString("g"));

            Console.Write("Starting to compare hashes and find direct duplicates");
            for (int i = 0; i < 6; i++)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
            Timer.Reset();
            #endregion

            //Compare the Calculated MD5 hashes and never store dupes.
            #region CompareMD5Hashes
            Tuple<string, ulong>[] comparisonInput = fileWithHash.ToArray();
            List<Tuple<string, ulong>> comparedItems = new();
            SweepingStep = "Detecting direct MD5 Similarities and ignoring items with too high similarity.";
            CurrentFileIdx = 0;
            OverallFiles = fileWithHash.Count;
            doingWork = true;
            Timer.Start();
            new Thread((() =>
            {
                Console.Clear();
                while (doingWork)
                {
                    Other.DrawSweepProgress();
                }
            })).Start();

            foreach (Tuple<string, ulong> file in comparisonInput)
            {
                CurrentFileIdx++;
                bool known = comparedItems.Select(t => CompareHash.Similarity(file.Item2, t.Item2)).Any(similarity => similarity > 90);

                if (known)
                {
                    _similarItems++;
                    continue;
                }

                comparedItems.Add(file);
            }

            doingWork = false;
            Console.Clear();
            Console.WriteLine("Found roughly " + _similarItems + " dupes or too similar items (likely regrouped or not different enough to matter).");
            #endregion

            return comparedItems;
        }

        /// <summary>
        /// Copies the files to a new location and list
        /// </summary>
        /// <param name="fileList"></param>
        /// <param name="targetDir"></param>
        /// <returns></returns>
        private static List<Tuple<string, ulong>> CopyFiles(IEnumerable<Tuple<string, ulong>> fileList, string targetDir)
        {
            List<Tuple<string, ulong>> newFiles = new();

            targetDir = targetDir + "\\" + CopyFolderName;

            //Check if folder structure for this folder already exists.
            if (!Directory.Exists(targetDir)) Directory.CreateDirectory(targetDir);

            foreach ((string item1, ulong item2) in fileList)
            {
                FileInfo fileInfo = new(item1);
                //This should usually NEVER happen but we still check this.
                if (!fileInfo.Exists) continue;


                string newLocation = targetDir + "\\" + fileInfo.Name;
                fileInfo.CopyTo(newLocation, true);
                newFiles.Add(new Tuple<string, ulong>(newLocation, item2));
            }
            return newFiles;
        }
    }
}
