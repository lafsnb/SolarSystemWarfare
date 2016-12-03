using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SolarSystemWarfare
{
    class Earth : Sprite
    {
        public Earth(double x, double y, double width, double height, double speed, int durability, Rectangle rect) 
            : base(x, y, width, height, speed, durability, rect)
        {
            
            Rect = rect;
        }
    }
}
