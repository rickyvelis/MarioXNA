using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class ScoreTest
    {
        [TestMethod]
        public void Score_ContructorTest()
        {
            Score score = new Score(new Vector2(0, 0), 1);

            Assert.AreEqual(score.mSpritePosition, new Vector2(0, 0));
        }

        [TestMethod]
        public void Score_UpdateCounterTest()
        {
            Score score1 = new Score(new Vector2(0, 0), 1);
            Score score2 = new Score(new Vector2(0, 0), 2);
            score1.UpdateCounter(12);
            score2.UpdateCounter(12);

            Assert.AreEqual(score1.Source, new Rectangle(32, 0, 16, 14));
            Assert.AreEqual(score2.Source, new Rectangle(16, 0, 16, 14));
        }
    }
}
