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


        public override void Draw(Graphics g)
        {
            /*            Bricks.Select(column => column.Select(brick => new
                        Rectangle(X + brick.X * 20, Y + brick.Y * 20, X + brick.X * 20 + 20, Y + brick.Y * 20 + 20)
                        )).
                        SelectMany(column => column.ToList())
                        .ToList()
                        .ForEach(
                            rectangle=> g.DrawRectangle(Pens.Black,rectangle));*/

            /*            foreach (var colum in Bricks)
                        {
                            foreach (var brick in colum) {
                                g.DrawRectangle(Pens.Black, new Rectangle(X + brick.X * 20, Y +brick.Y*20, X + brick.X*20 +20, Y + brick.Y*20 +20));
                            }
                        }

                    }*/
            /*for (int i = 0; i < Bricks.Length; i++)
            {
                for (int j = 0; j < Bricks[i].Length; j++)
                {
                     g.DrawRectangle(Pens.Black, new Rectangle(X + i * 20, Y + j * 20, 20, 20));
                }
            }*/
        }
    }
}
