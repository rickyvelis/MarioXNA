using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SuperMarioWorldXna
{
    public class Tile
    {
        public Vector2 mPosition;
        public char mSymbol;

        public Tile(char theSymbol, Vector2 thePosition)
        {
            mSymbol = theSymbol;
            mPosition = thePosition;
        }
    }
}
