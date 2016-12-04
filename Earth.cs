using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SolarSystemWarfare
{
    class Earth : Sprite
    {

        public bool Up { get; set; }
        public bool Down { get; set; }
        public bool Left { get; set; }
        public bool Right { get; set; }
        public bool Firing { get; set; }

        public Earth(double x, double y, double speed, int durability, Rectangle rect) 
            : base(x, y, speed, durability, rect)
        {
            Rect = rect;

            Up = false;
            Down = false;
            Left = false;
            Right = false;
            Firing = false;
        }
    }
}
