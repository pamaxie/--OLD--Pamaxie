using System.Diagnostics;

namespace Pamaxie.Database.Extensions.Basic
{
    public static class ByCrptExt
    {
        /// <summary>
        /// Calculates the ideal cost for Bycpt hashing
        /// </summary>
        /// <returns></returns>
        public static int CalculateSaltCost()
        {
            var cost = 5;
            var sw = new Stopwatch();
            sw.Start();
            BCrypt.Net.BCrypt.HashPassword("microbenchmark", cost);
            sw.Stop();

            double durationMS = sw.Elapsed.TotalMilliseconds;

            //Increasing cost by 1 would double the run time.
            //Keep increasing cost until the estimated duration is over 250 ms
            while (durationMS < 250)
            {
                cost += 1;
                durationMS *= 2;
            }

            return cost;
        }


    }
}
