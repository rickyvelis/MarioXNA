
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
    public class Koopa : GameObject
    {
        private string assetName;
        private int moveSpeed;
        private int moveLeft;
        private int moveRight;
        private float elapsed;
        private float delay;
        private int frames;
        private Vector2 velocity;
        private Vector2 direction = Vector2.Zero;
        private Vector2 speed = Vector2.Zero;

        public bool IsDead { get; private set; }

        public bool FacingRight { get; set; }

        public float VelocityY
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }


        public Koopa(Vector2 aStartPosition)
        {
            assetName = "images/greenKoopa";
            mSpritePosition = aStartPosition;
            mSpritePosition.Y -= 11;
            moveSpeed = 60;
            delay = 200f;
            velocity.X = 0f;
            frames = 0;
            moveLeft = -1;
            moveRight = 1;
            IsDead = false;
            FacingRight = false;
            direction.X = moveLeft;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager, assetName);
            Source = new Rectangle(20 * frames, 0, 16, Source.Height);
        }

        public void Update(GameTime theGameTime)
        {
            speed.X = moveSpeed;
            mSpritePosition += velocity;
            base.Update(theGameTime, speed, direction);

            UpdateAnimation();

            //Word gebruikt voor de zwaartekracht zodat de wilt blijven vallen
            velocity.Y = 4f;

            //Veranderd de richting van Koopa wanneer die tegen de meest linkse muur loopt
            if (mSpritePosition.X <= 0)
                ChangeDirection("Right");

            elapsed += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay)
            {
                if (frames >= 1)
                    frames = 0;
                else
                    frames++;
                elapsed = 0;
            }
        }

        /// <summary>
        /// Past de animatie aan naar de kant waar die naar toe loopt
        /// </summary>
        public void UpdateAnimation()
        {
            Source = new Rectangle(20 + 20 * frames, 0, 16, Source.Height);

            if (FacingRight == true)
                Effect = SpriteEffects.None;
            else
                Effect = SpriteEffects.FlipHorizontally;
        }

        /// <summary>
        /// Veranderd de richting waarop Koopa loopt
        /// </summary>
        /// <param name="aDirection"></param>
        public void ChangeDirection(string aDirection)
        {
            if (aDirection == "Right")
            {
                FacingRight = true;
                direction.X = moveRight;
            }
            else if (aDirection == "Left")
            {
                FacingRight = false;
                direction.X = moveLeft;
            }
            UpdateAnimation();
        }

        /// <summary>
        /// Wanneer er een Koopa sterft wordt dit uitgevoerd
        /// </summary>
        public void Death()
        {
            IsDead = true;
            velocity.Y = 3f;
            Effect = SpriteEffects.FlipVertically;
            moveLeft = 0;
            moveRight = 0;
        }

        /// <summary>
        /// Wanneer Speler dood gaat word deze methode aangeroepen om Koopa te laten stoppen met lopen
        /// </summary>
        public void Freeze()
        {
            moveSpeed = 0;
        }
    }
}
