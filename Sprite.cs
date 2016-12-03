using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SolarSystemWarfare
{
    class Sprite
    {
        public bool Dead { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Speed { get; private set; }
        protected int Durability { get; }
        public Rectangle Rect { get; protected set; }
        //private Icon Icon { get; } //TODO

        public Sprite(double x, double y, double speed, int durability, Rectangle rect)
        {
            X = x;
            Y = y;
            Speed = speed;
            Durability = durability;
            Dead = false;
        }

        public void MoveX(double x)
        {
            X += x;
        }

        public void MoveY(double y)
        {
            Y += y;
        }

        

        public void Destroy()
        {
            //return false; //TODO
            Dead = true;

        }
    }
}
