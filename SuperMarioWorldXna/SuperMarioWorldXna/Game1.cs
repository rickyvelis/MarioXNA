using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioWorldXna
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        public static Game1 Instance;
        private GraphicsDeviceManager graphics;
        private SpriteBatch theSpriteBatch;

        public int Life { get; set; }

        public int CoinScore { get; set; }

        public Game1()
        {
            Instance = this;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Life = 3;
        }

        protected override void Initialize()
        {
            ScreenManager.Instance.Initialize();
            ScreenManager.Instance.Dimensions = new Vector2(500, 400);
            graphics.PreferredBackBufferWidth = (int)ScreenManager.Instance.Dimensions.X;
            graphics.PreferredBackBufferHeight = (int)ScreenManager.Instance.Dimensions.Y;
            graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            theSpriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.LoadContent(Content);
        }

        protected override void Update(GameTime theGameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            ScreenManager.Instance.Update(theGameTime);

            if (CoinScore == 100)
            {
                CoinScore = 0;
                Life += 1; 
            }
            base.Update(theGameTime);
        }

        protected override void Draw(GameTime theGameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            theSpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Camera.Instance.ViewMatrix);
            ScreenManager.Instance.Draw(theSpriteBatch);
            theSpriteBatch.End();
            base.Draw(theGameTime);
        }
    }
}
