using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SolarSystemWarfare
{
    static class Score
    {
        private static long Points = 0;

        public static void ResetScore()
        {
            Points = 0;
        }

        public static void IncrementScore()
        {
            Points++;
        }

        public static long GetScore()
        {
            return Points;
        }

        public static void WriteToFile(string name)
        {
            string write = $"{name},{GetScore()}";

            using (StreamWriter file =
            new StreamWriter("HighScores.txt", true))
            {
                file.WriteLine(write);
            }
        }

        public static IDictionary<string, long> ReadFromFile()
        {
            IDictionary<string, long> dictionary = new Dictionary<string, long>();
            try
            {
                using (StreamReader sr = new StreamReader("HighScores.txt"))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string[] split = line.Split(',');
                        dictionary.Add(split[0], Convert.ToInt64(split[1]));
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            IDictionary<string, long> dict = dictionary.OrderByDescending(pair => pair.Value)
               .ToDictionary(pair => pair.Key, pair => pair.Value);

            return dict;
        }

    }
}
