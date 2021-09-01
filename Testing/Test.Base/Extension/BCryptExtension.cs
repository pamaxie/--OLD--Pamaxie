using System.Diagnostics;

namespace Test.TestBase
{
    /// <summary>
    /// Encryption extension for BCrypt
    /// TODO Remove once the actual encryption for Application Credentials have been made
    /// </summary>
    public class BCryptExtension
    {
        /// <summary>
        /// Calculates the ideal cost for BCrypt hashing
        /// </summary>
        /// <returns></returns>
        public static int CalculateSaltCost()
        {
            int cost = 5;
            Stopwatch sw = new();
            sw.Start();
            BCrypt.Net.BCrypt.HashPassword("microbenchmark", cost);
            sw.Stop();

            double durationMs = sw.Elapsed.TotalMilliseconds;

            //Increasing cost by 1 would double the run time.
            //Keep increasing cost until the estimated duration is over 250 ms
            while (durationMs < 250)
            {
                cost += 1;
                durationMs *= 2;
            }
            return cost;
        }
    }
}