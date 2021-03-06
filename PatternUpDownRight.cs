﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSystemWarfare
{
    class PatternUpDownRight : Patterns
    {



        public PatternUpDownRight(Sprite sprite, int howFarDown) 
            : base(sprite, howFarDown) { }

        
        public override void runPattern()
        {
            if (pathOne)
            {
                Sprite.MoveY(Sprite.Speed);
            } else if(pathTwo)
            {
                Sprite.MoveX(Sprite.Speed);
            }

            if (Sprite.Y >= howFarDown)
            {
                pathOne = false;
                pathTwo = true;
            }
            
        }
    }
}
