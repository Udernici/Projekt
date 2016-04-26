using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quadrax
{
    class Exit : GameObject
    {
        public Image image = Properties.Resources.exit;
        public Exit(int x, int y, bool solid, int weight) : base(x, y, false, 0)
        {

        }
        public bool Escaped(Player player1) {
            return (player1.X > X -20 && player1.X < X + 20 && player1.Y > Y - 20 && player1.Y < Y + 20);
                    //&& player2.X > X && player2.X < X + 20 && player2.Y > Y-20 && player2.Y < Y + 20);
                    //zatial zakomentovane, lebo nemame este druheho hraca
        }

        public override void Draw()
        {
        }
    }
}
