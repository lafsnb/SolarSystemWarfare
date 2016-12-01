using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SolarSystemWarfare
{
    class Patterns
    {

        private bool goLeft = false;
        private bool goRight = true;

        private Sprite Sprite { get; set; }

        public Patterns(Sprite sprite)
        {
            Sprite = sprite;
        }

        public void SwishSway()
        {
            if (goRight)
            {
                Sprite.MoveY(.5);
                Sprite.MoveX(2);
            }
            else if (goLeft)
            {
                Sprite.MoveY(.5);
                Sprite.MoveX(-2);
            }
            if (Sprite.X == 400)
            {
                goRight = false;
                goLeft = true;
            }
            else if (Sprite.X == 20)
            {
                goRight = true;
                goLeft = false;
            }
        }

    }
}
