using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class LifeTest
    {
        [TestMethod]
        public void Life_ContructorTest()
        {
            Game1.Instance.Life = 3;
            Life life = new Life(new Vector2(0, 0));
            Assert.AreEqual(new Vector2(0, 0), life.mSpritePosition);
        }
    }
}
