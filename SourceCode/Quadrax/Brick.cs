using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    class Brick : GameObject
    {
        public Image Image { get; private set; }
        public Brick(int x,int y, bool solid, int weight):base(x, y, solid, weight)
        {
                Random rnd = new Random();
                Image = Properties.Resources.wall_01;
                Image = new Image[5] { Properties.Resources.wall_01, Properties.Resources.wall_02, Properties.Resources.wall_03, Properties.Resources.wall_04, Properties.Resources.wall_05 }[rnd.Next(0, 5)]; // creates a number between 0 and 4

        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(Image,X, this.Y,30,30);
        }
    }
}
