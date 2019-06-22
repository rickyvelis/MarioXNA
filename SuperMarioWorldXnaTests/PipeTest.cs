using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class PipeTest
    {
        [TestMethod]
        public void Pipe_ConstructorTest()
        {
            Pipe pipe = new Pipe(new Vector2(0, 0), "aType");

            Vector2 actualPosition = pipe.mSpritePosition;
            string actualType = pipe.Type;

            Vector2 expectedPosition = new Vector2(0, 0);
            const string expectedType = "aType";
            Assert.AreEqual(actualPosition, expectedPosition);
            Assert.AreEqual(actualType, expectedType);
        }

        [TestMethod]
        public void Pipe_UpdateTypeTest()
        {
            Pipe pipe1 = new Pipe(new Vector2(0, 0), "aType");
            Pipe pipe2 = new Pipe(new Vector2(0, 0), "aType");
            Pipe pipe3 = new Pipe(new Vector2(0, 0), "aType");
            pipe1.UpdateType("Top");
            pipe2.UpdateType("Bottom");
            pipe3.UpdateType("");
            
            Rectangle expectedSource1 = new Rectangle(0, 0, 32, 32);
            Rectangle expectedSource2 = new Rectangle(0, 16, 32, 32);
            Rectangle expectedSource3 = Rectangle.Empty;

            Assert.AreEqual(pipe1.Source, expectedSource1);
            Assert.AreEqual(pipe2.Source, expectedSource2);
            Assert.AreEqual(pipe3.Source, expectedSource3);
        }
    }
}
