using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class Game1Test
    {
        [TestMethod]
        public void Game1_LivesTest()
        {
            Game1 game = new Game1();

            int actual = game.Life;

            int expected = 3;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Game1_CoinScoreTest()
        {
            Game1 game = new Game1();

            int actual = game.CoinScore;

            int expected = 0;

            Assert.AreEqual(expected, actual);

        }
    }
}
