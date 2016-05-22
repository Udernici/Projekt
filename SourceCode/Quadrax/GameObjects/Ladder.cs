using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    public class Ladder : GameObject
    {
        List<LadderPiece> ladderPieces = new List<LadderPiece>();
        public Ladder(int x, int y, bool solid, int weight, int height, MyCanvas canvasRef) :base(x, y, solid, weight)
        {
            this.Height = 25 * height;
            this.Width = 25;

            for (int i = 0; i < height; ++i)
            {
                LadderPiece l = new LadderPiece(x, y + i*25, solid, weight);
                ladderPieces.Add(l);
                this.Controls.Add(l);
            }
            AddObjects(canvasRef);
        }

        public bool IsPlayerClose(Player p)
        {
            if (Enumerable.Range(Location.X - 70, Location.X + 70).Contains(p.X) && Enumerable.Range(Location.Y, Location.Y + Height).Contains(p.Y))
            {
                return true;
            }
            return false;
        }

        internal void AddObjects(MyCanvas canvasRef)
        {
            foreach (LadderPiece p in ladderPieces)
            {
                canvasRef.AddObject(p);
            }
        }

        internal void RemoveObjects(MyCanvas canvasRef)
        {
            foreach (LadderPiece p in ladderPieces)
            {
                canvasRef.RemoveObject(p);
            }
        }
    }
}
