using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioWorldXna
{
    public class Castle : GameObject
    {
        private string assetName;
        public string Type { get; private set; }

        public Castle(Vector2 aStartPosition, string aType)
        {
            assetName = "images/castle";
            mSpritePosition = aStartPosition;
            Type = aType;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, assetName);
            UpdateType(Type);
        }

        /// <summary>
        /// Een switch case om vanuit Levelscreen.cs 2 soorten Castels te laten laden een grote en een kleine
        /// </summary>
        /// <param name="aType"></param>
        public void UpdateType(string aType)
        {
            switch (aType)
            {
                case "Small":
                    Source = new Rectangle(158, 96, 94, 80);
                    break;
                case "Big":
                    Source = new Rectangle(0, 0, 158, 176);
                    break;
            }
        }
    }

}

