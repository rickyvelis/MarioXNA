using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorldXna
{
    public class HUD : GameObject
    {
        private string assetName;

        public HUD(Vector2 aStartPosition)
        {
            assetName = "images/HUD";
            mSpritePosition = aStartPosition;
        }
        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, assetName);
            //De coordinaten van het plaatje die die moet tekenen
            Source = new Rectangle(0, 0, 400, 32);
        }
    }
}
