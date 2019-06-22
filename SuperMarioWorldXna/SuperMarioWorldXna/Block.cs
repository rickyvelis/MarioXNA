using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioWorldXna
{
    public class Block : GameObject
    {
        private string assetName;
        private float elapsed;
        private float delay;
        private int frames;

        public bool IsDead { get; set; }

        public string Type { get; set; }

        public string Item { get; private set; }

        public Block(Vector2 aStartPosition, string aType)
        {
            mSpritePosition = aStartPosition;
            Type = aType;
        }

        public Block(Vector2 aStartPosition, string aType, string aItem)
        {
            mSpritePosition = aStartPosition;
            Type = aType;
            Item = aItem;
            IsDead = false;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            assetName = "images/block";
            delay = 200f;
            frames = 0;
            base.LoadContent(theContentManager, assetName);
        }

        public void Update(GameTime theGameTime)
        {
            UpdateSprite();

            elapsed += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay)
            {
                if (frames >= 3)
                    frames = 0;
                else
                    frames++;
                elapsed = 0;
            }
        }

        /// <summary>
        /// Update de sprite aan de hand van welke block het is
        /// </summary>
        private void UpdateSprite()
        {
            switch (Type)
            {
                case "Mystery":
                    Source = new Rectangle(16 * frames, 0, 16, Source.Height);
                    break;
                case "Used":
                    Source = new Rectangle(64, 0, 16, Source.Height);
                    break;
                case "Stone":
                    Source = new Rectangle(80, 0, 16, Source.Height);
                    break;
                case "Brick":
                    Source = new Rectangle(96, 0, 16, Source.Height);
                    break;
            }
        }
    }

}

