using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class KoopaTest
    {
        [TestMethod]
        public void Koopa_FacingTest()
        {
            Koopa koopa = new Koopa(new Vector2(0, 0));

            bool actual = koopa.FacingRight;

            bool expected = false;

            Assert.AreEqual(expected, actual);
        }
    }
}
