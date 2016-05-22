using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Quadrax
{
    public class LadderPiece : GameObject
    {
        public LadderPiece(int x, int y, bool solid, int weight) :base(x, y, solid, weight)
        {
            Image = Properties.Resources.ladder;
            this.Height = 25;
            this.Width = 25;
        }
    }
}
