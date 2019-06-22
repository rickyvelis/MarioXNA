using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class CastleDoorTest
    {
        [TestMethod]
        public void CastleDoor_ConstructorTest()
        {
            CastleDoor door = new CastleDoor(new Vector2(0, 0));
            Assert.AreEqual(new Vector2(0, 0), door.mSpritePosition);
            Assert.AreEqual(new Rectangle(158, 64, 16, 32), door.Source);
        }
    }
}
