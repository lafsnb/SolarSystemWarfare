using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;

namespace SolarSystemWarfare
{
    class Enemy : Sprite
    {

        //private something ProjectileType { get; }
        private int NumOfProjectiles { get; }
        private double RateOfFire { get; }
        public Patterns Pattern { get; set; }

        public Enemy(double x, double y, double width, double height, double speed, int durability,
            int numOfProjectiles, double rateOfFire, Rectangle rect) 
            : base(x, y, width, height, speed, durability, rect)
        {
            NumOfProjectiles = numOfProjectiles;
            RateOfFire = rateOfFire;
            //rect = new Rectangle((int)x, (int)y, 20, 20);
            rect.Height = 20;
            rect.Width = 20;
            Rect = rect;
        }
    }
}
