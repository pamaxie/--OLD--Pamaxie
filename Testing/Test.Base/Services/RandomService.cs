using System;

namespace Test.Base
{
    /// <summary>
    /// Provides random data used for testing
    /// </summary>
    public static class RandomService
    {
        /// <summary>
        /// Generates a random key
        /// </summary>
        /// <returns>Randomly generated key</returns>
        public static string GenerateRandomKey(int length = 19)
        {
            Random rnd = new();
            ulong min = ulong.Parse("1" + new string('0', length - 1));
            ulong max = ulong.Parse(new string('9', length));
            ulong uRange = max - min;
            ulong ulongRand;
            do
            {
                byte[] buf = new byte[8];
                rnd.NextBytes(buf);
                ulongRand = (ulong)BitConverter.ToInt64(buf, 0);
            } while (ulongRand > ulong.MaxValue - ((ulong.MaxValue % uRange) + 1) % uRange);

            return "10" + ((long)(ulongRand % uRange)).ToString(new string('0', length));
        }

        /// <summary>
        /// Generates a random name
        /// </summary>
        /// <param name="length">Length of name</param>
        /// <returns>Randomly generated name</returns>
        public static string GenerateRandomName(int length = 0)
        {
            Random r = new();

            if (length == 0)
                length = r.Next(4, 9);
            
            string[] consonants =
            {
                "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "sh", "zh", "t", "v",
                "w", "x"
            };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string name = "";
            name += consonants[r.Next(consonants.Length)];
            name = char.ToUpper(name[0]) + name[1..];
            name += vowels[r.Next(vowels.Length)];
            int b = 2;
            while (b < length)
            {
                name += consonants[r.Next(consonants.Length)];
                b++;
                name += vowels[r.Next(vowels.Length)];
                b++;
            }

            return name;
        }
    }
}