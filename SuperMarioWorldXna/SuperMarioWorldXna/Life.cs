using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorldXna
{
    public class Life : GameObject
    {
        private string assetName;
        private float time;
        private bool minLife;

        public Life(Vector2 aStartPosition)
        {
            mSpritePosition = aStartPosition;
            minLife = false;
            LifeCounter(Game1.Instance.Life);
        }

        public void LoadContent(ContentManager theContentManager)
        {
            assetName = "images/numbers";
            base.LoadContent(theContentManager, assetName);
        }

        /// <summary>
        /// Update de levens van Speler die opgeslagen staan in Game1.cs
        /// </summary>
        /// <param name="theGameTime"></param>
        public void Update(GameTime theGameTime)
        {
            LifeCounter(Game1.Instance.Life);

            //De tijd zorgt ervoor dat er maar 1 leven eraf word gehaald
            if (time <= 10 && minLife == true)
                Game1.Instance.Life--;

            if (minLife == true)
                time += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;
        }

        /// <summary>
        /// Het leven nummer display in de HUD
        /// </summary>
        /// <param name="aLife"></param>
        public void LifeCounter(int aLife)
        {
            int rectX = 16 * aLife;
            Source = new Rectangle(rectX, 0, 16, 14);
        }

        /// <summary>
        /// Zet minLife naar true wanneer Speler dood gaat zodat de tijd kan lopen en er een leven afgaat
        /// </summary>
        public void UpdateLife()
        {
            minLife = true;
        }

    }
}
