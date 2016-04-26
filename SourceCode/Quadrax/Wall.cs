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
        public Brick[][] Bricks { get; set; }
        //constructor
        public Wall(int x, int y, int width, int height, bool solid, int weight) : base(x, y, solid, weight)
        {
            /*Bricks = new Brick[width][];
            for (int i = 0; i < width; i++)
            {
                Bricks[i] = new Brick[height];
                for (int j = 0; j < height; j++)
                {
                    Bricks[i][j] = new Brick
                    {
                        X = i,
                        Y = j
                    };
                }
            }*/

        }


        public override void Draw()
        {
        }
    }
}
