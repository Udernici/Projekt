using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    class Boulder : GameObject
    {
        //constructor
        public Boulder(int x, int y, bool solid, int weight, int size) : base(x, y, true, weight)
        {
            Image = Properties.Resources.boulder_01;
            this.Size = new Size(size, size);
        }

        public override void Draw()
        {

        }

    }
}
