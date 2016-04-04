using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quadrax
{
    class Player
    {
        int sila = -9999;
        private int X, Y = -9999;
        ImageList frames = new ImageList();
        int indexObrazku = 0;
        bool active = true;
        int MINOBRAZOK = 1;
        int POCETOBRAZKOV = 5;

        public Player(int x, int y, int sila, string[] adresy)
        {
            X = x;
            Y = y;
            this.sila = sila;
            foreach (string adresa in adresy)
            {
                string g = (System.IO.Directory.GetCurrentDirectory());
                Image image = Image.FromFile(adresa);
                frames.Images.Add(image);
            }

        }
        public int getX()
        {
            return X;
        }
        public int getY()
        {
            return Y;
        }
        public void Pohyb(KeyEventArgs key, Graphics g, int krok)
        {
            switch (key.KeyCode)
            {
                case Keys.Up:
                    Y -= krok;
                    posunFrame(0);
                    break;
                case Keys.Down:
                    posunFrame(1);
                    Y += krok;
                    break;
                case Keys.Left:
                    posunFrame(2);
                    X -= krok;
                    break;
                case Keys.Right:
                    posunFrame(3);
                    X += krok;
                    break;
                default:
                    break;
            }
        }


        public void Vykresli(Graphics g)
        {
            if (!active)
            {
                frames.Draw(g, new Point(X, Y), 0);

                // Call Application.DoEvents to force a repaint of the form.
                Application.DoEvents();

            }
            else
            {
                frames.Draw(g, new Point(X, Y), indexObrazku);

                // Call Application.DoEvents to force a repaint of the form.
                Application.DoEvents();
            }



        }

        private void posunFrame(int smerCislo)
        {
            if (indexObrazku >= 1 + MINOBRAZOK * smerCislo && indexObrazku < 1 + MINOBRAZOK * smerCislo + POCETOBRAZKOV)
            {
                indexObrazku = (indexObrazku + 1) % POCETOBRAZKOV + 1 + MINOBRAZOK * smerCislo;
            }
            else
            {
                indexObrazku = 1 + MINOBRAZOK * smerCislo;
            }
        }

    }
}
