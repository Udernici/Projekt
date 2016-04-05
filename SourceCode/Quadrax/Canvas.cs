using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace Quadrax
{
    partial class MyCanvas : Form
    {
        Player p1 = new Player(0, 0, 60, new string[] { "PlayerR1.png", "PlayerR2.png", "PlayerR3.png", "PlayerR2.png", "PlayerR3.png", "PlayerR2.png", "PlayerR3.png", "PlayerR2.png", "PlayerR3.png", "PlayerR3.png"});
        List<GameObject> objects = new List<GameObject>();
        Panel canvas;
        Graphics g;
        Timer gameTimer = new Timer();
        int VELKOSTOBJEKTU = 20;
        int VELKOSTKROKU = 5;
        LEVEL level;

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
            Load("level.xml");
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
            p1.Draw(g);
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
            if (pohyb(e, p1, objects.ToArray()))
            {
                p1.Move(e, g, VELKOSTKROKU);
                Redraw();
                e.Handled = true;
            }
        }


        public void Load(string adresa)
        {

            XmlSerializer ser = new XmlSerializer(typeof(LEVEL));
            using (XmlReader reader = XmlReader.Create(adresa))
            {
                level = (LEVEL)ser.Deserialize(reader);
            }

            foreach (var item in level.OBJEKTY.BALVAN)
            {
                Boulder tmp = new Boulder(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                objects.Add(tmp);
                tmp = new Boulder(450, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
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
                //GameObject tmp = new GameObject(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                //arrayGameObjects.Add(tmp);
            }
            foreach (var item in level.OBJEKTY.VYCHOD)
            {
                //TODO nastavit na balvany, rebriky, atd., nie na gameObjecty
                //GameObject tmp = new GameObject(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                //arrayGameObjects.Add(tmp);
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
        public bool pohyb(KeyEventArgs key, Player currentCharacter, GameObject[] array)
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
            foreach (var obj in array)
            {
                if (SameRowOrColumn(key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? currentCharacter.X : currentCharacter.Y, key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? obj.X : obj.Y))
                {
                    if (!obj.isSolid() || Overlap(key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? currentCharacter.Y : currentCharacter.X, key.KeyCode == Keys.Up || key.KeyCode == Keys.Down ? obj.Y : obj.X, key.KeyCode == Keys.Up || key.KeyCode == Keys.Left ? -VELKOSTKROKU : VELKOSTKROKU))
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
                                    //ma hrac dostatocnu silu na pohnutie kamenom?
                                    if (obj.canPush(currentCharacter.Strength))
                                    {
                                        if (pohybBouldra(key, obj, array))
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
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            return true;
        }
        public bool pohybBouldra(KeyEventArgs key, GameObject currentObject, GameObject[] array)
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
            foreach (var obj in array)
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
            if (((ch >= obj) && (ch <= obj + VELKOSTOBJEKTU)) ||
                ((ch + VELKOSTOBJEKTU >= obj) && (ch + VELKOSTOBJEKTU <= obj + VELKOSTOBJEKTU)))
            {
                return true;
            }
            return false;
        }
        
        //skontroluje ci by stupil na dany objekt
        public bool Overlap(int ch, int obj, int vk)
        {
            if (((ch + vk >= obj) && (ch + vk <= obj + VELKOSTOBJEKTU)) ||
                ((ch + vk + VELKOSTOBJEKTU >= obj) && (ch + vk + VELKOSTOBJEKTU <= obj + VELKOSTOBJEKTU)))
            {
                return true;
            }
            return false;
        }
    }
}
