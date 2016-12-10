using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSystemWarfare
{
    static class RemoveShips
    {

        static public void remove(IList<Sprite> list) 
        {
            loop(list);
        }

        static private void loop(IList<Sprite> list)
        {
            for (int counter = 0; counter != list.Count; counter++)
            {
                if (list[counter].Dead)
                {
                    list[counter] = null;
                    list.RemoveAt(counter);
                    loop(list);
                    break;
                } 
            }
        }

        static public void CollisionCheck(IList<Sprite> list)
        {
            for(int counter1 = 0; counter1 != list.Count; counter1++)
            {
                for (int counter2 = 0; counter2 != list.Count; counter2++)
                {

                    if (!list[counter1].Dead && !list[counter2].Dead)
                    {
                        if (CheckProjectile(list[counter1], list[counter2]))
                        {
                            continue;
                        }
                        else if (CheckProjectile(list[counter2], list[counter1]))
                        {
                            continue;
                        }

                        if (CollisionHelper(list[counter1], list[counter2], 0, 0))
                        {
                            list[counter1].Destroy();
                            list[counter2].Destroy();
                            continue;
                        }
                        if (CollisionHelper(list[counter1], list[counter2], list[counter1].Rect.Width, 0))
                        {
                            list[counter1].Destroy();
                            list[counter2].Destroy();
                            continue;
                        }
                        if (CollisionHelper(list[counter1], list[counter2], 0, list[counter1].Rect.Height))
                        {
                            list[counter1].Destroy();
                            list[counter2].Destroy();
                            continue;
                        }
                        if (CollisionHelper(list[counter1], list[counter2], list[counter1].Rect.Width, list[counter1].Rect.Height))
                        {
                            list[counter1].Destroy();
                            list[counter2].Destroy();
                            continue;
                        }
                    }

                }
            }
        }

        static private bool CheckProjectile(Sprite sprite1, Sprite sprite2)
        {
            if (sprite1 is Projectiles)
            {
                if (sprite2 is Projectiles)
                {
                    return true;
                }
                else if (((Projectiles)sprite1).Direction == Direction.UP)
                {
                    if (sprite2 is Earth)
                    {
                        return true;
                    }
                }
                else if (((Projectiles)sprite1).Direction == Direction.DOWN)
                {
                    if (sprite2 is Enemy)
                    {
                        return true;
                    }
                }
            }

            if (sprite1.GetType() == sprite2.GetType())
            {
                return true;
            }
            return false;
        }

        static private bool CollisionHelper(Sprite sprite1, Sprite sprite2, double width, double height)
        {
            if (sprite1.X + width >= sprite2.X &&
                sprite1.X + width <= sprite2.X + sprite2.Rect.Width &&
                sprite1.Y + height >= sprite2.Y &&
                sprite1.Y + height <= sprite2.Y + sprite2.Rect.Height)
            {
                if ((sprite1 is Enemy && sprite2 is Projectiles) ||
                    (sprite1 is Projectiles && sprite2 is Enemy))
                {
                    Score.IncrementScore();
                }
                return true;
            }

            return false;
        }

    }
}
