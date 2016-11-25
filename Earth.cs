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
        public Earth(int x, int y, int speed, int durability, Rectangle rect,
            Image icon) 
            : base(x, y, speed, durability, rect, icon)
        {

        }
    }
}
