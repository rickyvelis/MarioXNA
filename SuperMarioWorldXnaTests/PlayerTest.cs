using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class PlayerTest
    {
        [TestMethod]
        public void Player_ConstructorTest()
        {
            Player player = new Player(new Vector2(0, 0));

            Vector2 actual = player.mSpritePosition;

            Vector2 expected = new Vector2(0, 0);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod] 
        public void Player_UpdateSizeBigTest()
        {
            Player player = new Player(new Vector2(0, 0));
            player.UpdateSize("Big");

            string actual = player.GetSize;

            const string expected = "Big";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Player_UpdateSizeSmallTest()
        {
            Player player = new Player(new Vector2(0, 0));
            player.UpdateSize("Small");

            string actual = player.GetSize;

            const string expected = "Small";
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Player_DeathTest()
        {
            Player player = new Player(new Vector2(0, 0));
            player.Death();
            Assert.IsTrue(player.IsDead);
        }

        [TestMethod]
        public void Player_MakeInvincibleTest()
        {
            Player player = new Player(new Vector2(0, 0));
            player.MakeInvincible();
            Assert.IsTrue(player.IsInvincible);
        }
    }
}
