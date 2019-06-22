using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SuperMarioWorldXna
{
    public class ScreenManager
    {
        private ContentManager content;
        private GameScreen currentScreen;
        private GameScreen newScreen;
        private static ScreenManager instance;
        private Stack<GameScreen> screenStack = new Stack<GameScreen>();

        public static ScreenManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ScreenManager();
                return instance;
            }
        }

        public Vector2 Dimensions { get; set; }

        public void AddScreen(GameScreen screen)
        {
            newScreen = screen;
            screenStack.Push(screen);
            currentScreen.UnloadContent();
            currentScreen = newScreen;
            currentScreen.LoadContent(content);
        }

        public void Initialize()
        {
            currentScreen = new TitleScreen();
        }

        public void LoadContent(ContentManager theContentManager)
        {
            content = new ContentManager(theContentManager.ServiceProvider, "Content");
            currentScreen.LoadContent(theContentManager);
        }

        public void Update(GameTime theGameTime)
        {
            currentScreen.Update(theGameTime);
            Camera.Instance.Update();
        }

        public void Draw(SpriteBatch theSpriteBatch)
        {
            currentScreen.Draw(theSpriteBatch);
        }
    }
}
