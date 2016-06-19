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
    public static class KeyboardHelper
    {
        public static bool IsUp(this Keys keys) => keys == Keys.Up || keys == Keys.W;
        public static bool IsDown(this Keys keys) => keys == Keys.Down || keys == Keys.S;
        public static bool IsLeft(this Keys keys) => keys == Keys.Left || keys == Keys.A;
        public static bool IsRight(this Keys keys) => keys == Keys.Right || keys == Keys.D;



        public static bool IsHorizotal(this Keys keys) => IsLeft(keys) || IsRight(keys);
        public static bool IsVertical(this Keys keys) => IsUp(keys) || IsDown(keys);

        public static bool IsMovement(this Keys keys) => IsVertical(keys) || IsHorizotal(keys);
    }

    public partial class MyCanvas : Form
    {
        Player p1;
        Player p2;
        List<GameObject> objects = new List<GameObject>();
        List<Ladder> ladders;

        Image BACKGROUND = Properties.Resources.bg1;
        Timer gameTimer = new Timer();
        int VELKOSTCHARAKTERU = 50;
        int VELKOSTOBJEKTU = 25;
        int VELKOSTKROKU = 5;
        string levelName;
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
            this.levelName = Properties.Resources.level2;
            Load(levelName);

            //
            //testing space

            //
            Redraw();
            Refresh();

            //init konkretnych typov objektov, aby sa neprepocitavali pri kazdom hracovom move-e v leveli
            ladders = objects.OfType<Ladder>().ToList();
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
            if (keyData.IsMovement())
            {
                if (pohyb(keyData))
                {
                    activeCharacter.Move(keyData, VELKOSTKROKU, ladders);

                    PlayerGravity();

                    Redraw();
                    return true;
                }
            }
            else if (keyData == Keys.E)
            {
                objects.OfType<Lever>()
                    .Where(l => l.IsPlayerClose(activeCharacter))
                    .ToList()
                    .ForEach(l => l.ActivateLever(this));
            }
            else if (keyData == Keys.Q)
            {
                SwitchPlayer();
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void PlayerGravity()
        {
            bool falling = true;
            while (falling)
            {
                falling = !objects.Any(playerIntersectsObject);
                if (falling)
                {
                    activeCharacter.Location = new Point(activeCharacter.Location.X, activeCharacter.Location.Y + 1);
                    Redraw();
                }

            }

        }

        private bool playerIntersectsObject(GameObject obj)
        {
            Rectangle actorRect = new Rectangle(new Point(activeCharacter.Location.X, activeCharacter.Location.Y + 1), new Size(VELKOSTCHARAKTERU, VELKOSTCHARAKTERU));
            Rectangle targetRect = new Rectangle(obj.Location, new Size(VELKOSTOBJEKTU, VELKOSTOBJEKTU));
            return (actorRect.IntersectsWith(targetRect));
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

        public void Load(string content)
        {
            XmlSerializer ser = new XmlSerializer(typeof(LEVEL));
            using (TextReader reader = new StringReader(content))
            {
                level = (LEVEL)ser.Deserialize(reader);
            }

            p1 = new Player(level.SPAWN.X1, level.SPAWN.Y1, 20, VELKOSTCHARAKTERU, 1);
            p2 = new Player(level.SPAWN.X2, level.SPAWN.Y2, 100, VELKOSTCHARAKTERU, 2);
            activeCharacter = p1;

            foreach (var item in level.OBJEKTY.BALVAN)
            {
                Boulder tmp = new Boulder(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT, VELKOSTOBJEKTU);
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
                Brick b = new Brick(item.OVLADA.XJEDNA, item.OVLADA.YJEDNA, true, 9999);
                AddObject(b);
                List<GameObject> add = new List<GameObject>();
                add.Add(b);

                Brick k = new Brick(item.OVLADA.XDVA, item.OVLADA.YDVA, true, 9999);
                List<GameObject> rem = new List<GameObject>();
                rem.Add(k);

                Lever tmp = new Lever(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT, add, rem);
                AddObject(tmp);
            }
            foreach (var item in level.OBJEKTY.REBRIK)
            {
                Ladder tmp = new Ladder(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT, item.VELKOST.VYSKA, this);
                AddObject(tmp);
            }
            foreach (var item in level.OBJEKTY.STENA)
            {
                Brick tmp = new Brick(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                AddObject(tmp);
            }
            foreach (var item in level.OBJEKTY.STENY)
            {
                for (int i = 0; i < item.VELKOST.VYSKA; i++)
                {
                    for (int j = 0; j < item.VELKOST.SIRKA; j++)
                    {
                        Brick l = new Brick(item.SURADNICE.X + j * 25, item.SURADNICE.Y + i * 25, item.SOLID, item.WEIGHT);
                        AddObject(l);
                    }
                }
                Wall tmp = new Wall(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT, item.VELKOST.SIRKA, item.VELKOST.VYSKA, this);
                //AddObject(tmp);
            }
            foreach (var item in level.OBJEKTY.VYCHOD)
            {
                Exit ex = new Exit(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                AddObject(ex);
            }
            applyGravity();

        }

        private void applyGravity()
        {
            activeCharacter = p2;
            PlayerGravity();
            activeCharacter = p1;
            PlayerGravity();

            foreach (GameObject obj in objects)
            {
                if (obj is Boulder)
                {
                    BoulderGravity(obj);
                }
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
        private Point getNewPosition(Keys key, Point point)
        {
            int newX = point.X;
            int newY = point.Y;

            if (key.IsUp()) { newY -= VELKOSTKROKU; }
            if (key.IsDown()) { newY += VELKOSTKROKU; }
            if (key.IsLeft()) { newX -= VELKOSTKROKU; }
            if (key.IsRight()) { newX += VELKOSTKROKU; }
            return new Point(newX, newY);

        }
        public bool pohyb(Keys key)
        {
            if (!key.IsMovement()) { return false; }
            var newPos = getNewPosition(key, activeCharacter.Location);

            return objects.Where(obj => colision(newPos, obj.Location))
                .All(obj => resolveAdjacent(obj, key));
        }

        private bool colision(Point actor, Point target, bool isTargetCharacter = true)
        {
            var actorSize = isTargetCharacter ? new Size(VELKOSTCHARAKTERU, VELKOSTCHARAKTERU) : new Size(VELKOSTOBJEKTU, VELKOSTOBJEKTU);
            Rectangle actorRect = new Rectangle(actor, actorSize);
            Rectangle targetRect = new Rectangle(target, new Size(VELKOSTOBJEKTU, VELKOSTOBJEKTU));
            return (actorRect.IntersectsWith(targetRect));
        }


        private bool resolveAdjacent(GameObject obj, Keys key)
        {
            if (obj is Brick)
            {
                return false;
            }
            else if (obj is Boulder)
            {
                if (!obj.canPush(activeCharacter.Strength)|| !pohybBouldra(key, obj))
                {
                    return false;
                }
                int where = key.IsLeft() ? -VELKOSTKROKU : VELKOSTKROKU;
                obj.Location = new Point(obj.Location.X + @where, obj.Location.Y);
                BoulderGravity(obj);
                obj.Invalidate();
                return false;
            }

            if (obj is Exit)
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
            if (!key.IsMovement() || key.IsVertical()) { return false; }
            var newPos = getNewPosition(key, currentObject.Location);
            if (BoulderPlayerColision(currentObject))
            {
                return false;
            }
            return !objects.Any(obj => colision(newPos, obj.Location, isTargetCharacter: false) && !obj.Equals(currentObject));
        }

        private bool BoulderPlayerColision(GameObject obj)
        {
            Rectangle p1 = new Rectangle(this.p1.Location, new Size(VELKOSTCHARAKTERU, VELKOSTCHARAKTERU));
            Rectangle p2 = new Rectangle(this.p2.Location, new Size(VELKOSTCHARAKTERU, VELKOSTCHARAKTERU));
            Rectangle objRect = new Rectangle(obj.Location, new Size(VELKOSTOBJEKTU, VELKOSTOBJEKTU));
            return (p1.IntersectsWith(objRect) || p2.IntersectsWith(objRect));
        }

        private void BoulderGravity(GameObject boulder)
        {
            bool falling = true;
            while (falling)
            {
                falling = !objects.Any(obj
                            => !boulder.Equals(obj)
                            && boulder.Location.Y + VELKOSTOBJEKTU == obj.Location.Y
                            && boulder.Location.X <= obj.Location.X + VELKOSTOBJEKTU
                            && boulder.Location.X >= obj.Location.X);
                if (falling)
                {
                    boulder.Location = new Point(boulder.Location.X, boulder.Location.Y + 1);
                    Redraw();
                }

            }

        }




        private void restartButton_click(object sender, EventArgs e)
        {
            this.Controls.Remove(p1);
            this.Controls.Remove(p2);
            while (objects.Count > 0)
            {
                RemoveObject(objects[0]);
            }
            Load(this.levelName);
            Redraw();
            Invalidate();
            Refresh();
        }
        public void setRestartButton(int x, int y)
        {
            restartButton.Location = new Point(x, y);
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
