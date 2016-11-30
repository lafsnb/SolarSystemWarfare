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
        public double X { get; private set; }
        public double Y { get; private set; }
        private int Speed { get; }
        private int Durability { get; }
        private Rectangle Rect { get; }
        //private Icon Icon { get; } //TODO
        public Image Icon { get; private set; }

        public Sprite(double x, double y, int speed, int durability, 
            Rectangle rect, Image icon)
        {
            X = x;
            Y = y;
            Speed = speed;
            Durability = durability;
            Rect = rect;
            Icon = icon;
        }

        public void MoveX(double x)
        {
            X += x;
        }

        public void MoveY(double y)
        {
            Y += y;
        }

        

        public void Destroyed()
        {
            //return false; //TODO

        }
    }
}
