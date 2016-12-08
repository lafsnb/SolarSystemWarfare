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
        public DateTime Date { get; private set; }

        public PlayerScore(string name, long score, DateTime date)
        {
            Name = name;
            Score = score;
            Date = date;
        }

        public int CompareTo(PlayerScore score)
        {
            if (Score == score.Score)
            {
                return -Date.CompareTo(score.Date);
            } else
            {
                return -Score.CompareTo(score.Score);
            }
        }

    }
}
