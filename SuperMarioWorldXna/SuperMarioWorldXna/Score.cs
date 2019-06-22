using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SuperMarioWorldXna
{
    public class Score : GameObject
    {
        private string assetName;
        private int digitNumber;

        public Score(Vector2 aStartPosition, int aDigitNumber)
        {
            mSpritePosition = aStartPosition;
            digitNumber = aDigitNumber;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            assetName = "images/numbers";
            base.LoadContent(theContentManager, assetName);
            UpdateCounter(Game1.Instance.CoinScore);
        }

        /// <summary>
        /// Update de Score
        /// </summary>
        /// <param name="theGameTime"></param>
        public void Update(GameTime theGameTime)
        {
            //Update de score counter die gelokaliseert is in Game1.cs
            UpdateCounter(Game1.Instance.CoinScore);
        }

        /// <summary>
        /// Houd de Score bij en laat zien welke cijfers het spel moet tekenen
        /// </summary>
        /// <param name="aScore"></param>
        public void UpdateCounter(int aScore)
        {
            int firstDigit;
            int secondDigit = 0;
            if (aScore >= 10)
            {
                firstDigit = aScore % 10;
                secondDigit = aScore / 10;
            }
            else
                firstDigit = aScore;

            if (digitNumber == 1)
            {
                int rectX = 16 * firstDigit;
                Source = new Rectangle(rectX, 0, 16, 14);
            }
            else if (digitNumber == 2)
            {
                int rectX = 16 * secondDigit;
                Source = new Rectangle(rectX, 0, 16, 14);
            }
        }
    }
}
