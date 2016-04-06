using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    public class Brick
    {
        public Image Image { get; private set; }

        public int X { get; set; }
        public int Y { get; set; }
        public Brick()
        {
            {
                Random rnd = new Random();
                Image = new Image[] { Properties.Resources.wall_01, Properties.Resources.wall_02, Properties.Resources.wall_03, Properties.Resources.wall_04, Properties.Resources.wall_05 }[rnd.Next(0, 5)]; // creates a number between 0 and 4
            }

        }
    }
}
