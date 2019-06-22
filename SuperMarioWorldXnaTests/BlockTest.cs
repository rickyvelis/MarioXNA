using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class BlockTest
    {
        [TestMethod]
        public void Block_ContstructorSpritePositionAndTypeTest()
        {
            Block block = new Block(new Vector2(0, 0), "aType");

            Vector2 actualPosition = block.mSpritePosition;
            string actualType = block.Type;

            Vector2 expectedPosition = new Vector2(0, 0);
            const string expectedType = "aType";
            Assert.AreEqual(actualPosition, expectedPosition);
            Assert.AreEqual(actualType, expectedType);
        }

        [TestMethod]
        public void Block_ContstructorSpritePositionAndTypeAndItemTest()
        {
            Block block = new Block(new Vector2(0, 0), "aType", "aItem");

            Vector2 actualPosition = block.mSpritePosition;
            string actualType = block.Type;
            string actualItem = block.Item;

            Vector2 expectedPosition = new Vector2(0, 0);
            const string expectedType = "aType";
            const string expectedItem = "aItem";
            Assert.AreEqual(actualPosition, expectedPosition);
            Assert.AreEqual(actualType, expectedType);
            Assert.AreEqual(actualItem, expectedItem);
        }

        [TestMethod]
        public void Block_DeathDefaultTest()
        {
            Block block = new Block(new Vector2(0, 0), "");

            bool actual = block.IsDead;

            bool expected = false;
            Assert.AreEqual(actual, expected);
        }
    }
}
