
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioWorldXna
{
    public class Coin : GameObject
    {
        private string assetName;
        private float delay = 200f;
        private float elapsed;
        private int frames = 0;

        public bool IsDead { get; private set; }

        public Coin(Vector2 aStartPosition)
        {
            assetName = "images/coin";
            mSpritePosition = aStartPosition;
            IsDead = false;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, assetName);
            Source = new Rectangle(0, 0, 16, Source.Height);
        }

        public void Update(GameTime theGameTime)
        {
            UpdateAnimation();

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
        /// Update de animatie van Coin zodat die rond draait
        /// </summary>
        private void UpdateAnimation()
        {
            Source = new Rectangle(16 * frames, 0, 16, Source.Height);
        }

        /// <summary>
        /// Wanneer die opgepakt word door een Speler is de Coin dood en word die niet meer weergegeven
        /// </summary>
        public void Death()
        {
            IsDead = true;
        }
    }

}
