using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml;

namespace SuperMarioWorldXna
{
    public class TitleScreen : GameScreen
    {
        private List<Goomba> goombas = new List<Goomba>();
        private List<Tile> tiles = new List<Tile>();
        private List<Pipe> pipes = new List<Pipe>();
        private List<Ground> ground = new List<Ground>();
        private List<Coin> coins = new List<Coin>();
        private KeyboardState keyState;
        private Texture2D logoTexture;
        private Texture2D text;
        private Texture2D backgroundTexture;
        private float tileSize;


        /// <summary>
        /// Leest het .xml bestand uit om zo z'n eigen level uit te voeren.
        /// </summary>
        public TitleScreen()
        {
            LoadLevel("./Levels/titlescreen.xml");
        }

        /// <summary>
        /// Laad de content voor elk object.
        /// </summary>
        /// <param name="theContentManager"></param>
        public override void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager);

            //Laad alle logos in TitleScreen, tekst en achtergronden
            logoTexture = content.Load<Texture2D>("images/titleScreenLogo");
            text = content.Load<Texture2D>("images/titleScreenText");
            backgroundTexture = content.Load<Texture2D>("images/background1");

            //Laad voor elk Object het eigen content
            foreach (Goomba aGoomba in goombas)
                aGoomba.LoadContent(theContentManager);
            foreach (Pipe aPipe in pipes)
                aPipe.LoadContent(theContentManager);
            foreach (Ground aGround in ground)
                aGround.LoadContent(theContentManager);
            foreach (Coin aCoin in coins)
                aCoin.LoadContent(theContentManager);
        }

        /// <summary>
        /// Leest een .xml file met verschillende char's op verschillende posities om een level weer te geven met Objecten op de gewenste coordinaten
        /// </summary>
        /// <param name="theLevelFile"></param>
        public void LoadLevel(string theLevelFile)
        {
            //Leest per regel uit de .xml wat er moet worden weergegeven op welke positie; elke volgende regel is Y += 16 en elke volgende letter is X += 16
            XmlReader reader = XmlReader.Create(theLevelFile);
            int aPositionY = 0;
            int aPositionX = 0;
            tileSize = 16;

            string aCurrentElement = string.Empty;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement && reader.Name.Equals("level", StringComparison.OrdinalIgnoreCase))
                    break;
                //Bij een </row> in de .xml file gaat er +16 bij de Y positie
                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (aCurrentElement == "row")
                    {
                        aPositionY += (int)tileSize;
                        aPositionX = 0;
                    }
                }
                //Een regel wordt uitgelezen na een <row> en elke letter die wordt gelezen wordt opgeslagen in een List van Tiles die de char meekrijgt en de X- en Y positie.
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    if (aCurrentElement == "row")
                    {
                        string aRow = reader.Value;
                        for (int aCounter = 0; aCounter < aRow.Length; ++aCounter)
                        {
                            tiles.Add(new Tile(aRow[aCounter], new Vector2(aPositionX, aPositionY)));
                            aPositionX += (int)tileSize;
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element)
                    aCurrentElement = reader.Name;
            }
            //Er wordt gecheckt welke char er in elke Tile zit en er worden Objecten toegevoegd aan de bijbehorende Lists aan de hand van de char
            foreach (Tile aTile in tiles)
                switch (aTile.mSymbol)
                {
                    case 'G':
                        goombas.Add(new Goomba(new Vector2(aTile.mPosition.X, aTile.mPosition.Y)));
                        break;
                    case 'P':
                        pipes.Add(new Pipe(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Top"));
                        break;
                    case 'B':
                        pipes.Add(new Pipe(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Bottom"));
                        break;
                    case 'C':
                        coins.Add(new Coin(new Vector2(aTile.mPosition.X, aTile.mPosition.Y)));
                        break;
                    case '7':
                        ground.Add(new Ground(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "NorthWest"));
                        break;
                    case '8':
                        ground.Add(new Ground(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "North"));
                        break;
                    case '9':
                        ground.Add(new Ground(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "NorthEast"));
                        break;
                    case '4':
                        ground.Add(new Ground(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "West"));
                        break;
                    case '5':
                        ground.Add(new Ground(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Middle"));
                        break;
                    case '6':
                        ground.Add(new Ground(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "East"));
                        break;
                    case '1':
                        ground.Add(new Ground(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "SouthWest"));
                        break;
                    case '2':
                        ground.Add(new Ground(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "South"));
                        break;
                    case '3':
                        ground.Add(new Ground(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "SouthEast"));
                        break;
                }
        }

        /// <summary>
        /// De gameloop.
        /// </summary>
        /// <param name="theGameTime"></param>
        public override void Update(GameTime theGameTime)
        {
            //Reset de Camera naar 0
            Camera.Instance.mCameraPosition.X = 0;
            Camera.Instance.mCameraPosition.Y = 0;

            keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Space))
                ScreenManager.Instance.AddScreen(new LevelScreen(1));
            foreach (Goomba aGoomba in goombas)
                aGoomba.Update(theGameTime);
            foreach (Coin aCoin in coins)
                aCoin.Update(theGameTime);

            CheckCollision();
        }

        /// <summary>
        /// Handlet het gedrag van objecten indien deze met elkaar colliden
        /// </summary>
        public void CheckCollision()
        {
            Rectangle overlap;

            //Pipe collision met Goomba
            foreach (Pipe aPipe in pipes)
            {
                foreach (Goomba aGoomba in goombas)
                {
                    overlap = Rectangle.Intersect(aGoomba.BoundingBox, aPipe.BoundingBox);
                    if (aGoomba.BoundingBox.Intersects(aPipe.BoundingBox))
                    {
                        overlap = Rectangle.Intersect(aGoomba.BoundingBox, aPipe.BoundingBox);
                        if (overlap.Width > overlap.Height)
                        {
                            if (aGoomba.mSpritePosition.Y < aPipe.mSpritePosition.Y)
                            {
                                aGoomba.mSpritePosition.Y = aPipe.BoundingBox.Top - aGoomba.Source.Height;
                                aGoomba.VelocityY = 0;
                            }
                        }
                        else
                        {
                            if (aGoomba.mSpritePosition.X > aPipe.mSpritePosition.X && aGoomba.mSpritePosition.Y + aGoomba.Source.Height > aPipe.mSpritePosition.Y + 10)
                                aGoomba.ChangeDirection("Right");
                            else if (aGoomba.mSpritePosition.X < aPipe.mSpritePosition.X && aGoomba.mSpritePosition.Y + aGoomba.Source.Height > aPipe.mSpritePosition.Y + 10)
                                aGoomba.ChangeDirection("Left");
                        }
                    }
                }
            }
            //Ground collision met Goomba
            foreach (Ground aGround in ground)
            {
                if (aGround.Type == "North" || aGround.Type == "NorthWest" || aGround.Type == "NorthEast")
                {
                    foreach (Goomba aGoomba in goombas)
                    {
                        if (aGoomba.BoundingBox.Intersects(aGround.BoundingBox))
                        {
                            overlap = Rectangle.Intersect(aGoomba.BoundingBox, aGround.BoundingBox);
                            if (overlap.Width > overlap.Height)
                            {
                                if (aGoomba.mSpritePosition.Y < aGround.mSpritePosition.Y)
                                {
                                    aGoomba.mSpritePosition.Y = aGround.BoundingBox.Top - aGoomba.Source.Height;
                                    aGoomba.VelocityY = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Tekent alle objecten op het scherm
        /// </summary>
        /// <param name="theSpriteBatch"></param>
        public override void Draw(SpriteBatch theSpriteBatch)
        {
            theSpriteBatch.Draw(backgroundTexture, new Vector2(0, 0), Color.White);
            theSpriteBatch.Draw(logoTexture, new Vector2((ScreenManager.Instance.Dimensions.X - logoTexture.Width) / 2, 100), Color.White);
            theSpriteBatch.Draw(text, new Vector2((ScreenManager.Instance.Dimensions.X - text.Width) / 2, 300), Color.White);

            foreach (Pipe aPipe in pipes)
                if (aPipe.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aPipe.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X)
                    aPipe.Draw(theSpriteBatch);

            foreach (Ground aGround in ground)
                if (aGround.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aGround.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X)
                    aGround.Draw(theSpriteBatch);

            foreach (Goomba aGoomba in goombas)
                if (aGoomba.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aGoomba.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                || aGoomba.mSpritePosition.Y > ScreenManager.Instance.Dimensions.Y)
                    aGoomba.Draw(theSpriteBatch);


            foreach (Coin aCoin in coins)
                if (aCoin.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aCoin.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                && !aCoin.IsDead)
                    aCoin.Draw(theSpriteBatch);
        }
    }
}
