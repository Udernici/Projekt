using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quadrax
{
    public class Player : PictureBox
    {
        int strength = -9999;
        int x, y = -9999;
        List<Image> left = new List<Image>();
        List<Image> right = new List<Image>();
        ImageList up = new ImageList();
        ImageList down = new ImageList();
        ImageList towards = new ImageList();
        char direction = 'q';
        int indexObrazku = 0;
        bool active = false;
        int POCETOBRAZKOV = 4;

        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }

        public int Strength { get { return this.strength; } set { this.strength = value; } }

        public Player(int x, int y, int strength, int imageSize, int playerNumber) : base()
        {
            X = x;
            Y = y;
            this.strength = strength;
            this.Size = new System.Drawing.Size(imageSize,imageSize);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            BackColor = Color.Transparent;
            this.Location = new Point(X, Y);
            this.Click += player_Click;

            if(playerNumber == 1)
            {
                //left
                Image image = Properties.Resources.PlayerL1;
                left.Add(image);
                image = Properties.Resources.PlayerL2;
                left.Add(image);
                image = Properties.Resources.PlayerL3;
                left.Add(image);
                image = Properties.Resources.PlayerL4;
                left.Add(image);
                //right
                image = Properties.Resources.PlayerR1;
                right.Add(image);
                image = Properties.Resources.PlayerR2;
                right.Add(image);
                image = Properties.Resources.PlayerR3;
                right.Add(image);
                image = Properties.Resources.PlayerR4;
                right.Add(image);
                //towards
                towards.Images.Add(image);

            }
            else if (playerNumber == 2)
            {
                //left
                Image image = Properties.Resources.Player2L1;
                left.Add(image);
                image = Properties.Resources.Player2L2;
                left.Add(image);
                image = Properties.Resources.Player2L3;
                left.Add(image);
                image = Properties.Resources.Player2L4;
                left.Add(image);
                //right
                image = Properties.Resources.Player2R1;
                right.Add(image);
                image = Properties.Resources.Player2R2;
                right.Add(image);
                image = Properties.Resources.Player2R3;
                right.Add(image);
                image = Properties.Resources.Player2R4;
                right.Add(image);
                //towards
                towards.Images.Add(Properties.Resources.Player2R4);

            }
            this.Image = right[0];

        }

        private void player_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.Location.ToString());
        }

        public void Move(Keys key, int step, List<GameObject> ladders)
        {
            if (key == Keys.Left || key == Keys.Right || key == Keys.A || key == Keys.D)
            {
                X = key == Keys.Left || key == Keys.A ? X - step : X + step;
                direction = key == Keys.Left || key == Keys.A ? 'L' : 'R';
                indexObrazku = (indexObrazku + 1) % POCETOBRAZKOV;
            }
            else if (key == Keys.Up || key == Keys.Down || key == Keys.S || key == Keys.W)
            {
                //ak je na rebriku, moze sa hybat hore/dole
                foreach (Ladder l in ladders)
                {
                    if (l.IsPlayerClose(this))
                    {
                        Y = (key == Keys.Up|| key == Keys.W) ? Y - step : Y + step;
                        //ak Y - step prekroci Y suradnicu rebriku, nastavi sa na Y suradnicu rebriku -> 
                        //inak totiz dracik bugoval a nevedel zliest z rebriku
                        if (l.Location.Y > Y)
                        {
                            Y = Location.Y;
                        }
                        direction = (key == Keys.Up || key == Keys.W) ? 'U' : 'D';
                    }
                }

                //ak je pred nim len jeden brick/boulder vysky jedna, moze sa hybat hore/dole
                //TODO
            }
            this.Location = new Point(X, Y);
        }

        public void setStandingImage()
        {
            indexObrazku = 0;
            Draw();
        }

        private void InitializeComponent()
        {
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // Player
            // 
            this.Size = new System.Drawing.Size(100, 100);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);

        }

        public void Draw()
        {
            try
            {
                if (!active)
                {
                    switch (direction)
                    {
                        case 'U':
                            //up.Draw(g, new Point(X, Y), indexObrazku);
                            break;
                        case 'D':
                            //down.Draw(g, new Point(X, Y), indexObrazku);
                            break;
                        case 'L':
                            this.Image = left[indexObrazku];
                            break;
                        case 'R':
                            this.Image = right[indexObrazku];
                            break;
                    }
                }
                else
                {
                    //towards.Draw(g, new Point(X, Y), 0);
                }

                // Call Application.DoEvents to force a repaint of the form.
                this.Invalidate();
                Application.DoEvents();
            }
            catch
            {
                MessageBox.Show("Nemam obrazok.");
            }
        }
    }
}
