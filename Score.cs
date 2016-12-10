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
    }
}
