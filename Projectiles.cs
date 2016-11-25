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

        private Direction Direction { get; }
        private int Damage { get; }
        private Enemy[] EnemiesHit { get; }

        public Projectiles(int x, int y, int speed, int durability, Rectangle rect,
            Image icon, Direction direction, int damage) 
            : base(x, y, speed, durability, rect, icon)
        {
            Direction = direction;
            Damage = damage;
        }
    }
}
