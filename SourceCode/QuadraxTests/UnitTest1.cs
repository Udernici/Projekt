using System;
using Quadrax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

    }
}
