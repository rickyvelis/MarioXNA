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
    public class Mushroom : GameObject
    {
        private string assetName;
        private int moveSpeed;
        private int moveLeft;
        private int moveRight;
        private Vector2 mVelocity;

        public bool IsDead { get; private set; }
        
        public bool FacingRight { get; private set; }

        public bool Hidden { get; set; }

        public float VelocityY
        {
            get { return mVelocity.Y; }
            set { mVelocity.Y = value; }
        }

        Vector2 mDirection = Vector2.Zero;
        Vector2 mSpeed = Vector2.Zero;

        public Mushroom(Vector2 aStartPosition)
        {
            mSpritePosition = aStartPosition;
            Hidden = false;
        }

        public Mushroom(Vector2 aStartPosition, bool hidden)
        {
            mSpritePosition = aStartPosition;
            Hidden = hidden;
        }


        public void LoadContent(ContentManager theContentManager)
        {
            assetName = "images/mushroom";
            moveSpeed = 60;
            mVelocity.X = 0f;
            moveLeft = -1;
            moveRight = 1;
            FacingRight = false;
            mDirection.X = moveLeft;
            IsDead = false;
            base.LoadContent(theContentManager, assetName);
            Source = new Rectangle(0, 0, Source.Width, Source.Height);
        }

        public void Update(GameTime theGameTime)
        {
            if (Hidden)
            {
                mVelocity.Y = 0f;
                moveSpeed = 0;
            }
            else
            {
                mVelocity.Y = 1.5f;
                moveSpeed = 60;
            }
                
            mSpeed.X = moveSpeed;
            mSpritePosition += mVelocity;
            base.Update(theGameTime, mSpeed, mDirection);
            
        }
        /// <summary>
        /// Veranderd de kant waarop de Mushroom op gaat
        /// </summary>
        /// <param name="aDirection"></param>
        public void ChangeDirection(string aDirection)
        {
            if (aDirection == "Right")
            {
                FacingRight = true;
                mDirection.X = moveRight;
            }
            else if (aDirection == "Left")
            {
                FacingRight = false;
                mDirection.X = moveLeft;
            }
        }

        /// <summary>
        /// Zorgt ervoor dat isDead op True gezet word die ervoor zorgt dat Mushroom weggaat
        /// </summary>
        public void Death()
        {
            IsDead = true;
        }
    }
}
