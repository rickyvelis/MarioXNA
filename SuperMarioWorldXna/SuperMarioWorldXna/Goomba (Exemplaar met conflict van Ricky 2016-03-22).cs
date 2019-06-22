
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
    public class Goomba : GameObject
    {
        private string assetName;
        private int moveSpeed;
        private int moveLeft = -1;
        private int moveRight = 1;
        private bool facingRight;
        private float elapsed;
        private float delay;
        private int frames;

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;


        public Goomba(Vector2 aStartPosition)
        {
            assetName = "images/goomba";
            SpritePosition = aStartPosition;
            moveSpeed = 50;
            facingRight = false;
            delay = 200f;
            frames = 0;
        }


        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, assetName);
            Source = new Rectangle(0, 0, 16, Source.Height);
        }

        public void Update(GameTime theGameTime)
        {
            mDirection.X = moveLeft;
            mSpeed.X = moveSpeed;
            base.Update(theGameTime, mSpeed, mDirection);

            UpdateAnimation();

            elapsed += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;
            if (elapsed >= delay)
            {
                if (frames >= 1)
                {
                    frames = 0;
                }
                else
                {
                    frames++;
                }
                elapsed = 0;
            }
        }

        public void UpdateAnimation()
        {
            if (facingRight == true)
            {
                Source = new Rectangle(40 + 20 * frames, 0, 16, Source.Height);
            }
            else
            {
                Source = new Rectangle(20 * frames, 0, 16, Source.Height);
            }
        }

        public void GoombaCollision()
        {
            if (facingRight == true)
            {
                facingRight = false;
                UpdateAnimation();
                mDirection.X = moveLeft;
            }
            else if (facingRight == false)
            {
                facingRight = true;
                UpdateAnimation();
                mDirection.X = moveRight;
            }
        }

        public void GoombaDeath()
        {
            SpritePosition.X = 0;
        }
    }
}
