using System;
using Quadrax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace QuadraxTests
{
    [TestClass]
    public class UnitTest1
    {
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
            MyCanvas c = new MyCanvas();
            Player prvy = c.activeCharacter;
            c.SwitchPlayer();
            Assert.IsFalse(prvy.Equals(c.activeCharacter));
        }

        [TestMethod]
        public void SwitchPlayer2()
        {
            MyCanvas c = new MyCanvas();
            Player prvy = c.activeCharacter;
            c.SwitchPlayer();
            c.SwitchPlayer();
            Assert.IsTrue(prvy.Equals(c.activeCharacter));
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
            MyCanvas mc = new MyCanvas();
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
            int povodneX = p1.X;
            p1.Move(System.Windows.Forms.Keys.A, 10,new List<GameObject>());
            Assert.IsTrue(povodneX == p1.X + 10);

            povodneX = p1.X;
            p1.Move(System.Windows.Forms.Keys.Left, 10, new List<GameObject>());
            Assert.IsTrue(povodneX == p1.X + 10);
        }

        [TestMethod]
        public void TestMoveRight()
        {
            int povodneX = p1.X;
            p1.Move(System.Windows.Forms.Keys.D, 10, new List<GameObject>());
            Assert.IsTrue(povodneX == p1.X - 10);

            povodneX = p1.X;
            p1.Move(System.Windows.Forms.Keys.Right, 10, new List<GameObject>());
            Assert.IsTrue(povodneX == p1.X - 10);
        }

        [TestMethod]
        public void TestTemplate()
        {
            Assert.IsTrue(true);
        }
    }
}
