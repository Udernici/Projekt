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
        List<Image> left = new List<Image>();
        List<Image> right = new List<Image>();
        ImageList up = new ImageList();
        ImageList down = new ImageList();
        ImageList towards = new ImageList();
        char direction = 'q';
        int indexObrazku = 0;
        bool active = false;
        int POCETOBRAZKOV = 4;

        public int Strength { get { return this.strength; } set { this.strength = value; } }

        public Player(int x, int y, int strength, int imageSize, int playerNumber) : base()
        {

            this.strength = strength;
            this.Size = new System.Drawing.Size(imageSize,imageSize);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            BackColor = Color.Transparent;
            this.Location = new Point(x, y);
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

        public void Move(Keys key, int step, List<Ladder> ladders)
        {
            if (key.IsHorizotal())
            {
                Location = new Point(key.IsLeft() ? Location.X - step : Location.X + step, Location.Y);
                direction = key.IsLeft() ? 'L' : 'R';
                indexObrazku = (indexObrazku + 1) % POCETOBRAZKOV;
            }
            else if (key.IsVertical())
            {
                //ak je na rebriku, moze sa hybat hore/dole
                foreach (Ladder l in ladders)
                {
                    if (l.IsPlayerClose(this))
                    {
                        Location=new Point(Location.X,key.IsUp() ? Location.Y - step : Location.Y + step);
                        //ak Y - step prekroci Y suradnicu rebriku, nastavi sa na Y suradnicu rebriku -> 
                        //inak totiz dracik bugoval a nevedel zliest z rebriku

                        //if (l.Location.Y > Location.Y)
                        //{
                        //    this.Location = new Point(Location.X, l.Location.Y);
                        //}

                        direction = (key.IsDown()) ? 'U' : 'D';
                    }
                }

                //ak je pred nim len jeden brick/boulder vysky jedna, moze sa hybat hore/dole
                //TODO
            }
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
