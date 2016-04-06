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
        }

        public override void Draw(Graphics g)
        {
            g.DrawImage(Image,X, this.Y,30,30);
        }
    }
}
