using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;


namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class LevelScreenTest
    {
        [TestMethod]
        public void LevelScreen_EndGameTest()
        {

            Player player = new Player(new Vector2(0, 0));


            bool actual = player.IsDead;

            bool expected = false;

            Assert.AreEqual(expected, actual);


        }
    }
}
