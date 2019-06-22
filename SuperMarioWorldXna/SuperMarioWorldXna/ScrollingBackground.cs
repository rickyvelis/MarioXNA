using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace SuperMarioWorldXna
{
    class ScrollingBackground
    {
        private List<GameObject> backgroundSprites;

        private GameObject rightMostSprite;
        private GameObject leftMostSprite;
        private Viewport viewport;

        /// <summary>
        /// De kant waarop de X positie op gaat        
        /// </summary>

        public enum HorizontalScrollDirection
        {
            Left,
            Right
        }

        public ScrollingBackground(Viewport theViewport)
        {
            backgroundSprites = new List<GameObject>();
            rightMostSprite = null;
            leftMostSprite = null;
            viewport = theViewport;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            //Verwijderd de sprites links en rechts
            rightMostSprite = null;
            leftMostSprite = null;
            float aWidth = 0;

            //Laad alle sprites
            foreach (GameObject aBackgroundSprite in backgroundSprites)
            {
                aBackgroundSprite.LoadContent(theContentManager, aBackgroundSprite.mAssetName);

                if (rightMostSprite == null)
                {
                    aBackgroundSprite.mSpritePosition = new Vector2(viewport.X, viewport.Y);
                    leftMostSprite = aBackgroundSprite;
                }
                else
                {
                    aBackgroundSprite.mSpritePosition = new Vector2(rightMostSprite.mSpritePosition.X + rightMostSprite.mSpriteSize.Width, viewport.Y);
                }

                rightMostSprite = aBackgroundSprite;
                aWidth += aBackgroundSprite.mSpriteSize.Width;
            }
            int aIndex = 0;
            if (backgroundSprites.Count > 0 && aWidth < viewport.Width * 2)
            {
                do
                {
                    //Voegt nog een sprite toe
                    GameObject aBackgroundSprite = new GameObject();
                    aBackgroundSprite.mAssetName = backgroundSprites[aIndex].mAssetName;
                    aBackgroundSprite.LoadContent(theContentManager, aBackgroundSprite.mAssetName);
                    aBackgroundSprite.mSpritePosition = new Vector2(rightMostSprite.mSpritePosition.X + rightMostSprite.mSpriteSize.Width, viewport.Y);
                    backgroundSprites.Add(aBackgroundSprite);
                    rightMostSprite = aBackgroundSprite;

                    aWidth += aBackgroundSprite.mSpriteSize.Width;

                    //Gaat naar de volgende Sprite die in de Loadcontent van Levelscreen.cs staat
                    aIndex += 1;
                    if (aIndex > backgroundSprites.Count - 1)
                    {
                        aIndex = 0;
                    }

                } while (aWidth < viewport.Width * 2);

            }
        }

        /// <summary>
        /// Voegt de nieuwe achtergrond toe
        /// </summary>
        /// <param name="theAssetName"></param>
        public void AddBackground(string theAssetName)
        {
            GameObject aBackgroundSprite = new GameObject();
            aBackgroundSprite.mAssetName = theAssetName;

            backgroundSprites.Add(aBackgroundSprite);
        }

        public void Update(GameTime theGameTime, int theSpeed, HorizontalScrollDirection theDirection)
        {
            if (theDirection == HorizontalScrollDirection.Left)
            {
                //Kijkt of de sprite van het scherm af is van Links
                foreach (GameObject aBackgroundSprite in backgroundSprites)
                {
                    if (aBackgroundSprite.mSpritePosition.X < viewport.X - aBackgroundSprite.mSpriteSize.Width)
                    {
                        aBackgroundSprite.mSpritePosition = new Vector2(rightMostSprite.mSpritePosition.X + rightMostSprite.mSpriteSize.Width, viewport.Y);
                        rightMostSprite = aBackgroundSprite;
                    }
                }
            }
            else if (theDirection == HorizontalScrollDirection.Right)
            {
                //Kijkt of de sprite van het scherm af is van Rechts
                foreach (GameObject aBackgroundSprite in backgroundSprites)
                {
                    if (aBackgroundSprite.mSpritePosition.X > viewport.X + viewport.Width)
                    {
                        aBackgroundSprite.mSpritePosition = new Vector2(leftMostSprite.mSpritePosition.X - leftMostSprite.mSpriteSize.Width, viewport.Y);
                        leftMostSprite = aBackgroundSprite;
                    }
                }
            }
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (GameObject aBackgroundSprite in backgroundSprites)
            {
                aBackgroundSprite.Draw(theSpriteBatch);
            }
        }

    }
}

