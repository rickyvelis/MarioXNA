using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioWorldXna
{
    public class CastleDoor : GameObject
    {
        private string assetName;

        public CastleDoor(Vector2 aStartPosition)
        {
            assetName = "images/castleDoor";
            mSpritePosition = aStartPosition;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, assetName);
            //De coordinaten van de Sprite die die moet tekenen
        }
    }

}

