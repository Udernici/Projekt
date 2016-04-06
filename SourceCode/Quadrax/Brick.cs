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
        public string Image { get; private set; }

        public int X { get; set; }
        public int Y { get; set; }
        public Brick()
        {
            {
                Random rnd = new Random();
                Image = new string[5] { "Graphics/bricks/wall_01.png", "Graphics/bricks/wall_02.png", "Graphics/bricks/wall_03.png", "Graphics/bricks/wall_04.png", "Graphics/bricks/wall_05.png" }[rnd.Next(0, 5)]; // creates a number between 0 and 4
            }

        }
    }
}
