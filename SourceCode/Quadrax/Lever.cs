using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    class Lever : GameObject
    {
        public Lever(int x, int y, bool solid, int weight):base(x, y, solid, weight)
        {
            Image = Properties.Resources.Lever_1;
            this.Height = 25;
            this.Width = 25;
        }

        public bool IsPlayerClose(Player p)
        {
            int x = 30;
            if (Enumerable.Range(Location.X - 70, Location.X + 70).Contains(p.X) && Enumerable.Range(Location.Y - 30, Location.Y + 30).Contains(p.Y))
            {
                return true;
            }
            return false;
        }

        public override void Draw()
        {
            Invalidate();
        }
    }
}
