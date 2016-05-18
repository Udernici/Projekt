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

        public Player(int x, int y, int strength, int imageSize) : base()
        {
            X = x;
            Y = y;
            this.strength = strength;
            this.Size = new System.Drawing.Size(imageSize,imageSize);
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            BackColor = Color.Transparent;
            this.Image = Properties.Resources.PlayerL1;
            this.Location = new Point(X, Y);
            this.Click += player_Click;

            //left
            Image image = Properties.Resources.PlayerL1;
            //left.Images.Add(image);
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

        private void player_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.Location.ToString());
        }

        public void Move(Keys key, int step)
        {
            switch (key)
            {
                case Keys.Up:
                    //Y -= step;
                    //direction = 'U';
                    break;
                case Keys.Down:
                    //Y += step;
                    //direction = 'D';
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
