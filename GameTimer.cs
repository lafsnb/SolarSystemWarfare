using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace SolarSystemWarfare
{
    class GameTimer : Timer
    {

        private int time;

        public GameTimer(double interval) : base(interval){}

        public void Pause()
        {
            this.Stop();
            setStartTime();
        }

        public void setStartTime()
        {
            time = DateTime.Now.Millisecond;
        }

        public void Resume()
        {
            int timePassed = time - DateTime.Now.Millisecond;
            Interval = Interval - timePassed;
            this.Start();
        }
    }
}
