using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Quadrax
{
    partial class MyCanvas : Form
    {
        Player p1;
        List<GameObject> objects = new List<GameObject>();

        Image BACKGROUND = Properties.Resources.bg1;

        Panel canvas;
        Graphics g;
        Timer gameTimer = new Timer();
        int VELKOSTCHARAKTERU = 50;
        int VELKOSTOBJEKTU = 20;
        int VELKOSTKROKU = 5;
        private Button restartButton;
        LEVEL level;
        Player activeCharacter;

        public MyCanvas()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.Height = 600;
            this.Width = 800;
            g = canvas.CreateGraphics();
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, canvas, new object[] { true });
            p1 = new Player(0, 0, 60, VELKOSTCHARAKTERU);
            activeCharacter = p1;
            Load(Properties.Resources.level);
            Redraw();
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
            //toto fixnut
            canvas.BackgroundImage = this.BackgroundImage = BACKGROUND;
            g.DrawImage(canvas.BackgroundImage,0,0,this.Width,this.Height);
            p1.Draw(g);
            foreach (GameObject gobj in objects)
             {
                 gobj.Draw(g);
             }
        }


        private void InitializeComponent()
        {
            this.canvas = new System.Windows.Forms.Panel();
            this.restartButton = new System.Windows.Forms.Button();
            this.canvas.SuspendLayout();
            this.SuspendLayout();
            // 
            // canvas
            // 
            this.canvas.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.canvas.Controls.Add(this.restartButton);
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(737, 430);
            this.canvas.TabIndex = 0;
            this.canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.MyCanvas_Paint);
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(768, 36);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(75, 23);
            this.restartButton.TabIndex = 0;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Visible = false;
            this.restartButton.Click += new System.EventHandler(this.restartButton_click);
            // 
            // MyCanvas
            // 
            this.ClientSize = new System.Drawing.Size(737, 430);
            this.Controls.Add(this.canvas);
            this.Name = "MyCanvas";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MyCanvas_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MyCanvas_KeyDown);
            this.canvas.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void MyCanvas_Paint(object sender, PaintEventArgs e)
        {
        }

        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            if (pohyb(e))
            {
                p1.Move(e, g, VELKOSTKROKU);
                Redraw();
                e.Handled = true;
            }
        }


        public void Load(string content)
        {
            XmlSerializer ser = new XmlSerializer(typeof(LEVEL));
            using (TextReader reader = new StringReader(content))
            {
                level = (LEVEL)ser.Deserialize(reader);
            }
            
            p1.X = level.SPAWN.X;
            p1.Y = level.SPAWN.Y;

            foreach (var item in level.OBJEKTY.BALVAN)
            {
                Boulder tmp = new Boulder(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                objects.Add(tmp);
            }
            foreach (var item in level.OBJEKTY.PICHLIACE)
            {
                //TODO nastavit na balvany, rebriky, atd., nie na gameObjecty
                //GameObject tmp = new GameObject(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                //arrayGameObjects.Add(tmp);
            }
            foreach (var item in level.OBJEKTY.PREPINAC)
            {
                //TODO nastavit na balvany, rebriky, atd., nie na gameObjecty
                //GameObject tmp = new GameObject(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                //arrayGameObjects.Add(tmp);
            }
            foreach (var item in level.OBJEKTY.REBRIK)
            {
                //TODO nastavit na balvany, rebriky, atd., nie na gameObjecty
                //GameObject tmp = new GameObject(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                //arrayGameObjects.Add(tmp);
            }
            foreach (var item in level.OBJEKTY.STENA)
            {
                //TODO nastavit na balvany, rebriky, atd., nie na gameObjecty
                Brick tmp = new Brick(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                objects.Add(tmp);
            }
            foreach (var item in level.OBJEKTY.VYCHOD)
            {
                Exit ex = new Exit(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                objects.Add(ex);
            }


        }
        public void Save()
        {
            List<LEVELOBJEKTYBALVAN> balvany = new List<LEVELOBJEKTYBALVAN>();
            foreach (GameObject item in objects)
            {
                //TODO implementovat ked budeme mat objekty
                if (item.GetType() == typeof(Boulder))
                {
                    LEVELOBJEKTYBALVAN tmp = new LEVELOBJEKTYBALVAN();
                    tmp.SOLID = item.isSolid();
                    tmp.WEIGHT = item.getWeight();
                    tmp.SURADNICE.X = item.X;
                    tmp.SURADNICE.Y = item.Y;
                    balvany.Add(tmp);
                }

            }
        }
        public bool pohyb(KeyEventArgs key)
        {
            switch (key.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    break;
                default:
                    return false;
            }
            foreach (var obj in objects)
            {
                if (SameRowOrColumn(key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? activeCharacter.X : activeCharacter.Y, key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? obj.X : obj.Y))
                {
                    if (Overlap(key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? activeCharacter.Y : activeCharacter.X, key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? obj.Y : obj.X, key.KeyCode == Keys.Up || key.KeyCode == Keys.Left ? -VELKOSTKROKU : VELKOSTKROKU))
                    {
                        bool res = resolveAdjacent(obj,key);
                        if(!res){
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private bool resolveAdjacent(GameObject obj, KeyEventArgs key)
        {
            if (obj.GetType() == typeof(Boulder) || obj.GetType() == typeof(Brick))
            {
                //ma hrac dostatocnu silu na pohnutie kamenom?
                if (obj.canPush(activeCharacter.Strength))
                {
                    if (pohybBouldra(key, obj))
                    {
                        obj.X += key.KeyCode == Keys.Left ? -VELKOSTKROKU : VELKOSTKROKU;
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else if (obj.GetType() == typeof(Exit))
            {
                var x = (Exit)obj;
                MessageBox.Show("Vyhral si!");
                Application.Exit();
            }
            return true;
        }
        public bool pohybBouldra(KeyEventArgs key, GameObject currentObject)
        {
            switch (key.KeyCode)
            {
                case Keys.Up:
                case Keys.Down:
                case Keys.Left:
                case Keys.Right:
                    break;
                default:
                    return false;
            }
            foreach (var obj in objects)
            {
                if (obj.Equals(currentObject))
                {
                    continue;
                }
                if (SameRowOrColumn(key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? currentObject.X : currentObject.Y, key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? obj.X : obj.Y))
                {
                    if (!obj.isSolid() || Overlap(key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? currentObject.Y : currentObject.X, key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? obj.Y : obj.X, key.KeyCode == Keys.Up || key.KeyCode == Keys.Left ? -VELKOSTKROKU : VELKOSTKROKU))
                    {
                        switch (key.KeyCode)
                        {
                            case Keys.Up:
                            case Keys.Down:
                                //if (obj.GetType() == typeof(rebrik))
                                //{
                                //    return true;
                                //}
                                return false;
                            case Keys.Left:
                            case Keys.Right:
                                if (obj.GetType() == typeof(Boulder))
                                {
                                    return false;
                                }
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            return true;
        }

        //skontroluje ci je v rovnakom riadku alebo stlpci
        public bool SameRowOrColumn(int ch, int obj)
        {
            if (((ch >= obj && ch <= obj + VELKOSTOBJEKTU) ||
                (ch + VELKOSTOBJEKTU >= obj && ch + VELKOSTOBJEKTU <= obj + VELKOSTOBJEKTU)) || 
               ((obj >= ch && obj <= ch + VELKOSTCHARAKTERU)||
                (obj + VELKOSTOBJEKTU >= ch && obj + VELKOSTOBJEKTU <= ch + VELKOSTCHARAKTERU)))
            {
                return true;
            }
            return false;
        }
        
        //skontroluje ci by stupil na dany objekt
        public bool Overlap(int ch, int obj, int vk)
        {
            if (((ch + vk >= obj && ch + vk <= obj + VELKOSTOBJEKTU) ||
                (ch + vk + VELKOSTOBJEKTU >= obj && ch + vk + VELKOSTOBJEKTU <= obj + VELKOSTOBJEKTU))||
               ((obj >= ch + vk && obj <= ch + VELKOSTCHARAKTERU + vk) ||
                (obj + VELKOSTOBJEKTU >= ch + vk && obj + VELKOSTOBJEKTU <= ch + vk + VELKOSTCHARAKTERU)))
            {
                return true;
            }
            return false;
        }

        private void restartButton_click(object sender, EventArgs e)
        {
//            Load("adresa");
        }
        public void setRestartButton(int x,int y) {
            restartButton.Location=new Point(x,y);
            restartButton.Visible = false;
        }
    }
}
