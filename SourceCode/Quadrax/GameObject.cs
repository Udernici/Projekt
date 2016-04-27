﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quadrax
{
    abstract class GameObject : PictureBox
    {
        int x;
        int y;
        bool solid;
        int weight;
        public GameObject(int x, int y, bool solid, int weight) : base()
        {
            this.x = x;
            this.y = y;
            this.solid = solid;
            this.weight = weight;
            this.Location = new Point(x, y);
            DoubleBuffered = true;
            BackColor = Color.Transparent;
            this.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Click += GameObject_Click;
        }

        private void GameObject_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this.Location.ToString());
        }

        public int X { get { return this.x; } set { this.x = value; } }
        public int Y { get { return this.y; } set { this.y = value; } }

        //is collidable
        public bool isSolid()
        {
            return solid;
        }
        
        public bool canPush(int strength)
        {
            return strength >= weight;
        }
        public int getWeight()
        {
            return weight;
        }

        public virtual void Draw()
        {
            this.Invalidate();
        }

    }
}

