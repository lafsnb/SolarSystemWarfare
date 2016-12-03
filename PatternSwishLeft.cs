using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSystemWarfare
{
    class PatternSwishLeft : PatternSwishRight
    {

        public PatternSwishLeft(Sprite sprite, int howFarDown) 
            : base (sprite, howFarDown)
        {
            pathOne = false;
            pathTwo = true;
        }
    }
}
