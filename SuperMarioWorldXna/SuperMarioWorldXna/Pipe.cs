using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioWorldXna
{
    public class Pipe : GameObject
    {
        private string assetName;
        public string Type { get; private set; }

        public Pipe(Vector2 aStartPosition, string aType)
        {
            assetName = "images/pipe";
            mSpritePosition = aStartPosition;
            Type = aType;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, assetName);
            UpdateType(Type);
        }

        /// <summary>
        /// Een switch case om vanuit Levelscreen.cs 2 soorten Pipes te laten laden een onderkant en bovenkant
        /// </summary>
        /// <param name="aType"></param>
        public void UpdateType(string aType)
        {
            switch (aType)
            {
                case "Top":
                    Source = new Rectangle(0, 0, 32, 32);
                    break;
                case "Bottom":
                    Source = new Rectangle(0, 16, 32, 32);
                    break;
            }
        }
    }

}

