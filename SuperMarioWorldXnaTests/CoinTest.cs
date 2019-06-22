using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class CoinTest
    {
        [TestMethod]
        public void Coin_ConstructorTest()
        {
            Coin coin = new Coin(new Vector2(0, 0));
            Assert.AreEqual(new Vector2(0, 0), coin.mSpritePosition);
            Assert.IsFalse(coin.IsDead);
        }

        [TestMethod]
        public void Coin_DeathTest()
        {
            Coin coin = new Coin(new Vector2(0, 0));
            coin.Death();
            Assert.IsTrue(coin.IsDead);
        }
    }
}
