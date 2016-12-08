using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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

        public static void WriteToFile(List<PlayerScore> scores)
        {
            
            using (Stream stream = new FileStream("HighScores.bin", FileMode.Create, FileAccess.Write, FileShare.None))
            {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, scores);
            }
        }

        //public static void WriteToFile(string name, IDictionary<string, long> scores)
        //{

        //    if (scores.ContainsKey(name))
        //    {
        //        bool writeToFile = false;

        //        for (int counter = 0; counter != scores.Count; counter++)
        //        {
        //            if (scores.ElementAt(counter).Key == name)
        //            {
        //                if (Score.GetScore() >= scores.ElementAt(counter).Value)
        //                {
        //                    scores.Remove(name);
        //                    scores.Add(name, GetScore());
        //                    writeToFile = true;
        //                    break;
        //                }
        //            }
        //        }

        //        if (writeToFile)
        //        {
        //            using (StreamWriter file =
        //            new StreamWriter("HighScores.txt", false))
        //            {
        //                for (int counter = 0; counter != scores.Count; counter++)
        //                {
        //                    file.WriteLine($"{scores.ElementAt(counter).Key},{scores.ElementAt(counter).Value}");
        //                }
        //            }
        //        }

        //    }
        //    else
        //    {
        //        string write = $"{name},{GetScore()}";

        //        using (StreamWriter file =
        //        new StreamWriter("HighScores.txt", true))
        //        {
        //            file.WriteLine(write);
        //        }
        //    }
        //}

        public static List<PlayerScore> RestoreScores()
        {
            List<PlayerScore> playerScores;

            if (File.Exists("HighScores.bin"))
            {

                using (Stream stream = new FileStream("HighScores.bin", FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    IFormatter formatter = new BinaryFormatter();
                    playerScores = (List<PlayerScore>)formatter.Deserialize(stream);
                }
            } else
            {
                playerScores = new List<PlayerScore>();
            }
            return playerScores;
        }



        //public static IDictionary<string, long> ReadFromFile()
        //{
        //    IDictionary<string, long> dictionary = new Dictionary<string, long>();
        //    try
        //    {
        //        using (StreamReader sr = new StreamReader("HighScores.txt"))
        //        {
        //            while (!sr.EndOfStream)
        //            {
        //                string line = sr.ReadLine();
        //                string[] split = line.Split(',');
        //                dictionary.Add(split[0], Convert.ToInt64(split[1]));
        //            }
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("The file could not be read:");
        //        Console.WriteLine(e.Message);
        //    }

        //    IDictionary<string, long> dict = dictionary.OrderByDescending(pair => pair.Value)
        //       .ToDictionary(pair => pair.Key, pair => pair.Value);

        //    return dict;
        //}

    }
}
