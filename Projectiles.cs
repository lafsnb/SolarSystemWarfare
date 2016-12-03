using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Drawing;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace SolarSystemWarfare
{

    enum Direction { UP, DOWN, LEFT, RIGHT };

    class Projectiles : Sprite
    {

        private Direction Direction { get; }
        private int Damage { get; }
        private Enemy[] EnemiesHit { get; }

        public Projectiles(int x, int y, double width, double height, double speed, int durability, Rectangle rect,
            Direction direction, int damage) 
            : base(x, y, width, height, speed, durability, rect)
        {
            Direction = direction;
            Damage = damage;
        }
    }
}
