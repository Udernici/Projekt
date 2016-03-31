using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quadrax
{
    partial class MyCanvas : Form
    {
        Player p1 = new Player(0, 0, 10);
        Player p2 = new Player(10, 10, 20);
        
        List<GameObject> objects = new List<GameObject>();
        Panel canvas;
        Graphics g;
        Timer gameTimer = new Timer();

        public MyCanvas()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.Height = 600;
            this.Width = 800;
            g = canvas.CreateGraphics();
            //AddObject(new Boulder(10, 10, true, 10));

            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, canvas, new object[] { true });
            //Redraw();
            AddObject(new Boulder(30, 30, true, 20));
        }

        public void AddObject(GameObject o)
        {
            objects.Add(o);
            Invalidate();
        }

        public void RemoveObject(GameObject o)
        {
            objects.Remove(o);
            Invalidate();
        }

        public void Redraw()
        {
            //add graphic logic

            // pictureBox1.BackColor = Color.Black;
            g.Clear(Color.White);
            foreach (GameObject gobj in objects)
             {
                 gobj.Draw(g);
             }

        }

        public void Clear()
        {
            objects.Clear();
            Invalidate();
        }

        private void InitializeComponent()
        {
            this.canvas = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.Location = new System.Drawing.Point(24, 12);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(824, 485);
            this.canvas.TabIndex = 0;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.MyCanvas_Paint);
            // 
            // MyCanvas
            // 
            this.ClientSize = new System.Drawing.Size(870, 509);
            this.Controls.Add(this.canvas);
            this.Name = "MyCanvas";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MyCanvas_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MyCanvas_KeyDown);
            this.ResumeLayout(false);

        }

        private void MyCanvas_Paint(object sender, PaintEventArgs e)
        {
            Redraw();
        }

        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.A)
            {
                objects[0].X += -10;
                Redraw();
                e.Handled = true;
            }
        }
    }
}
