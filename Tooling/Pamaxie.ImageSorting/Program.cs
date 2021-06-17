using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoenM.ImageHash.HashAlgorithms;
using PamaxieML.Model;
using Console = System.Console;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using CoenM.ImageHash;
using static PamaxieML.Model.OutputProperties;

namespace Pamaxie.ImageSorting
{
    class Program
    {
        public const string CopyFolderName = "Compared Files";
        public static readonly IEnumerable<ImagePredictionResult> OutputDirectories = Enum.GetValues(typeof(OutputProperties.ImagePredictionResult)).Cast<OutputProperties.ImagePredictionResult>();
        public static string SourceDir = string.Empty;
        public static string TargetDir = string.Empty;
        public static int CurrentFileIdx;
        public static int OverallFiles;
        public static int SimilarItems;
        public static string SweepingStep;
        public static Stopwatch Timer;
        
        ///Sorts out images beforehand by predicted label and accuracy for said label.
        static void Main(string[] args)
        {
            bool comparefiles = false;

            while (true)
            {
                Console.WriteLine("Do you want to verify images are douplicates or not or do you want to go streight up to training?");
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y:
                        comparefiles = true;
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

            if (comparefiles)
            {
                while (true)
                {
                    Console.WriteLine("Please enter the folder which contains the images you want to predict the values of.");
                    SourceDir = Console.ReadLine();
                    if (Directory.Exists(SourceDir)) break;
                    Console.WriteLine("Invalid or Unknown Folder was entered. Folder could not be found. Please try again.");
                }
            }

            while (true)
            {
                if (comparefiles)
                    Console.WriteLine("Please enter a target DIRECTORY. Please do NOT place this on your desktop directly. We will create a few folders so it may get quite crowded.");
                else
                    Console.WriteLine("Please enter a target directory for the images to scan. We will move all images that will be used for the comparison over from the directory root into a folder called: \"Compared Files\"");
                TargetDir = Console.ReadLine();
                if (Directory.Exists(TargetDir)) break;
                Console.WriteLine("Target directory could not be found. Please ensure it exists and try again.");
            }

            //This may run for a long ass time.
            Console.WriteLine("Getting all files in the directory. Can take a very very long time.");
            var files = Directory.GetFiles(SourceDir);

            var items = new List<Tuple<string, ulong>>();
            if (comparefiles)
                items = GetSimilarItems(files);
            else
                items = MoveToSubDir(files);
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
                var key = Console.ReadKey();
                switch (key.Key)
                {
                    case ConsoleKey.Y:
                        copyItems = true;
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

            if (copyItems)
            {
                items = CopyFiles(items, TargetDir);
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
            foreach (var directory in OutputDirectories)
            {
                Console.WriteLine("Creating Directory structure for label: " + directory);
                var newDir = TargetDir + "\\" + directory;
                if (!Directory.Exists(newDir))
                {
                    Directory.CreateDirectory(newDir);
                    for (int i = 0; i < 10; i++)
                    {
                        Directory.CreateDirectory(newDir + "\\" + i * 10 + "%");
                    }
                }
            }


            Console.WriteLine("Finished creating directories.");
            var doingWork = true;
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
            foreach (var item in items)
            {
                CurrentFileIdx++;


                // Add input data
                ModelInput input = new ModelInput
                {
                    ImageSource = item.Item1
                };
                // Load model and predict output of sample data
                ConsumeModel.Predict(input, out OutputProperties labelResult);
                var folder = string.Empty;
                switch (labelResult.PredictedLabel)
                {
                    case OutputProperties.ImagePredictionResult.Gore:
                        folder = GetLikelyhoodFolder(labelResult.GoreLikelihood) + "%";
                        break;
                    case OutputProperties.ImagePredictionResult.None:
                        folder = GetLikelyhoodFolder(labelResult.NoneLikelihood) + "%";
                        break;
                    case OutputProperties.ImagePredictionResult.Pornographic:
                        folder = GetLikelyhoodFolder(labelResult.PornographicLikelihood) + "%";
                        break;
                    case OutputProperties.ImagePredictionResult.Racy:
                        folder = GetLikelyhoodFolder(labelResult.RacyLikelihood) + "%";
                        break;
                    case OutputProperties.ImagePredictionResult.Violence:
                        folder = GetLikelyhoodFolder(labelResult.ViolenceLikelihood) + "%";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (CurrentFileIdx < 4)
                {
                    Console.Clear();
                }
                var fi = new FileInfo(item.Item1);
                fi.CopyTo(TargetDir + "\\" + labelResult.PredictedLabel + "\\" + folder + "\\" + fi.Name, true);
            }

            Timer.Stop();
            doingWork = false;
            Console.Clear();
            Console.WriteLine("Finished work. Press any key to exit.");
            Console.ReadKey();
        }

        /// <summary>
        /// Get the folder where to put the items in depending on likelyhood.
        /// </summary>
        /// <param name="likelyhood"></param>
        /// <returns></returns>
        public static string GetLikelyhoodFolder(float likelyhood)
        {
            likelyhood *= 100;
            var num = likelyhood  - likelyhood % 10;
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
            var items = new List<Tuple<string, ulong>>();
            foreach (var file in files)
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
        public static List<Tuple<string, ulong>> GetSimilarItems(string[] files)
        {
            //Calculate MD5 Hashes
            #region GetMd5Hashes
            Console.Write("Starting to compute MD5 hashes for all files");
            for (int i = 0; i < 6; i++)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
            var hashAlgorithm = new DifferenceHash();

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

            var fileWithHash = new ConcurrentBag<Tuple<string, ulong>>();
            Parallel.ForEach(files, (file) =>
            {
                try
                {
                    CurrentFileIdx++;
                    using var stream = File.OpenRead(file);
                    var hash = hashAlgorithm.Hash(stream);
                    fileWithHash.Add(new Tuple<string, ulong>(file, hash));
                }
                catch (Exception)
                {

                }

            });
            doingWork = false;

            Timer.Stop();
            Console.Clear();
            Console.WriteLine("Going through roughly " + files.Length + " took around: " + Timer.Elapsed.ToString("g"));

            Console.Write("Starting to compare hashes and find direct douplicates");
            for (int i = 0; i < 6; i++)
            {
                Console.Write(".");
                Thread.Sleep(500);
            }
            Timer.Reset();
            #endregion

            //Compare the Calculated MD5 hashes and never store doupes.
            #region CompareMD5Hashes

            var comparisonInput = fileWithHash.ToArray();
            var comparedItems = new List<Tuple<string, ulong>>();
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

            foreach (var file in comparisonInput)
            {
                CurrentFileIdx++;
                bool known = false;

                for (int y = 0; y < comparedItems.Count; y++)
                {
                    var similarity = CompareHash.Similarity(file.Item2, comparedItems[y].Item2);
                    if (similarity > 90)
                    {
                        known = true;
                        break;
                    }
                }

                if (known)
                {
                    SimilarItems++;
                    continue;
                }

                comparedItems.Add(file);
            }

            doingWork = false;
            Console.Clear();
            Console.WriteLine("Found roughly " + SimilarItems + " doupes or too similar items (likely recropped or not different enough to mattter).");
            #endregion

            return comparedItems;
        }

        /// <summary>
        /// Copies the files to a new location and list
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        public static List<Tuple<string, ulong>> CopyFiles(List<Tuple<string, ulong>> fileList, string targetDir)
        {
            var newFiles = new List<Tuple<string, ulong>>();

            targetDir = targetDir + "\\" + CopyFolderName;

            //Check if folder structure for this folder already exists.
            if (!Directory.Exists(targetDir))
                Directory.CreateDirectory(targetDir);

            foreach (var file in fileList)
            {
                var fileInfo = new FileInfo(file.Item1);
                //This should usually NEVER happen but we still check this.
                if (!fileInfo.Exists) continue;


                var newLocation = targetDir + "\\" + fileInfo.Name;
                fileInfo.CopyTo(newLocation, true);
                newFiles.Add(new Tuple<string, ulong>(newLocation, file.Item2));
            }
            return newFiles;
        }
    }
}
