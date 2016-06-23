using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    public class Lever : GameObject
    {
        public List<GameObject> activeObjs = new List<GameObject>();
        public List<GameObject> disabledObj = new List<GameObject>();
        bool on = true;
        public Lever(int x, int y, bool solid, int weight, List<GameObject> act, List<GameObject> dis) :base(x, y, solid, weight)
        {
            Image = on ? Properties.Resources.Lever_1 : Properties.Resources.Lever_2;
            this.Height = 25;
            this.Width = 25;
            this.activeObjs = act;
            this.disabledObj = dis;
        }

        public bool IsPlayerClose(Player p)
        {
            if (Enumerable.Range(Location.X - p.Width/2, Width + p.Width/2).Contains(p.Location.X) && Enumerable.Range(Location.Y - 30, Location.Y + 30).Contains(p.Location.Y))
            {
                return true;
            }
            return false;
        }

        public void ActivateLever(MyCanvas canvas)
        {
            List<GameObject> tmp = new List<GameObject>();
            tmp.AddRange(disabledObj);
            disabledObj.Clear();
            disabledObj.AddRange(activeObjs);
            activeObjs.Clear();
            activeObjs.AddRange(tmp);
            on = !on;
            Image = on ? Properties.Resources.Lever_1 : Properties.Resources.Lever_2;
            foreach (GameObject go in disabledObj)
            {
                canvas.RemoveObject(go);
            }
            foreach (GameObject go in activeObjs)
            {
                canvas.AddObject(go);
            }
        }

        public override void Draw()
        {
            Invalidate();
        }
    }
}
