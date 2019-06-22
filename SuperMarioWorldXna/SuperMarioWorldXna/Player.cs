

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioWorldXna
{
    public class Player : GameObject
    {
        private string assetName;
        private int moveSpeed;
        private int moveLeft;
        private int moveRight;
        private bool facingRight;
        private bool playerBegin;
        private float elapsed;
        private float delay;
        private int frames;
        private int maxFrames;
        private int invincibleCounter;
        private Vector2 velocity;
        private Vector2 direction = Vector2.Zero;
        private Vector2 speed = Vector2.Zero;
        private State currentState;
        private Size currentSize;

        public bool OnGround { get; set; }

        public bool IsDead { get; set; }

        public float VelocityY
        {
            get { return velocity.Y; }
            set { velocity.Y = value; }
        }

        public string GetSize
        {
            get
            {
                if (currentSize == Size.Small)
                    return "Small";
                else
                    return "Big";
            }
        }

        public bool IsInvincible
        {
            get
            {
                if (invincibleCounter > 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// De Movement State van de speler
        /// </summary>
        enum State
        {
            Idle,
            Walking,
            Jumping,
            Falling,
            Death
        }

        /// <summary>
        /// De State of die groot/klein is
        /// </summary>
        enum Size
        {
            Big,
            Small
        }

        public Player(Vector2 aStartPosition)
        {
            moveSpeed = 160;
            moveLeft = -1;
            moveRight = 1;
            facingRight = true;
            playerBegin = true;
            IsDead = false;
            OnGround = true;
            velocity.X = 4f;
            delay = 50f;
            frames = 0;
            currentState = State.Idle;
            currentSize = Size.Small;
            invincibleCounter = 0;
            mSpritePosition = aStartPosition;
        }

        public void LoadContent(ContentManager theContentManager)
        {
            assetName = "images/mario";
            base.LoadContent(theContentManager, assetName);
        }

        public void Update(GameTime theGameTime)
        {
            UpdateSprite();
            KeyboardState aCurrentKeyboardState = Keyboard.GetState();
            KeyboardState mPreviousKeyboardState = aCurrentKeyboardState;

            //Laat de Camera de Speler volgen
            Camera.Instance.SetFocalPoint(new Vector2(mSpritePosition.X, ScreenManager.Instance.Dimensions.Y / 2));
            invincibleCounter -= 1;

            //Zolang de Speler niet dood is kun je niet bewegen
            if (playerBegin == false && currentState != State.Death)
            {
                UpdateMovement(aCurrentKeyboardState);
            }
            //Velocity word gebruikt tijdens Springen en Zwaartekracht
            mSpritePosition += velocity;

            //Zorgt voor de zwaartekracht bij de speler
            if (velocity.Y == 0)
                OnGround = true;
            else if (OnGround == false && velocity.Y < 7f)
                velocity.Y += 0.4f;

            //Begin van de level doet de Speler dit voor een korte periode
            if (playerBegin == true)
            {
                velocity.X -= 0.07f;
                currentSize = Size.Small;
                currentState = State.Walking;
                delay += 5f;
                moveSpeed = 180;
            }

            //Zet playerBegin naar false als die op een bepaald punt is
            if (velocity.X <= 0f && playerBegin == true)
            {
                playerBegin = false;
                velocity.X = 0f;
            }

            elapsed += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;

            if (elapsed >= delay)
            {
                if (frames >= maxFrames - 1)
                    frames = 0;
                else
                    frames++;
                elapsed = 0;
            }

            base.Update(theGameTime, speed, direction);
        }

        /// <summary>
        /// Update de movement van speler wanneer er bepaalde condities geevenaard worden
        /// </summary>
        /// <param name="aCurrentKeyboardState"></param>
        private void UpdateMovement(KeyboardState aCurrentKeyboardState)
        {
            speed = Vector2.Zero;
            direction = Vector2.Zero;

            // Left Movement
            if (aCurrentKeyboardState.IsKeyDown(Keys.Left) == true && mSpritePosition.X > 0)
            {
                speed.X = moveSpeed;
                direction.X = moveLeft;
                facingRight = false;
                if (OnGround == true && currentState != State.Walking)
                    currentState = State.Walking;
            }

            // Right Movement
            else if (aCurrentKeyboardState.IsKeyDown(Keys.Right) == true)
            {
                speed.X = moveSpeed;
                direction.X = moveRight;
                facingRight = true;
                if (OnGround == true && currentState != State.Walking)
                    currentState = State.Walking;
            }

            // Idle
            else if (OnGround == true)
                currentState = State.Idle;

            // Jumping
            if (aCurrentKeyboardState.IsKeyDown(Keys.X) && OnGround == true)
            {
                velocity.Y = -7f;
                OnGround = false;
                currentState = State.Jumping;
            }

            //Falling
            if (velocity.Y >= 0f && OnGround == false)
                currentState = State.Falling;

            // Running
            if (aCurrentKeyboardState.IsKeyDown(Keys.Z))
            {
                delay = 50f;
                moveSpeed = 180;
            }
            else if (OnGround == true)
            {
                delay = 100f;
                moveSpeed = 120;
            }
        }

        /// <summary>
        /// Update de Sprite aan de hand van de States van Speler
        /// </summary>
        private void UpdateSprite()
        {
            int rectY;
            int rectHeight;
            if (currentSize == Size.Small)
            {
                rectHeight = 22;
                rectY = 42;
                maxFrames = 2;
            }
            else
            {
                rectHeight = 31;
                rectY = 1;
                maxFrames = 3;
            }

            Rectangle rWalk = new Rectangle(20 * frames, rectY, 16, rectHeight);
            Rectangle rJump = new Rectangle(60, rectY, 16, rectHeight);
            Rectangle rIdle = new Rectangle(0, rectY, 16, rectHeight);
            Rectangle rFall = new Rectangle(80, rectY, 16, rectHeight);
            Rectangle rDeath = new Rectangle(100, 40, 16, 24);
            Rectangle rInvincible = new Rectangle(40 * frames, rectY, 16, rectHeight);

            if (facingRight == false)
                Effect = SpriteEffects.FlipHorizontally;
            else
                Effect = SpriteEffects.None;
            if (IsInvincible)
                Source = rInvincible;
            else if (currentState == State.Walking)
                Source = rWalk;
            else if (currentState == State.Jumping)
                Source = rJump;
            else if (currentState == State.Idle)
                Source = rIdle;
            else if (currentState == State.Falling)
                Source = rFall;
            else if (currentState == State.Death)
                Source = rDeath;
        }

        /// <summary>
        /// Past de size aan bij bepaalde condities
        /// </summary>
        /// <param name="newSize"></param>
        public void UpdateSize(string newSize)
        {
            if (newSize == "Big")
            {
                currentSize = Size.Big;
                mSpritePosition.Y -= Source.Height / 2;
            }
            else
                currentSize = Size.Small;
        }

        /// <summary>
        /// Veranderd bapaalde waardes van Speler wanneer die dood is om zo duidelijk te maken dat die dood is
        /// </summary>
        public void Death()
        {
            IsDead = true;
            currentState = State.Death;
            speed.X = 0;
        }

        /// <summary>
        ///  Wanneer speler geraakt word terwijl die state Big heeft word die weer Small en voor een korte periode onsterfelijk
        /// </summary>
        public void MakeInvincible()
        {
            invincibleCounter = 50;
        }
    }
}

