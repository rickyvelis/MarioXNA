using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SuperMarioWorldXna
{
    public class LevelScreen : GameScreen
    {
        public Player mPlayer;
        public List<Goomba> mGoombas = new List<Goomba>();
        public List<Tile> mTiles = new List<Tile>();
        public List<MysteryBlock> mMysteryBlocks = new List<MysteryBlock>();
        public List<Pipe> mPipes = new List<Pipe>();
        private ScrollingBackground mBackground;

        float velocity = 0.1f;


        public LevelScreen()
        {
            LoadLevel("./Levels/level.xml");
        }

        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            mBackground = new ScrollingBackground(Game1.Instance.GraphicsDevice.Viewport);
            mBackground.AddBackground("images/Background1");
            mBackground.AddBackground("images/Background2");
            mBackground.LoadContent(Content);
            foreach (Goomba aGoomba in mGoombas)
                aGoomba.LoadContent(Content);
            foreach (MysteryBlock aMysteryBlock in mMysteryBlocks)
                aMysteryBlock.LoadContent(Content);
            foreach (Pipe aPipe in mPipes)
                aPipe.LoadContent(Content);
            mPlayer.LoadContent(Content);
        }


        public void LoadLevel(string theLevelFile)
        {
            XmlReader reader = XmlReader.Create(theLevelFile);
            int aPositionY = 0;
            int aPositionX = 0;

            string aCurrentElement = string.Empty;
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.EndElement &&
                    reader.Name.Equals("level", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (aCurrentElement == "row")
                    {
                        aPositionY += 16;
                        aPositionX = 0;
                    }
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    if (aCurrentElement == "row")
                    {
                        string aRow = reader.Value;

                        for (int aCounter = 0; aCounter < aRow.Length; ++aCounter)
                        {
                            mTiles.Add(new Tile(aRow[aCounter], new Vector2(aPositionX, aPositionY)));
                            aPositionX += 16; //magicnumber!!!!!
                        }
                    }
                }
                else if (reader.NodeType == XmlNodeType.Element)
                {
                    aCurrentElement = reader.Name;
                }
            }
            foreach (Tile aTile in mTiles)
            {
                switch (aTile.Symbol)
                {
                    case 'G':
                        mGoombas.Add(new Goomba(new Vector2(aTile.Position.X, aTile.Position.Y)));
                        break;
                    case 'P':
                        mPlayer = new Player(new Vector2(aTile.Position.X, aTile.Position.Y));
                        break;
                    case 'M':
                        mMysteryBlocks.Add(new MysteryBlock(new Vector2(aTile.Position.X, aTile.Position.Y)));
                        break;
                    case 'Q':
                        mPipes.Add(new Pipe(new Vector2(aTile.Position.X, aTile.Position.Y)));
                        break;
                }
            }
            foreach (MysteryBlock aMysteryBlock in mMysteryBlocks)
            {
                Rectangle overlap = Rectangle.Intersect(mPlayer.BoundingBox, aMysteryBlock.BoundingBox);

                if (mPlayer.BoundingBox.Intersects(aMysteryBlock.BoundingBox))
                {
                    if (overlap.Width > overlap.Height)
                    {
                        if (mPlayer.SpritePosition.Y > aMysteryBlock.SpritePosition.Y)
                        {
                            mPlayer.SpritePosition.Y += 1;
                        }
                        if (mPlayer.SpritePosition.Y < aMysteryBlock.SpritePosition.Y)
                        {
                            mPlayer.SpritePosition.Y -= 1;
                            mPlayer.VelocityY = 0;
                        }
                    }
                    else
                    {
                        if (mPlayer.SpritePosition.X > aMysteryBlock.SpritePosition.X)
                            mPlayer.SpritePosition.X += 1;
                        if (mPlayer.SpritePosition.X < aMysteryBlock.SpritePosition.X)
                            mPlayer.SpritePosition.X -= 1;
                    }
                }
            }
        }

        #region Collision

        public void CheckCollision()
        {
            foreach (Goomba aGoomba in mGoombas)
            {
                if (mPlayer.BoundingBox.Intersects(aGoomba.BoundingBox) && mPlayer.SpritePosition.Y < aGoomba.SpritePosition.Y)
                    aGoomba.GoombaDeath();
                else if (mPlayer.BoundingBox.Intersects(aGoomba.BoundingBox))
                    mPlayer.PlayerDeath();
            }
            foreach (Pipe aPipe in mPipes)
            {
                Rectangle overlap = Rectangle.Intersect(mPlayer.BoundingBox, aPipe.BoundingBox);
                float lastPosition = 0f;
                float currentPosition;

                if (mPlayer.BoundingBox.Intersects(aPipe.BoundingBox))
                {

                    if (overlap.Width > overlap.Height)
                    {
                        if (mPlayer.SpritePosition.Y > aPipe.SpritePosition.Y)
                            mPlayer.SpritePosition.Y = aPipe.BoundingBox.Bottom;
                        if (mPlayer.SpritePosition.Y < aPipe.SpritePosition.Y)
                        {
                            mPlayer.SpritePosition.Y = aPipe.BoundingBox.Top - mPlayer.Source.Height;
                            mPlayer.VelocityY = 0;
                            lastPosition = mPlayer.SpritePosition.Y;
                            velocity = 0f;
                        }
                    }
                    else
                    {
                        if (mPlayer.SpritePosition.X > aPipe.SpritePosition.X)
                            mPlayer.SpritePosition.X = aPipe.BoundingBox.Right;
                        else
                            mPlayer.SpritePosition.X = aPipe.BoundingBox.Left - mPlayer.Source.Width;
                    }
                }
                else if (mPlayer.OnGround == true)
                {
                    mPlayer.VelocityY = velocity;
                    currentPosition = mPlayer.SpritePosition.Y;
                    if (lastPosition != currentPosition)
                        velocity += 0.01f;
                }
            }
            foreach (MysteryBlock aMysteryBlock in mMysteryBlocks)
            {
                Rectangle overlap = Rectangle.Intersect(mPlayer.BoundingBox, aMysteryBlock.BoundingBox);
                float lastPosition = 0f;
                float currentPosition;

                if (mPlayer.BoundingBox.Intersects(aMysteryBlock.BoundingBox))
                {
                    if (overlap.Width > overlap.Height)
                    {
                        if (mPlayer.SpritePosition.Y > aMysteryBlock.SpritePosition.Y)
                            mPlayer.SpritePosition.Y = aMysteryBlock.BoundingBox.Bottom;
                        else
                        {
                            mPlayer.SpritePosition.Y = aMysteryBlock.BoundingBox.Top - mPlayer.Source.Height;
                            mPlayer.VelocityY = 0;
                            lastPosition = mPlayer.SpritePosition.Y;
                            velocity = 0f;
                        }
                    }
                    else
                    {
                        if (mPlayer.SpritePosition.X > aMysteryBlock.SpritePosition.X)
                            mPlayer.SpritePosition.X = aMysteryBlock.BoundingBox.Right;
                        else
                            mPlayer.SpritePosition.X = aMysteryBlock.BoundingBox.Left - mPlayer.Source.Width;

                    }
                }
                else if (mPlayer.OnGround == true)
                {
                    mPlayer.VelocityY = velocity;
                    currentPosition = mPlayer.SpritePosition.Y;
                    if (lastPosition != currentPosition)
                        velocity += 0.01f;

                }
            }
        }

        #endregion

        public override void UnloadContent()
        {
            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Keys.Escape))
                ScreenManager.Instance.AddScreen(new TitleScreen());
            if (keyState.IsKeyDown(Keys.X))
                mBackground.Update(gameTime, 160, ScrollingBackground.HorizontalScrollDirection.Left);
            foreach (Goomba aGoomba in mGoombas)
                aGoomba.Update(gameTime);
            foreach (MysteryBlock aMysteryBlock in mMysteryBlocks)
                aMysteryBlock.Update(gameTime);
            foreach (Pipe aPipe in mPipes)
                aPipe.Update(gameTime);
            mPlayer.Update(gameTime);
            CheckCollision();

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            mBackground.Draw(spriteBatch);

            foreach (Goomba aGoomba in mGoombas)
                aGoomba.Draw(spriteBatch);
            foreach (MysteryBlock aMysteryBlock in mMysteryBlocks)
                aMysteryBlock.Draw(spriteBatch);
            foreach (Pipe aPipe in mPipes)
                aPipe.Draw(spriteBatch);
            mPlayer.Draw(spriteBatch);
        }
    }
}

