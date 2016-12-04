using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SolarSystemWarfare
{

    enum Direction { UP, DOWN, LEFT, RIGHT };

    class Projectiles : Sprite
    {

        /*Tori changed Direction to public in order to access it in MainWindow*/
        public Direction Direction { get; }
        private int Damage { get; }
        private Enemy[] EnemiesHit { get; }

        public Projectiles(double x, double y, double speed, int durability, Rectangle rect,
            Direction direction, int damage)
            : base(x, y, speed, durability, rect)
        {
            Direction = direction;
            Damage = damage;

            /*Tori added dimensions for rectangle and initialized Rect*/
            rect.Height = 20;
            rect.Width = 5;
            Rect = rect;
        }
    }
}
