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
                if (list[counter] == null)
                {
                    list.RemoveAt(counter);
                    loop(list);
                    Console.WriteLine("Ships were removed");
                    break;
                } 
            }
        }

    }
}
