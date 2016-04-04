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
        Player p1 = new Player(0, 0, 10,new string[6] { "PlayerL.png", "PlayerR.png", "PlayerL.png", "PlayerR.png", "PlayerL.png", "PlayerR.png" });
        List<GameObject> objects = new List<GameObject>();

        Image BACKGROUND = Image.FromFile("Bg.jpg");

        Panel canvas;
        Graphics g;
        Timer gameTimer = new Timer();
        int VELKOSTOBJEKTU = 300;
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
            //canvas.BackgroundImage = Image.FromFile("Bg.jpg");
            
           // g.Clear(Color.Transparent);
            p1.Vykresli(g);
            canvas.BackgroundImage = this.BackgroundImage = BACKGROUND;
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
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(0, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(870, 509);
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
                p1.Pohyb(e, g, 10);
                Redraw();
                e.Handled = true;
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
                //TODO nastavit na balvany, rebriky, atd., nie na gameObjecty
                //GameObject tmp = new GameObject(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                //arrayGameObjects.Add(tmp);
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
                //if (item.GetType() == typeof(Balvan))
                //{
                //    LEVELOBJEKTYBALVAN tmp = new LEVELOBJEKTYBALVAN();
                //    tmp.SOLID = item.isSolid();
                //    tmp.WEIGHT = item.getWeight();
                //    tmp.SURADNICE.X = item.X;
                //    tmp.SURADNICE.Y = item.Y;
                //    balvany.Add(tmp);
                //}

            }
        }
        public bool pohyb(KeyEventArgs key, Player currentCharacter, GameObject[] array)
        {
            switch (key.KeyCode)
            {
                case Keys.Up:
                    foreach (var obj in array)
                    {
                        if (obj.X / VELKOSTOBJEKTU == (currentCharacter.getX() - VELKOSTOBJEKTU / 2) / VELKOSTOBJEKTU && obj.Y / VELKOSTOBJEKTU == (currentCharacter.getY() + VELKOSTKROKU) / VELKOSTOBJEKTU)
                        {
                            //if (obj.gettype() == typeof(rebrik))
                            //{
                            //    return true;
                            //}
                            //else
                            //    return false;
                            return false;
                        }

                    }
                    break;
                case Keys.Down:
                    foreach (var obj in array)
                    {
                        if (obj.X / VELKOSTOBJEKTU == (currentCharacter.getX() - VELKOSTOBJEKTU / 2) / VELKOSTOBJEKTU && obj.Y / VELKOSTOBJEKTU == (currentCharacter.getY() + VELKOSTKROKU) / VELKOSTOBJEKTU)
                        {
                            //if (obj.gettype() == typeof(rebrik))
                            //{
                            //    return true;
                            //}
                            //else
                            //    return false;
                            return false;
                        }
                    }
                    break;
                case Keys.Left:
                    foreach (var obj in array)
                    {
                        if (obj.X / VELKOSTOBJEKTU == (currentCharacter.getX() - VELKOSTKROKU) / VELKOSTOBJEKTU && obj.Y / VELKOSTOBJEKTU == (currentCharacter.getY()) / VELKOSTOBJEKTU)
                        {
                            //if (obj.gettype() == typeof(stena))
                            //{
                            //    return false;
                            //}
                            // ak je balvan a ma vacsiu hmotnost ako charakter silu tiez false, inak pozreme ci mozme, ak ani balvan ani stena true

                            //else
                            //    return false;
                            return false;
                        }
                    }
                    break;
                case Keys.Right:
                    foreach (var obj in array)
                    {
                        if (obj.X / VELKOSTOBJEKTU == (currentCharacter.getX() + VELKOSTKROKU) / VELKOSTOBJEKTU && obj.Y / VELKOSTOBJEKTU == (currentCharacter.getY()) / VELKOSTOBJEKTU)
                        {
                            //if (obj.gettype() == typeof(rebrik))
                            //{
                            //    return true;
                            //}
                            //else
                            //    return false;
                            return false;
                        }

                    }
                    break;
                default:
                    break;
            }
            return false;
        }
    }


}
