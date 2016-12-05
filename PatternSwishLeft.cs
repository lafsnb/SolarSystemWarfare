using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSystemWarfare
{
    class PatternSwishLeft : PatternSwishRight
    {

        public PatternSwishLeft(Sprite sprite, int howFarDown, int howFarX) 
            : base (sprite, howFarDown, howFarX)
        {
            pathOne = false;
            pathTwo = true;
        }
    }
}
