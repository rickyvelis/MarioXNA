using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class GoombaTest
    {
        [TestMethod]
        public void Goomba_FacingTest()
        {
            Goomba goomba = new Goomba(new Vector2(0, 0));

            bool actual = goomba.FacingRight;

            bool expected = false;

            Assert.AreEqual(expected, actual);
        }
    }
}
