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
                    Console.WriteLine("Ships were removed");
                    break;
                } 
            }
        }

        static public void CollisionCheck(IList<Sprite> list)
        {
            for(int counter1 = 0; counter1 != list.Count; counter1++)
            {
                for (int counter2 = counter1 + 1; counter2 != list.Count; counter2++)
                {
                    //if (list[counter1].X >= list[counter2].X && 
                    //    list[counter1].X <= list[counter2].X + list[counter2].Rect.Width &&
                    //    list[counter1].Y >= list[counter2].Y &&
                    //    list[counter1].Y <= list[counter2].Y + list[counter2].Rect.Height)
                    //{
                    //    list[counter1] = null;
                    //    list[counter2] = null;
                    //}
                    //if (list[counter1].X + list[counter1].Rect.Width >= list[counter2].X &&
                    //    list[counter1].X + list[counter1].Rect.Width <= list[counter2].X + list[counter2].Rect.Width &&
                    //    list[counter1].Y >= list[counter2].Y &&
                    //    list[counter1].Y <= list[counter2].Y + list[counter2].Rect.Height)
                    //{
                    //    list[counter1] = null;
                    //    list[counter2] = null;
                    //}

                    if (!list[counter1].Dead && !list[counter2].Dead)
                    {

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

        static private bool CollisionHelper(Sprite sprite1, Sprite sprite2, double width, double height)
        {
            if (sprite1.X + width >= sprite2.X &&
                sprite1.X + width <= sprite2.X + sprite2.Rect.Width &&
                sprite1.Y + height >= sprite2.Y &&
                sprite1.Y + height <= sprite2.Y + sprite2.Rect.Height)
            {
                Console.WriteLine("Collision detected");
                return true;
            }

            return false;
        }

    }
}
