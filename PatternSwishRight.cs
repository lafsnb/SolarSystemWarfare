using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SolarSystemWarfare
{
    class PatternSwishRight : Patterns
    {
        private int howFarX;

        public PatternSwishRight(Sprite sprite, int howFarDown, int howFarX) 
            : base (sprite, howFarDown)
        {
            this.howFarX = howFarX;
        }


        public override void runPattern()
        {
            if (Sprite.Y >= howFarDown)
            {
                if (pathOne)
                {
                    Sprite.MoveY(Sprite.Speed / 4);
                    Sprite.MoveX(Sprite.Speed);
                } else
                {
                    Sprite.MoveY(Sprite.Speed / 4);
                    Sprite.MoveX(-Sprite.Speed);
                }
                
            }
            if (pathOne && !(Sprite.Y >= howFarDown))
            {
                Sprite.MoveY(Sprite.Speed / 4);
                Sprite.MoveX(Sprite.Speed);
            }
            else if (pathTwo && !(Sprite.Y >= howFarDown))
            {
                Sprite.MoveY(Sprite.Speed / 4);
                Sprite.MoveX(-Sprite.Speed);
            }
            if (Sprite.X == howFarX && !(Sprite.Y >= howFarDown))
            {
                pathOne = false;
                pathTwo = true;
            }
            else if (Sprite.X == 20 && !(Sprite.Y >= howFarDown))
            {
                pathOne = true;
                pathTwo = false;
            }
        }

    }
}
