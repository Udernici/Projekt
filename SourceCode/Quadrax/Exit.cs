﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    public class Exit : GameObject
    {
        public Exit(int x, int y, bool solid, int weight) : base(x, y, false, 0)
        {
            this.Image = Properties.Resources.exit;
            this.Size = new Size(50, 50);
        }
        public bool Escaped(Player player1, Player player2)
        {

            return (player1.Location.X > Location.X - player1.Width && 
                player1.Location.X < Location.X + player1.Width 
                && player1.Location.Y > Location.Y - player1.Height 
                && player1.Location.Y < Location.Y + player1.Height
                
                    && player2.Location.X > Location.X - player2.Width 
                    && player2.Location.X < Location.X + player2.Width 
                    && player2.Location.Y > Location.Y - player2.Height 
                    && player2.Location.Y < Location.Y + player2.Height);
        }

        public override void Draw()
        {
            this.Invalidate();
        }
    }
}
