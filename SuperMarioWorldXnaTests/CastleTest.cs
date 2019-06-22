using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class CastleTest
    {
        [TestMethod]
        public void Castle_ConstructorTest()
        {
            Castle castle = new Castle(new Vector2(0, 0), "aType");
            
            Vector2 expectedPosition = new Vector2(0, 0);
            const string expectedType = "aType";

            Assert.AreEqual(castle.mSpritePosition, expectedPosition);
            Assert.AreEqual(castle.Type, expectedType);
        }

        [TestMethod]
        public void Castle_UpdateTypeTest()
        {

            Castle castle1 = new Castle(new Vector2(0, 0), "aType");
            Castle castle2 = new Castle(new Vector2(0, 0), "aType");
            Castle castle3 = new Castle(new Vector2(0, 0), "aType");
            castle1.UpdateType("Small");
            castle2.UpdateType("Big");
            castle3.UpdateType("");

            Rectangle expectedSource1 = new Rectangle(158, 96, 94, 80);
            Rectangle expectedSource2 = new Rectangle(0, 0, 158, 176);
            Rectangle expectedSource3 = Rectangle.Empty;

            Assert.AreEqual(castle1.Source, expectedSource1);
            Assert.AreEqual(castle2.Source, expectedSource2);
            Assert.AreEqual(castle3.Source, expectedSource3);
        }
    }
}
