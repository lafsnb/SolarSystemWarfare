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
        public Earth(double x, double y, double speed, int durability, Rectangle rect) 
            : base(x, y, speed, durability, rect)
        {
    
            Rect = rect;
        }
    }
}
