using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Drawing;
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
        private System.Drawing.Rectangle Rect2 { get; }
        private double Width { get; }
        private double Height { get; }
        //private Icon Icon { get; } //TODO

        public Sprite(double x, double y, double width, double height, double speed, int durability, Rectangle rect)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Speed = speed;
            Durability = durability;
            Dead = false;
            Rect2 = new System.Drawing.Rectangle((int) x, (int) y, (int) width, (int) height);
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
            Dead = true;

        }
    }
}
