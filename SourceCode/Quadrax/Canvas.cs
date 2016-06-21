using System;
using System.Collections;
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
        public string TestLevelString = Properties.Resources.TestLevel;

        Image BACKGROUND = Properties.Resources.bg1;
        Timer gameTimer = new Timer();
        int VELKOSTCHARAKTERU = 50;
        int VELKOSTOBJEKTU = 25;
        int VELKOSTKROKU = 5;
        string levelName;
        LEVEL level;
        private Button restartButton;
        public Player activeCharacter;
        private Button leftButton;
        private Button rightButton;
        private Button selectButton;
        private Button menuButton;
        private TextBox text;
        private List<KeyValuePair<string,string>> menuLevels;
        private int index;
        public MyCanvas()
        {
            menuLevels = new List<KeyValuePair<string, string>>();
            menuLevels.Add(new KeyValuePair<string, string>(Properties.Resources.level2, "Level 2"));
            menuLevels.Add(new KeyValuePair<string, string>(Properties.Resources.TestLevel, "Testovaci Level"));
            index = 0;
            this.levelName = menuLevels[0].Key;
            InitializeComponent();
            BackgroundImage = BACKGROUND;
            DoubleBuffered = true;
            this.Height = 600;
            this.Width = 800;
            this.TransparencyKey = Color.Transparent;
            this.KeyPreview = true; //KeyDown works thnx to this
            
            //
            //testing space


            //
            //Load(levelName);
            //Redraw();
            //Refresh();

            //init konkretnych typov objektov, aby sa neprepocitavali pri kazdom hracovom move-e v leveli
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
            restartButton.Visible = false;
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
            //MENU
            leftButton = new System.Windows.Forms.Button();
            leftButton.Name = "leftButton";
            leftButton.Text = "Left";
            leftButton.Location = new Point(100, 100);
            leftButton.Click += new System.EventHandler(this.leftButton_click);
            Controls.Add(leftButton);

            rightButton = new System.Windows.Forms.Button();
            rightButton.Name = "rightButton";
            rightButton.Text = "Right";
            rightButton.Location = new Point(400, 100);
            rightButton.Click += new System.EventHandler(this.rightButton_click);
            Controls.Add(rightButton);


            selectButton = new System.Windows.Forms.Button();
            selectButton.Name = "selectButton";
            selectButton.Text = "Select Level";
            selectButton.Location = new Point(260, 125);
            selectButton.Click += new System.EventHandler(this.selectButton_click);
            Controls.Add(selectButton);

            menuButton = new System.Windows.Forms.Button();
            menuButton.Name = "menuButton";
            menuButton.Text = "To Menu";
            menuButton.Location = new Point(625, 42);
            menuButton.Click += new System.EventHandler(this.menuButton_click);
            Controls.Add(menuButton);
            menuButton.Visible = false;

            text = new TextBox();
            text.Location = new Point(200, 100);
            text.Width = 175;
            text.Text = menuLevels[0].Value;
            text.TextAlign = HorizontalAlignment.Center;
            text.BackColor = Color.LightGray;
            Controls.Add(text);
            //KONIEC MENU

        }

        
        //MENU - button f-cie
        private void menuButton_click(object sender, EventArgs e)
        {
            this.Controls.Remove(p1);
            this.Controls.Remove(p2);
            while (objects.Count > 0)
            {
                RemoveObject(objects[0]);
            }
            showMenu();
        }

        private void showMenu()
        {
            text.Visible = true;
            selectButton.Visible = true;
            rightButton.Visible = true;
            leftButton.Visible = true;
            restartButton.Visible = false;
            menuButton.Visible = false;
        }

        private void selectButton_click(object sender, EventArgs e)
        {
            text.Visible = false;
            selectButton.Visible = false;
            rightButton.Visible = false;
            leftButton.Visible = false;
            restartButton.Visible = true;
            menuButton.Visible = true;
            Load(levelName);
            Redraw();
            Refresh();
        }

        private void leftButton_click(object sender, EventArgs e)
        {
            if (index == 0)
            {
                index = menuLevels.Count - 1;
            }
            else {
                index--;
            }
            levelName = menuLevels[index].Key;
            text.Text = menuLevels[index].Value;
        }

        private void rightButton_click(object sender, EventArgs e)
        {
            index = (index + 1) % menuLevels.Count;
            levelName = menuLevels[index].Key;
            text.Text = menuLevels[index].Value;
        }


        //KONIEC MENU - button f-cie

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
                    activeCharacter.Move(keyData, VELKOSTKROKU, objects);

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
                applyGravity();
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
            activeCharacter = activeCharacter.Equals(p1) ? p2 : p1;

            activeCharacter.BringToFront();
        }

        public LEVEL ParseLevel(string content)
        {
            XmlSerializer ser = new XmlSerializer(typeof(LEVEL));
            LEVEL l;
            using (TextReader reader = new StringReader(content))
            {
                l = (LEVEL)ser.Deserialize(reader);
            }
            return l;
        }

        public void Load(string content)
        {
            level = ParseLevel(content);

            if (level.OBJEKTY.BALVAN != null) {
                foreach (var item in level.OBJEKTY.BALVAN)
                {
                    Boulder tmp = new Boulder(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT, VELKOSTOBJEKTU);
                    AddObject(tmp);
                }
            }
            if (level.OBJEKTY.PICHLIACE != null) {
                foreach (var item in level.OBJEKTY.PICHLIACE)
                {
                    //TODO nastavit na balvany, rebriky, atd., nie na gameObjecty
                    //GameObject tmp = new GameObject(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                    //arrayGameObjects.Add(tmp);
                }
            }
            if (level.OBJEKTY.PREPINAC != null)
            { 
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
            }
            if (level.OBJEKTY.REBRIK != null)
            { 
                foreach (var item in level.OBJEKTY.REBRIK)
                {
                    Ladder tmp = new Ladder(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT, item.VELKOST.VYSKA, this);
                    AddObject(tmp);
                }
            }
            if (level.OBJEKTY.STENA != null)
            { 
                foreach (var item in level.OBJEKTY.STENA)
                {
                    Brick tmp = new Brick(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                    AddObject(tmp);
                }
            }
            if (level.OBJEKTY.STENY != null)
            { 
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
            }
            if (level.OBJEKTY.VYCHOD != null)
            { 
                foreach (var item in level.OBJEKTY.VYCHOD)
                {
                    Exit ex = new Exit(item.SURADNICE.X, item.SURADNICE.Y, item.SOLID, item.WEIGHT);
                    AddObject(ex);
                }
            }

            p1 = new Player(level.SPAWN.X1, level.SPAWN.Y1, 20, VELKOSTCHARAKTERU, 1);
            p2 = new Player(level.SPAWN.X2, level.SPAWN.Y2, 100, VELKOSTCHARAKTERU, 2);
            
            activeCharacter = p1;
            applyGravity();
        }

        private void applyGravity()
        {

            PlayerGravity();
            SwitchPlayer();
            PlayerGravity();
            SwitchPlayer();

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

        public void BoulderGravity(GameObject boulder)
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
