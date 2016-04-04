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
        ImageList left = new ImageList();
        ImageList right = new ImageList();
        ImageList up = new ImageList();
        ImageList down = new ImageList();
        ImageList towards = new ImageList();
        char direction = 'q';
        int indexObrazku = 0;
        bool active = false;
        int POCETOBRAZKOV = 2;

        public int getX()
        {
            return X;
        }
        public int getY()
        {
            return Y;
        }
        public Player(int x, int y, int sila, string[] adresy)
        {
            X = x;
            Y = y;
            this.sila = sila;
            for (int i = 0; i < adresy.Length; i++)
            {
                if (i < POCETOBRAZKOV)
                {
                    AddImage(adresy[i], left);
                }
                else if (i < POCETOBRAZKOV * 2)
                {
                    AddImage(adresy[i], right);
                }
                else if (i < POCETOBRAZKOV * 3)
                {
                    AddImage(adresy[i], up);
                }
                else if (i < POCETOBRAZKOV * 4)
                {
                    AddImage(adresy[i], down);
                }
                else
                {
                    AddImage(adresy[i], towards);
                }
            }
        }

        private void AddImage(string adresa, ImageList list)
        {
            string g = (System.IO.Directory.GetCurrentDirectory());
            Image image = Image.FromFile(adresa);
            list.Images.Add(image);
        }
        public void Move(KeyEventArgs key, Graphics g, int step)
        {
            switch (key.KeyCode)
            {
                case Keys.Up:
                    Y -= step;
                    direction = 'U';
                    break;
                case Keys.Down:
                    Y += step;
                    direction = 'D';
                    break;
                case Keys.Left:
                    X -= step;
                    direction = 'L';
                    break;
                case Keys.Right:
                    X += step;
                    direction = 'R';
                    break;
                default:
                    break;
            }

            indexObrazku = (indexObrazku + 1) % POCETOBRAZKOV;
        }


        public void Draw(Graphics g)
        {
            try
            {
                if (!active)
                {
                    switch (direction)
                    {
                        case 'U':
                            up.Draw(g, new Point(X, Y), indexObrazku);
                            break;
                        case 'D':
                            down.Draw(g, new Point(X, Y), indexObrazku);
                            break;
                        case 'L':
                            left.Draw(g, new Point(X, Y), indexObrazku);
                            break;
                        case 'R':
                            right.Draw(g, new Point(X, Y), indexObrazku);
                            break;
                    }
                }
                else
                {
                    towards.Draw(g, new Point(X, Y), 0);
                }

                // Call Application.DoEvents to force a repaint of the form.
                Application.DoEvents();
            }
            catch
            {
                MessageBox.Show("Nemam obrazok.");
            }
        }
    }
}
