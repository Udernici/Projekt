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
    public partial class MyCanvas : Form
    {
        Player p1;
        Player p2;
        List<GameObject> objects = new List<GameObject>();
        List<GameObject> ladders;

        Image BACKGROUND = Properties.Resources.bg1;
        Timer gameTimer = new Timer();
        int VELKOSTCHARAKTERU = 50;
        int VELKOSTOBJEKTU = 30;
        int VELKOSTKROKU = 5;

        LEVEL level;
        private Button restartButton;
        public Player activeCharacter;
        

        public MyCanvas()
        {
            InitializeComponent();
            BackgroundImage = BACKGROUND;
            DoubleBuffered = true;
            this.Height = 600;
            this.Width = 800;
            this.TransparencyKey = Color.Transparent;
            this.KeyPreview = true; //KeyDown works thnx to this

            Load(Properties.Resources.level);

            //
            //testing space

            Ladder l = new Ladder(200, 200, true, 999, 10, this);
            AddObject(l);

            Brick b = new Brick(100, 100, true, 50);
            AddObject(b);
            List<GameObject> add = new List<GameObject>();
            add.Add(b);

            Brick k = new Brick(200, 100, true, 50);
            List<GameObject> rem = new List<GameObject>();
            rem.Add(k);

            Lever tmp = new Lever(200, 450, true, 500, add, rem);
            AddObject(tmp);

            //
            Redraw();
            Refresh();

            //init konkretnych typov objektov, aby sa neprepocitavali pri kazdom hracovom move-e v leveli
            ladders = GetObjectsOfType(typeof(Ladder));
        }

        public void AddObject(GameObject o)
        {
            objects.Add(o);
            this.Controls.Add(o);
            Invalidate();
            o.Invalidate();
        }

        public void RemoveObject(GameObject o)
        {
            objects.Remove(o);
            this.Controls.Remove(o);
            Invalidate();
            o.Invalidate();
        }

        public void Redraw()
        {
            //add graphic logic
            p1.Draw();
            this.Controls.Add(p1);

            p2.Draw();
            this.Controls.Add(p2);
        }


        private void InitializeComponent()
        {
            this.restartButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // restartButton
            // 
            this.restartButton.Location = new System.Drawing.Point(625, 12);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(129, 26);
            this.restartButton.TabIndex = 0;
            this.restartButton.Text = "Restart Level";
            this.restartButton.UseVisualStyleBackColor = true;
            this.restartButton.Click += new System.EventHandler(this.restartButton_click);
            // 
            // MyCanvas
            // 
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(837, 430);
            this.Controls.Add(this.restartButton);
            this.Name = "MyCanvas";
            this.Click += new System.EventHandler(this.MyCanvas_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.MyCanvas_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.MyCanvas_KeyUp);
            this.ResumeLayout(false);

        }

        private void MyCanvas_Paint(object sender, PaintEventArgs e)
        {
        }
        
        //z nejakeho dovodu nefungoval KeyDown na sipky -> fix (nahrada za MyCanvas_KeyDown)
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            List<Keys> movKeys = new List<Keys>() { Keys.Left, Keys.Right, Keys.Up, Keys.Down };
            if (movKeys.Contains(keyData))
            {
                if (pohyb(keyData))
                {
                    activeCharacter.Move(keyData, VELKOSTKROKU, ladders);
                    Redraw();
                    return true;
                }
            }
            else if (keyData == Keys.E)
            {
                List<GameObject> levers = GetObjectsOfType(typeof(Lever));
                foreach (Lever l in levers) {
                    if (l.IsPlayerClose(activeCharacter))
                    {
                        l.ActivateLever(this);
                    }
                }
            }
            else if (keyData == Keys.Q)
            {
                SwitchPlayer();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public void SwitchPlayer()
        {
            if (activeCharacter.Equals(p1))
            {
                activeCharacter = p2;
            }
            else
            {
                activeCharacter = p1;
            }
        }

        public List<GameObject> GetObjectsOfType(Type type)
        {
            List<GameObject> res = new List<GameObject>();
            foreach (var obj in objects)
            {
                if (obj.GetType() == type)
                {
                    res.Add(obj);
                }
            }
            return res;
        }

        public void Load(string content)
        {
            XmlSerializer ser = new XmlSerializer(typeof(LEVEL));
            using (TextReader reader = new StringReader(content))
            {
                level = (LEVEL)ser.Deserialize(reader);
            }

            p1 = new Player(level.SPAWN.X1, level.SPAWN.Y1, 20, VELKOSTCHARAKTERU, 1);
            p2 = new Player(level.SPAWN.X2, level.SPAWN.Y2, 20, VELKOSTCHARAKTERU, 2);
            activeCharacter = p1;

            foreach (var item in level.OBJEKTY.BALVAN)
            {
                Boulder tmp = new Boulder(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT,VELKOSTOBJEKTU);
                AddObject(tmp);
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
                Lever tmp = new Lever(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT, new List<GameObject>(), new List<GameObject>());
                AddObject(tmp);
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
                AddObject(tmp);
            }
            foreach (var item in level.OBJEKTY.VYCHOD)
            {
                Exit ex = new Exit(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                AddObject(ex);
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
                    tmp.SURADNICE.X = item.Location.X;
                    tmp.SURADNICE.Y = item.Location.Y;
                    balvany.Add(tmp);
                }

            }
        }
        public bool pohyb(Keys key)
        {
            switch (key)
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
                if (SameRowOrColumn(key == Keys.Up || key == Keys.Down ? activeCharacter.Location.X : activeCharacter.Location.Y, key == Keys.Up || key == Keys.Down ? obj.Location.X : obj.Location.Y))
                {
                    if (Overlap(key == Keys.Up || key == Keys.Down ? activeCharacter.Location.Y : activeCharacter.Location.X, key == Keys.Up || key == Keys.Down ? obj.Location.Y : obj.Location.X, key == Keys.Up || key == Keys.Left ? -VELKOSTKROKU : VELKOSTKROKU))
                    {
                        bool res = resolveAdjacent(obj, key);
                        if (!res)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
        

        private bool resolveAdjacent(GameObject obj, Keys key)
        {
            if (obj.GetType() == typeof(Boulder) || obj.GetType() == typeof(Brick))
            {
                //ma hrac dostatocnu silu na pohnutie kamenom?
                if (obj.canPush(activeCharacter.Strength))
                {
                    if (pohybBouldra(key, obj))
                    {
                        int where = key == Keys.Left ? -VELKOSTKROKU : VELKOSTKROKU;
                        obj.Location = new Point(obj.Location.X + where, obj.Location.Y);
                        obj.Invalidate();

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
                if (x.Escaped(p1, p2))
                {
                    MessageBox.Show("Vyhral si!");
                    Application.Exit();
                }

            }
            return true;
        }

        public bool pohybBouldra(Keys key, GameObject currentObject)
        {
            switch (key)
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
                if (SameRowOrColumn(key == Keys.Up || key == Keys.Down ? currentObject.Location.X : currentObject.Location.Y, key == Keys.Up || key == Keys.Down ? obj.Location.X : obj.Location.Y))
                {
                    if (!obj.isSolid() || Overlap(key == Keys.Up || key == Keys.Down ? currentObject.Location.Y : currentObject.Location.X, key == Keys.Up || key == Keys.Down ? obj.Location.Y : obj.Location.X, key == Keys.Up || key == Keys.Left ? -VELKOSTKROKU : VELKOSTKROKU))
                    {
                        switch (key)
                        {
                            case Keys.Up:
                            case Keys.Down:
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
            this.Controls.Remove(p1);
            while(objects.Count > 0)
            {
                RemoveObject(objects[0]);
            }
            Load(Properties.Resources.level);
            Redraw();
            Invalidate();
            Refresh();
        }
        public void setRestartButton(int x,int y) {
            restartButton.Location=new Point(x,y);
            restartButton.Visible = false;
        }

        private void MyCanvas_Click(object sender, EventArgs e)
        {
           // MessageBox.Show("Events? pls?");
        }

        private void MyCanvas_KeyUp(object sender, KeyEventArgs e)
        {
            activeCharacter.setStandingImage();
        }
    }
}
