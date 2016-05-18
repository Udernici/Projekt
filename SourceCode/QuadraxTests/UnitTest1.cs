using System;
using Quadrax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QuadraxTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PlayerLeverClose()
        {
            Player p = new Quadrax.Player(100, 100, 100, 100);
            Lever l = new Quadrax.Lever(100, 100, false, 20, null, null);
            Assert.IsTrue(l.IsPlayerClose(p));
        }
        [TestMethod]
        public void PlayerLeverNotClose()
        {
            Player p = new Quadrax.Player(10, 100, 100, 100);
            Lever l = new Quadrax.Lever(100, 100, false, 20, null, null);
            Assert.IsFalse(l.IsPlayerClose(p));
        }
    }
}
