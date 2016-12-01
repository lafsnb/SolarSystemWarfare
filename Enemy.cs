using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SolarSystemWarfare
{
    class Enemy : Sprite
    {

        //private something ProjectileType { get; }
        private int NumOfProjectiles { get; }
        private double RateOfFire { get; }

        public Enemy(double x, double y, int speed, int durability, Rectangle rect, 
            Image icon, int numOfProjectiles, double rateOfFire) 
            : base(x, y, speed, durability, rect, icon)
        {
            NumOfProjectiles = numOfProjectiles;
            RateOfFire = rateOfFire;
        }
    }
}
