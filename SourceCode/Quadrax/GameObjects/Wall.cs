using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    class Wall : GameObject
    {
        List<Brick> brickPieces = new List<Brick>();
        //constructor
        public Wall(int x, int y, bool solid, int weight, int width, int height, MyCanvas canvasRef)
            : base(x, y, solid, weight)
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Brick l = new Brick(x + j * 25, y + i * 25, solid, weight);
                    brickPieces.Add(l);
                    this.Controls.Add(l);
                }
            }
            AddObjects(canvasRef);
        }

        void AddObjects(MyCanvas canvasRef)
        {
            foreach (Brick p in brickPieces)
            {
                canvasRef.AddObject(p);
            }
        }


        public override void Draw()
        {
        }
    }
}
