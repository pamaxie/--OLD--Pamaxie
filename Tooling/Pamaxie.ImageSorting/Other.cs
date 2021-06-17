using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pamaxie.ImageSorting
{
    public static class Other
    {
        /// <summary>
        /// Displays the current sweeping progress to attach http and https to the urls
        /// </summary>
        public static void DrawSweepProgress()
        {
            if (Program.CurrentFileIdx == 0) return;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            var tickPerObject = Program.Timer.ElapsedTicks / Program.CurrentFileIdx;
            var remainingTime = TimeSpan.FromTicks(tickPerObject * (Program.OverallFiles - Program.CurrentFileIdx));
            var progressBar = GetProgress((((float)Program.CurrentFileIdx / (float)Program.OverallFiles) * 100), 100);
            Console.WriteLine("Overall sweeping progress of Files\n" +
                              $"Currently we are: {Program.SweepingStep}\n" +
                              $"Remaining minutes: {remainingTime:c}\n" +
                              $"Sweeped {Program.CurrentFileIdx}/{Program.OverallFiles} Files so far.\n" +
                              $"{progressBar}\n");
        }

        /// <summary>
        /// Draws the overall progress of scraping the website content.
        /// </summary>
        public static void DrawOverallProgress()
        {
            if (Program.CurrentFileIdx == 0) return;
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            var progressBar = GetProgress((((float)Program.CurrentFileIdx / (float)Program.OverallFiles) * 100), 100);
            var msPerObject = Program.Timer.ElapsedMilliseconds / Program.CurrentFileIdx;
            var remainingTime = TimeSpan.FromTicks(msPerObject * (Program.OverallFiles - Program.CurrentFileIdx));
            //var runningThreads = WorkerThreadList.Count(x => x.IsAlive);
            var currentRuntime = Program.Timer.Elapsed.ToString("g");
            Console.WriteLine("Overall analysis progress\n" +
                              $"Overall runtime is: {currentRuntime}\n" +
                              $"Runtime per object is: {msPerObject} ms/File\n" +
                              $"Remaining Time is: {remainingTime:c}\n" +
                              //$"Still running worker threads: {runningThreads}\n" +
                              $"Analyzed {Program.CurrentFileIdx}/{Program.OverallFiles} Files so far.\n" +
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
