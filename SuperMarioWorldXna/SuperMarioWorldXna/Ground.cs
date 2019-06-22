using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioWorldXna
{
    public class Ground : GameObject
    {
        private string assetName;

        public string Type { get; private set; }

        public Ground(Vector2 aStartPosition, string aType)
        {
            mSpritePosition = aStartPosition;
            assetName = "images/ground";
            Type = aType;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, assetName);
            GroundType(Type);
        }

        /// <summary>
        /// Kiest welk stukje uit de spritesheet er gedrawed moet worden 
        /// </summary>
        /// <param name="aType"></param>
        public void GroundType(string aType)
        {
            switch (aType)
            {
                case "NorthWest":
                    Source = new Rectangle(0, 0, 16, 16);
                    break;
                case "North":
                    Source = new Rectangle(16, 0, 16, 16);
                    break;
                case "NorthEast":
                    Source = new Rectangle(32, 0, 16, 16);
                    break;
                case "SouthWest":
                    Source = new Rectangle(0, 32, 16, 16);
                    break;
                case "South":
                    Source = new Rectangle(16, 32, 16, 16);
                    break;
                case "SouthEast":
                    Source = new Rectangle(32, 32, 16, 16);
                    break;
                case "Middle":
                    Source = new Rectangle(16, 16, 16, 16);
                    break;
                case "West":
                    Source = new Rectangle(0, 16, 16, 16);
                    break;
                case "East":
                    Source = new Rectangle(32, 16, 16, 16);
                    break;
                default:
                    Source = new Rectangle(16, 16, 16, 16);
                    break;
            }
        }
    }
}

