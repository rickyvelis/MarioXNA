using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SuperMarioWorldXna;
using Microsoft.Xna.Framework;

namespace SuperMarioWorldXnaTests
{
    [TestClass]
    public class TileTest
    {
        [TestMethod]
        public void Tile_ContructorTest()
        {
            Tile tile = new Tile('a', new Vector2(0, 0));

            Assert.AreEqual(tile.mSymbol, 'a');
            Assert.AreEqual(tile.mPosition, new Vector2(0, 0));
        }
    }
}
