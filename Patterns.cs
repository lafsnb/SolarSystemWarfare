using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarSystemWarfare
{
    abstract class Patterns
    {

        protected bool pathOne = true;
        protected bool pathTwo = false;
        protected int howFarDown;

        protected Sprite Sprite { get; set; }

        public Patterns(Sprite sprite, int howFarDown)
        {
            Sprite = sprite;
            this.howFarDown = howFarDown;
        }

        abstract public void runPattern();

    }
}
