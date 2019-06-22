using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioWorldXna
{
    public class GameObject
    {
        public Vector2 mSpritePosition = new Vector2(0, 0);
        public string mAssetName;
        public Rectangle mSpriteSize;
        private Texture2D spriteTexture;
        private Rectangle source;
        private SpriteEffects effect;
        private float scale = 1.0f;

        public Rectangle Source
        {
            get { return source; }
            set
            {
                source = value;
                mSpriteSize = new Rectangle(0, 0, (int)(source.Width * Scale), (int)(source.Height * Scale));
            }
        }

        public SpriteEffects Effect
        {
            get { return effect; }
            set { effect = value; }
        }

        public float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                mSpriteSize = new Rectangle(0, 0, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }

        /// <summary>
        /// Voegt een BoundingBox bij elk GameObject toe
        /// </summary>
        public Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)mSpritePosition.X, (int)mSpritePosition.Y, (int)(Source.Width * Scale), (int)(Source.Height * Scale));
            }
        }

        public void LoadContent(ContentManager theContentManager, string theAssetName)
        {
            spriteTexture = theContentManager.Load<Texture2D>(theAssetName);
            mAssetName = theAssetName;
            Source = new Rectangle(0, 0, spriteTexture.Width, spriteTexture.Height);
            mSpriteSize = new Rectangle(0, 0, (int)(spriteTexture.Width * Scale), (int)(spriteTexture.Height * Scale));
        }

        public void Update(GameTime theGameTime, Vector2 theSpeed, Vector2 theDirection)
        {
            mSpritePosition += theDirection * theSpeed * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(spriteTexture, mSpritePosition, Source, Color.White, 0.0f, Vector2.Zero, Scale, effect, 0);
        }

    }
}
