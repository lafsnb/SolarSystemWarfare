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
        private int X { get; set; }
        private int Y { get; set; }
        private int Speed { get; }
        private int Durability { get; }
        private Rectangle Rect { get; }
        //private Icon Icon { get; } //TODO
        private Image Icon { get; }

        public Sprite(int x, int y, int speed, int durability, 
            Rectangle rect, Image icon)
        {
            X = x;
            Y = y;
            Speed = speed;
            Durability = durability;
            Rect = rect;
            Icon = icon;
        }

        public void MoveX(int x)
        {
            X += x;
        }

        public void MoveY(int y)
        {
            Y += y;
        }

        

        public bool Destroyed()
        {
            return false; //TODO
        }
    }
}
