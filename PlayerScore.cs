using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSystemWarfare
{
    [Serializable]
    struct PlayerScore : IComparable<PlayerScore>
    {

        public string Name { get; private set; }
        public long Score { get; private set; }
        public int Rank { get; private set; }

        public PlayerScore(string name, long score, int rank)
        {
            Name = name;
            Score = score;
            Rank = rank;
        }

        public int CompareTo(PlayerScore score)
        {
            if (Score == score.Score)
            {
                return 0;
            } else
            {
                return -Score.CompareTo(score.Score);
            }
        }

    }
}
