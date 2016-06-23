using System;
using Quadrax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace QuadraxTests
{
    [TestClass]
    public class UnitTest1
    {
        MyCanvas mc = new MyCanvas();
        Player p1 = new Quadrax.Player(100, 100, 100, 100, 1);
        Player p2 = new Quadrax.Player(10, 100, 100, 100, 2);

        [TestMethod]
        public void PlayerLeverClose()
        {
            Lever l = new Quadrax.Lever(100, 100, false, 20, null, null);
            Assert.IsTrue(l.IsPlayerClose(p1));
        }

        [TestMethod]
        public void PlayerLeverNotClose()
        {
            Lever l = new Quadrax.Lever(100, 100, false, 20, null, null);
            Assert.IsFalse(l.IsPlayerClose(p2));
        }

        [TestMethod]
        public void EscapedFalse()
        {
            Exit e = new Quadrax.Exit(-100, -100, true, 99999);
            Assert.IsFalse(e.Escaped(p1, p2));
        }

        [TestMethod]
        public void Escaped()
        {
            Exit e = new Quadrax.Exit(100, 100, true, 99999);
            Assert.IsTrue(e.Escaped(p1, p1));
        }
        
        [TestMethod]
        public void SwitchPlayer()
        {
            mc.Load(mc.TestLevelString);
            Player prvy = mc.activeCharacter;
            mc.SwitchPlayer();
            Assert.IsFalse(prvy.Equals(mc.activeCharacter));
        }

        [TestMethod]
        public void SwitchPlayer2()
        {
            mc.Load(mc.TestLevelString);
            Player prvy = mc.activeCharacter;
            mc.SwitchPlayer();
            mc.SwitchPlayer();
            Assert.IsTrue(prvy.Equals(mc.activeCharacter));
        }

        [TestMethod]
        public void Solid()
        {
            Brick g = new Brick(10, 10, true, 10);
            Assert.IsTrue(g.isSolid());
        }

        [TestMethod]
        public void NotSolid()
        {
            Brick b = new Brick(10, 10, false, 10);
            Assert.IsFalse(b.isSolid());
        }

        [TestMethod]
        public void LeverSwitch()
        {
            List<GameObject> act = new List<GameObject>();
            Brick b = new Brick(10, 10, false, 10);
            act.Add(b);
            List<GameObject> dsb = new List<GameObject>();

            List<GameObject> test = new List<GameObject>();
            test.AddRange(act);

            Lever l = new Lever(10, 10, false, 10, act, dsb);
            l.ActivateLever(mc);

            Assert.IsTrue(l.disabledObj[0] == test[0]);
        }

        [TestMethod]
        public void TestMoveLeft()
        {
            int povodneX = p1.Location.X;
            p1.Move(System.Windows.Forms.Keys.A, 10,new List<GameObject>());
            Assert.IsTrue(povodneX == p1.Location.X + 10);

            povodneX = p1.Location.X;
            p1.Move(System.Windows.Forms.Keys.Left, 10, new List<GameObject>());
            Assert.IsTrue(povodneX == p1.Location.X + 10);
        }

        [TestMethod]
        public void TestMoveRight()
        {
            int povodneX = p1.Location.X;
            p1.Move(System.Windows.Forms.Keys.D, 10, new List<GameObject>());
            Assert.IsTrue(povodneX == p1.Location.X - 10);

            povodneX = p1.Location.X;
            p1.Move(System.Windows.Forms.Keys.Right, 10, new List<GameObject>());
            Assert.IsTrue(povodneX == p1.Location.X - 10);
        }

        [TestMethod]
        public void TestPlayerGravity()
        {
            mc.Load(mc.TestLevelString);

            LEVEL lvl = mc.ParseLevel(mc.TestLevelString);
            int povodneY = lvl.SPAWN.X1;

            Assert.IsTrue(povodneY != mc.activeCharacter.Location.Y);
        }
        
        [TestMethod]
        public void TestBoulderGravitye()
        {
            mc.Load(mc.TestLevelString);
            Boulder b = new Boulder(100,100,true,10,10);
            int povodneY = b.Location.Y;
            mc.BoulderGravity(b);
            
            Assert.IsTrue(povodneY != b.Location.Y);
        }

        [TestMethod]
        public void PlayerLadderClose()
        {
            Ladder l = new Quadrax.Ladder(100, 100, false, 20, 5, mc);
            Assert.IsFalse(l.IsPlayerClose(p2));
        }

        [TestMethod]
        public void PlayerLadderNOTClose()
        {
            Ladder l = new Quadrax.Ladder(200, 100, false, 20, 5, mc);
            Assert.IsFalse(l.IsPlayerClose(p2));
        }

        [TestMethod]
        public void TestLadderClimbUp()
        {
            int povodneY = p1.Location.Y;
            MyCanvas c = new MyCanvas();
            Ladder l = new Ladder(150, 70, true, 10, 5, c);

            List<GameObject> obj = new List<GameObject>();
            obj.Add(l);

            p1.Move(System.Windows.Forms.Keys.Up, 10, obj);
            Assert.IsTrue(povodneY == p1.Location.Y + 10);

            p1.Move(System.Windows.Forms.Keys.W, 10, obj);
            Assert.IsTrue(povodneY == p1.Location.Y + 20);

        }

        [TestMethod]
        public void TestLadderClimbUpOverTheEdge()
        {
            int povodneY = p1.Location.Y;
            Ladder l = new Ladder(150, 100, true, 10, 5, mc);

            List<GameObject> obj = new List<GameObject>();
            obj.Add(l);

            p1.Move(System.Windows.Forms.Keys.Up, 10, obj);
            Assert.IsTrue(povodneY == p1.Location.Y);
            
        }

        [TestMethod]
        public void TestLadderClimbDown()
        {
            int povodneY = p1.Location.Y;
            Ladder l = new Ladder(150, 100, true, 10, 5, mc);

            List<GameObject> obj = new List<GameObject>();
            obj.Add(l);

            p1.Move(System.Windows.Forms.Keys.Down, 10, obj);
            Assert.IsTrue(povodneY == p1.Location.Y - 10);

            p1.Move(System.Windows.Forms.Keys.S, 10, obj);
            Assert.IsTrue(povodneY == p1.Location.Y - 20);
        }

        [TestMethod]
        public void PlayerStrongEnoughToMoveBoulder()
        {
            Boulder b = new Boulder(p1.Location.X + 10, p1.Location.Y, true, p1.Strength - 5, 10);
            Assert.IsTrue(b.canPush(p1.Strength));
        }

        [TestMethod]
        public void PlayerNOTStrongEnoughToMoveBoulder()
        {
            Boulder b = new Boulder(p1.Location.X + 10, p1.Location.Y, true, p1.Strength + 5, 10);
            Assert.IsFalse(b.canPush(p1.Strength));
        }

        [TestMethod]
        public void TwoBoulders()
        {
            mc.Load(mc.TestLevelString);
            Boulder b = new Boulder(100, 100, true, 30, 25);
            Boulder b1 = new Boulder(100, 125, true, 20, 25);
            int povodnyX1 = b1.Location.X;
            int povodnyX = b.Location.X;
            Player tmp = new Player(85, 125, 25, 40, 1);
            mc.activeCharacter = tmp;
            mc.AddObject(b);
            mc.AddObject(b1);

            bool res = mc.pohyb(System.Windows.Forms.Keys.D);
            Assert.IsTrue(b1.Location.X == povodnyX1 + 5); //VELKOSTKROKU
            Assert.IsTrue(b.Location.X == povodnyX);
        }

        [TestMethod]
        public void TwoBoulders2()
        {
            mc.Load(mc.TestLevelString);
            Boulder b = new Boulder(425, 425, true, 30, 25);
            Boulder b1 = new Boulder(425, 450, true, 20, 25);
            int povodnyX1 = b1.Location.X;
            int povodnyX = b.Location.X;
            Player tmp = new Player(375, 425, 500, 40, 1);
            mc.activeCharacter = tmp;
            mc.AddObject(b);
            mc.AddObject(b1);

            mc.pohyb(System.Windows.Forms.Keys.D);
            Assert.IsTrue(tmp.Location == tmp.Location);
            Assert.IsTrue(b1.Location.X == povodnyX1 + 5); //VELKOSTKROKU
            bool r = mc.pohyb(System.Windows.Forms.Keys.D);
            Assert.IsTrue(b.Location.X == povodnyX + 5);
        }

        [TestMethod]
        public void TestTemplate()
        {
            Assert.IsTrue(true);
        }
    }
}
