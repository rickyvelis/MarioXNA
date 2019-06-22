using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace SuperMarioWorldXna
{
    public class LevelScreen : GameScreen
    {
        private Player player;
        private HUD hud;
        private Life life;
        private Score score1;
        private Score score2;
        private Castle castle;
        private CastleDoor castledoor;
        private List<Goomba> goombas = new List<Goomba>();
        private List<Koopa> koopas = new List<Koopa>();
        private List<Tile> tiles = new List<Tile>();
        private List<Block> blocks = new List<Block>();
        private List<Pipe> pipes = new List<Pipe>();
        private List<Ground> ground = new List<Ground>();
        private List<Coin> coins = new List<Coin>();
        private List<Mushroom> mushrooms = new List<Mushroom>();
        private ScrollingBackground background;
        private float tileSize;
        private int currentLevel;
        private float time;
        private int levelAmount;

        /// <summary>
        /// Geef een levelnummer mee voor het level dat je wilt laden.
        /// </summary>
        /// <param name="aLevel"></param>
        public LevelScreen(int aLevel)
        {
            currentLevel = aLevel;
            LoadLevel(aLevel);
            levelAmount = 3;
        }

        /// <summary>
        /// Laad de content voor elk object.
        /// </summary>
        /// <param name="theContentManager"></param>
        public override void LoadContent(ContentManager theContentManager)
        {
            base.LoadContent(theContentManager);

            background = new ScrollingBackground(Game1.Instance.GraphicsDevice.Viewport);
            background.AddBackground("images/background1");
            background.AddBackground("images/background1");
            background.AddBackground("images/background1");
            background.AddBackground("images/background1");
            background.AddBackground("images/background1");
            background.AddBackground("images/background1");
            background.AddBackground("images/background1");
            background.LoadContent(theContentManager);
            //foreaches kunnen weg!!!z
            foreach (Goomba aGoomba in goombas)
                aGoomba.LoadContent(theContentManager);
            foreach (Koopa aKoopa in koopas)
                aKoopa.LoadContent(theContentManager);
            foreach (Block aBlock in blocks)
                aBlock.LoadContent(theContentManager);
            foreach (Pipe aPipe in pipes)
                aPipe.LoadContent(theContentManager);
            foreach (Ground aGround in ground)
                aGround.LoadContent(theContentManager);
            foreach (Coin aCoin in coins)
                aCoin.LoadContent(theContentManager);
            foreach (Mushroom aMushroom in mushrooms)
                aMushroom.LoadContent(theContentManager);

            player.LoadContent(theContentManager);

            castle.LoadContent(theContentManager);
            castledoor.LoadContent(theContentManager);
            hud = new HUD(new Vector2(0, 20));
            hud.LoadContent(theContentManager);

            life = new Life(new Vector2(0, 20));
            life.LoadContent(theContentManager);

            score1 = new Score(new Vector2(0, 0), 1);
            score1.LoadContent(theContentManager);
            score2 = new Score(new Vector2(0, 0), 2);
            score2.LoadContent(theContentManager);
        }

        /// <summary>
        /// Leest een .xml file met verschillende char's op verschillende posities om een level weer te geven met Objecten op de gewenste coordinaten
        /// </summary>
        /// <param name="aLevel"></param>
        public void LoadLevel(int aLevel)
        {
            //Laad een level uit een .xml file. Na de 3e level wordt de game gereset.
            string aLevelFile = "./Levels/level01.xml";
            switch (aLevel)
            {
                case 1:
                    aLevelFile = "./Levels/level01.xml";
                    break;
                case 2:
                    aLevelFile = "./Levels/level02.xml";
                    break;
                case 3:
                    aLevelFile = "./Levels/level03.xml";
                    break;
                default:
                    ResetGame();
                    break;
            }

            //Leest per regel uit de .xml wat er moet worden weergegeven op welke positie; elke volgende regel is Y += 16 en elke volgende letter is X += 16
            XmlReader reader = XmlReader.Create(aLevelFile);
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
                    case 'K':
                        koopas.Add(new Koopa(new Vector2(aTile.mPosition.X, aTile.mPosition.Y)));
                        break;
                    case 'M':
                        player = new Player(new Vector2(aTile.mPosition.X, aTile.mPosition.Y));
                        break;
                    case '?':
                        blocks.Add(new Block(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Mystery"));
                        break;
                    case '¿':
                        blocks.Add(new Block(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Mystery", "Mushroom"));
                        mushrooms.Add(new Mushroom(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), true));
                        break;
                    case 'S':
                        blocks.Add(new Block(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Stone"));
                        break;
                    case 'b':
                        blocks.Add(new Block(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Brick"));
                        break;
                    case 'P':
                        pipes.Add(new Pipe(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Top"));
                        break;
                    case 'B':
                        pipes.Add(new Pipe(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Bottom"));
                        break;
                    case '$':
                        coins.Add(new Coin(new Vector2(aTile.mPosition.X, aTile.mPosition.Y)));
                        break;
                    case 'U':
                        mushrooms.Add(new Mushroom(new Vector2(aTile.mPosition.X, aTile.mPosition.Y)));
                        break;
                    case 'I':
                        castle = new Castle(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Big");
                        castledoor = new CastleDoor(new Vector2(aTile.mPosition.X + 71, aTile.mPosition.Y + 144));
                        break;
                    case 'J':
                        castle = new Castle(new Vector2(aTile.mPosition.X, aTile.mPosition.Y), "Small");
                        castledoor = new CastleDoor(new Vector2(aTile.mPosition.X + 39, aTile.mPosition.Y + 49));
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
        /// Reset het level d.m.v. een nieuwe Levelscreen te maken met dezelfde levelnr.
        /// </summary>
        public void ResetLevel()
        {
            ScreenManager.Instance.AddScreen(new LevelScreen(currentLevel));
        }

        /// <summary>
        /// Reset het spel door Life op 3 en Coins op 0 te zetten en een nieuwe TitleScreen te maken.
        /// </summary>
        public void ResetGame()
        {
            Game1.Instance.Life = 3;
            Game1.Instance.CoinScore = 0;
            ScreenManager.Instance.AddScreen(new TitleScreen());
        }

        /// <summary>
        /// Maak een nieuwe LevelScreen met +1 bij het levelnr. Bij uitvoering in het laatste level wordt de game gereset.
        /// </summary>
        public void NextLevel()
        {
            if (currentLevel < levelAmount)
            {
                currentLevel += 1;
                ScreenManager.Instance.AddScreen(new LevelScreen(currentLevel));
            }
            else
                ResetGame();
        }

        /// <summary>
        /// Handlet de dingen die moeten gebeuren indien de player sterft.
        /// </summary>
        public void EndGame()
        {
            player.Death();
            life.UpdateLife();
            foreach (Goomba aGoomba in goombas)
                aGoomba.Freeze();
            foreach (Koopa aKoopa in koopas)
                aKoopa.Freeze();
        }

        /// <summary>
        /// Handlet het gedrag van objecten indien deze met elkaar colliden
        /// </summary>
        public void CheckCollision()
        {
            Rectangle overlap;

            //Goomba collision
            foreach (Goomba aGoomba in goombas)
            {
                if (!aGoomba.IsDead)
                {
                    //Goomba collision met Player
                    overlap = Rectangle.Intersect(player.BoundingBox, aGoomba.BoundingBox);
                    if (player.BoundingBox.Intersects(aGoomba.BoundingBox))
                        if (overlap.Width > overlap.Height)
                        {
                            if (player.mSpritePosition.Y < aGoomba.mSpritePosition.Y)
                                aGoomba.Death();
                            else
                                EndGame();
                        }
                        else
                        {
                            if (player.GetSize == "Small" && !player.IsInvincible)
                                EndGame();
                            else
                            {
                                player.UpdateSize("Small");
                                if (!player.IsInvincible)
                                    player.MakeInvincible();
                            }
                        }
                    //Goomba collision met Koopa
                    foreach (Koopa aKoopa in koopas)
                    {
                        if (!aKoopa.IsDead)
                        {
                            if (aGoomba.BoundingBox.Intersects(aKoopa.BoundingBox) && aGoomba.BoundingBox.X < aKoopa.BoundingBox.X)
                            {
                                if (aGoomba.FacingRight)
                                    aGoomba.ChangeDirection("Left");
                                if (!aKoopa.FacingRight)
                                    aKoopa.ChangeDirection("Right");
                            }
                            if (aGoomba.BoundingBox.Intersects(aKoopa.BoundingBox) && aGoomba.BoundingBox.X > aKoopa.BoundingBox.X)
                            {
                                if (!aGoomba.FacingRight)
                                    aGoomba.ChangeDirection("Right");
                                if (aKoopa.FacingRight)
                                    aKoopa.ChangeDirection("Left");
                            }
                        }
                    }
                }
            }

            //Koopa collision met Player
            foreach (Koopa aKoopa in koopas)
            {
                overlap = Rectangle.Intersect(player.BoundingBox, aKoopa.BoundingBox);
                if (player.BoundingBox.Intersects(aKoopa.BoundingBox))
                {
                    if (overlap.Width > overlap.Height)
                    {
                        if (player.mSpritePosition.Y < aKoopa.mSpritePosition.Y)
                            aKoopa.Death();
                    }
                    else if (!aKoopa.IsDead)
                    {
                        if (player.GetSize == "Small" && !player.IsInvincible)
                            EndGame();
                        else
                        {
                            player.UpdateSize("Small");
                            if (!player.IsInvincible)
                                player.MakeInvincible();
                        }
                    }
                }
            }

            //Mushroom collision met Player
            foreach (Mushroom aMushroom in mushrooms)
            {
                if (aMushroom.BoundingBox.Intersects(player.BoundingBox) && !aMushroom.IsDead && !aMushroom.Hidden)
                {
                    aMushroom.Death();
                    if (player.GetSize == "Small")
                        player.UpdateSize("Big");
                }
            }

            //Player Collision met CastleDoor
            if (player.BoundingBox.Intersects(castledoor.BoundingBox))
            {
                NextLevel();
            }

            //Player Collision met Coin
            foreach (Coin aCoin in coins)
            {
                if (player.BoundingBox.Intersects(aCoin.BoundingBox) && !aCoin.IsDead)
                {
                    aCoin.Death();
                    Game1.Instance.CoinScore++;
                }
            }

            //Pipe Collision
            foreach (Pipe aPipe in pipes)
            {
                //Pipe Collision met Goomba
                foreach (Goomba aGoomba in goombas)
                {
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
                //Pipe Collision met Mushroom
                foreach (Mushroom aMushroom in mushrooms)
                {
                    if (aMushroom.BoundingBox.Intersects(aPipe.BoundingBox))
                    {
                        overlap = Rectangle.Intersect(aMushroom.BoundingBox, aPipe.BoundingBox);
                        if (overlap.Width > overlap.Height)
                        {
                            if (aMushroom.mSpritePosition.Y < aPipe.mSpritePosition.Y)
                            {
                                aMushroom.mSpritePosition.Y = aPipe.BoundingBox.Top - aMushroom.Source.Height;
                                aMushroom.VelocityY = 0;
                            }
                        }
                        else
                        {
                            if (aMushroom.mSpritePosition.X > aPipe.mSpritePosition.X && aMushroom.mSpritePosition.Y + aMushroom.Source.Height > aPipe.mSpritePosition.Y + 10)
                                aMushroom.ChangeDirection("Right");
                            else if (aMushroom.mSpritePosition.X < aPipe.mSpritePosition.X && aMushroom.mSpritePosition.Y + aMushroom.Source.Height > aPipe.mSpritePosition.Y + 10)
                                aMushroom.ChangeDirection("Left");
                        }
                    }
                }
                //Pipe collision met Koopa
                foreach (Koopa aKoopa in koopas)
                {
                    if (aKoopa.BoundingBox.Intersects(aPipe.BoundingBox))
                    {
                        overlap = Rectangle.Intersect(aKoopa.BoundingBox, aPipe.BoundingBox);
                        if (overlap.Width > overlap.Height)
                        {
                            if (aKoopa.mSpritePosition.Y < aPipe.mSpritePosition.Y)
                            {
                                aKoopa.mSpritePosition.Y = aPipe.BoundingBox.Top - aKoopa.Source.Height;
                                aKoopa.VelocityY = 0;
                            }
                        }
                        else
                        {
                            if (aKoopa.mSpritePosition.X > aPipe.mSpritePosition.X && aKoopa.mSpritePosition.Y + aKoopa.Source.Height > aPipe.mSpritePosition.Y + 10)
                                aKoopa.ChangeDirection("Right");
                            else if (aKoopa.mSpritePosition.X < aPipe.mSpritePosition.X && aKoopa.mSpritePosition.Y + aKoopa.Source.Height > aPipe.mSpritePosition.Y + 10)
                                aKoopa.ChangeDirection("Left");
                        }
                    }
                }
                //Pipe Collision met Player
                if (player.BoundingBox.Intersects(aPipe.BoundingBox))
                {
                    overlap = Rectangle.Intersect(player.BoundingBox, aPipe.BoundingBox);
                    if (overlap.Width > overlap.Height)
                    {
                        if (player.mSpritePosition.Y > aPipe.mSpritePosition.Y)
                            player.mSpritePosition.Y = aPipe.BoundingBox.Bottom;
                        if (player.mSpritePosition.Y < aPipe.mSpritePosition.Y)
                        {
                            player.mSpritePosition.Y = aPipe.BoundingBox.Top - player.Source.Height;
                            player.VelocityY = 0;
                        }
                    }
                    else
                    {
                        if (player.mSpritePosition.X > aPipe.mSpritePosition.X && player.mSpritePosition.Y + player.Source.Height > aPipe.mSpritePosition.Y + 10)
                            player.mSpritePosition.X = aPipe.BoundingBox.Right;
                        else if (player.mSpritePosition.X < aPipe.mSpritePosition.X && player.mSpritePosition.Y + player.Source.Height > aPipe.mSpritePosition.Y + 10)
                            player.mSpritePosition.X = aPipe.BoundingBox.Left - player.Source.Width;
                    }
                }
                else if (player.OnGround == true)
                {
                    player.VelocityY = 4.5f;
                }
            }

            //Ground collision
            foreach (Ground aGround in ground)
            {
                if (aGround.Type == "North" || aGround.Type == "NorthWest" || aGround.Type == "NorthEast")
                {
                    //Ground collision met Player
                    if (player.BoundingBox.Intersects(aGround.BoundingBox))
                    {
                        overlap = Rectangle.Intersect(player.BoundingBox, aGround.BoundingBox);
                        if (overlap.Width > overlap.Height)
                        {
                            if (player.mSpritePosition.Y < aGround.mSpritePosition.Y)
                            {
                                player.mSpritePosition.Y = aGround.BoundingBox.Top - player.Source.Height;
                                player.VelocityY = 0;
                            }
                        }
                    }
                    else if (player.OnGround == true)
                    {
                        player.VelocityY = 4.5f;
                    }
                    //Ground collision met Goomba
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
                    //Ground collision met Koopa
                    foreach (Koopa aKoopa in koopas)
                    {
                        if (aKoopa.BoundingBox.Intersects(aGround.BoundingBox))
                        {
                            overlap = Rectangle.Intersect(aKoopa.BoundingBox, aGround.BoundingBox);
                            if (overlap.Width > overlap.Height)
                            {
                                if (aKoopa.mSpritePosition.Y < aGround.mSpritePosition.Y)
                                {
                                    aKoopa.mSpritePosition.Y = aGround.BoundingBox.Top - aKoopa.Source.Height;
                                    aKoopa.VelocityY = 0;
                                }
                            }
                        }
                    }
                    //Ground collision met Mushroom
                    foreach (Mushroom aMushroom in mushrooms)
                    {
                        if (aMushroom.BoundingBox.Intersects(aGround.BoundingBox))
                        {
                            overlap = Rectangle.Intersect(aMushroom.BoundingBox, aGround.BoundingBox);
                            if (overlap.Width > overlap.Height)
                            {
                                if (aMushroom.mSpritePosition.Y < aGround.mSpritePosition.Y)
                                {
                                    aMushroom.mSpritePosition.Y = aGround.BoundingBox.Top - aMushroom.Source.Height;
                                    aMushroom.VelocityY = 0;
                                }
                            }
                        }
                    }
                }
            }

            //Block collision
            foreach (Block aBlock in blocks)
            {
                if (!aBlock.IsDead)
                {
                    //Block collision met Goomba
                    foreach (Goomba aGoomba in goombas)
                    {
                        if (aGoomba.BoundingBox.Intersects(aBlock.BoundingBox))
                        {
                            overlap = Rectangle.Intersect(aGoomba.BoundingBox, aBlock.BoundingBox);
                            if (overlap.Width > overlap.Height)
                            {
                                if (aGoomba.mSpritePosition.Y < aBlock.mSpritePosition.Y)
                                {
                                    aGoomba.mSpritePosition.Y = aBlock.BoundingBox.Top - aGoomba.Source.Height;
                                    aGoomba.VelocityY = 0;
                                }
                            }
                            else
                            {
                                if (aGoomba.mSpritePosition.X > aBlock.mSpritePosition.X && aGoomba.mSpritePosition.Y + aGoomba.Source.Height > aBlock.mSpritePosition.Y + 10)
                                    aGoomba.ChangeDirection("Right");
                                else if (aGoomba.mSpritePosition.X < aBlock.mSpritePosition.X && aGoomba.mSpritePosition.Y + aGoomba.Source.Height > aBlock.mSpritePosition.Y + 10)
                                    aGoomba.ChangeDirection("Left");
                            }
                        }
                    }
                    //Block collision met Mushroom
                    foreach (Mushroom aMushroom in mushrooms)
                    {
                        if (aMushroom.BoundingBox.Intersects(aBlock.BoundingBox))
                        {
                            overlap = Rectangle.Intersect(aMushroom.BoundingBox, aBlock.BoundingBox);
                            if (overlap.Width > overlap.Height)
                            {
                                if (aMushroom.mSpritePosition.Y < aBlock.mSpritePosition.Y)
                                {
                                    aMushroom.mSpritePosition.Y = aBlock.BoundingBox.Top - aMushroom.Source.Height;
                                    aMushroom.VelocityY = 0;
                                }
                            }
                            else
                            {
                                if (aMushroom.mSpritePosition.X > aBlock.mSpritePosition.X && aMushroom.mSpritePosition.Y + aMushroom.Source.Height > aBlock.mSpritePosition.Y + 10)
                                    aMushroom.ChangeDirection("Right");
                                else if (aMushroom.mSpritePosition.X < aBlock.mSpritePosition.X && aMushroom.mSpritePosition.Y + aMushroom.Source.Height > aBlock.mSpritePosition.Y + 10)
                                    aMushroom.ChangeDirection("Left");
                            }
                        }
                    }
                    //Block collision met Koopa
                    foreach (Koopa aKoopa in koopas)
                    {
                        if (aKoopa.BoundingBox.Intersects(aBlock.BoundingBox))
                        {
                            overlap = Rectangle.Intersect(aKoopa.BoundingBox, aBlock.BoundingBox);
                            if (overlap.Width > overlap.Height)
                            {
                                if (aKoopa.mSpritePosition.Y < aBlock.mSpritePosition.Y)
                                {
                                    aKoopa.mSpritePosition.Y = aBlock.BoundingBox.Top - aKoopa.Source.Height;
                                    aKoopa.VelocityY = 0;
                                }
                            }
                            else
                            {
                                if (aKoopa.mSpritePosition.X > aBlock.mSpritePosition.X && aKoopa.mSpritePosition.Y + aKoopa.Source.Height > aBlock.mSpritePosition.Y + 10)
                                    aKoopa.ChangeDirection("Right");
                                else if (aKoopa.mSpritePosition.X < aBlock.mSpritePosition.X && aKoopa.mSpritePosition.Y + aKoopa.Source.Height > aBlock.mSpritePosition.Y + 10)
                                    aKoopa.ChangeDirection("Left");
                            }
                        }
                    }
                    //Block collision met PLayer
                    if (player.BoundingBox.Intersects(aBlock.BoundingBox))
                    {
                        overlap = Rectangle.Intersect(player.BoundingBox, aBlock.BoundingBox);
                        if (overlap.Width > overlap.Height)
                        {
                            if (player.mSpritePosition.Y > aBlock.mSpritePosition.Y)
                            {
                                player.mSpritePosition.Y = aBlock.BoundingBox.Bottom;
                                if (!player.OnGround && aBlock.Type == "Mystery")
                                {
                                    aBlock.Type = "Used";
                                    if (aBlock.Item == "Mushroom")
                                    {
                                        foreach (Mushroom aMushroom in mushrooms)
                                        {
                                            if (aMushroom.mSpritePosition == aBlock.mSpritePosition && aMushroom.Hidden)
                                            {
                                                aMushroom.mSpritePosition.Y = aBlock.mSpritePosition.Y - tileSize;
                                                aMushroom.Hidden = false;
                                            }
                                        }
                                    }
                                    else
                                        Game1.Instance.CoinScore++;
                                }
                                if (!player.OnGround && aBlock.Type == "Brick")
                                    if (player.GetSize == "Big")
                                        aBlock.IsDead = true;
                            }
                            else
                            {
                                player.mSpritePosition.Y = aBlock.BoundingBox.Top - player.Source.Height;
                                player.VelocityY = 0;
                                player.VelocityY = 0f;
                            }
                        }
                        else
                        {
                            if (player.mSpritePosition.X > aBlock.mSpritePosition.X && player.mSpritePosition.Y + player.Source.Height > aBlock.mSpritePosition.Y + 10)
                                player.mSpritePosition.X = aBlock.BoundingBox.Right;
                            else if (player.mSpritePosition.X < aBlock.mSpritePosition.X && player.mSpritePosition.Y + player.Source.Height > aBlock.mSpritePosition.Y + 10)
                                player.mSpritePosition.X = aBlock.BoundingBox.Left - player.Source.Width;
                        }
                    }
                    else if (player.OnGround == true)
                    {
                        player.VelocityY = 4.5f;
                    }
                }
            }
        }

        /// <summary>
        /// De gameloop.
        /// </summary>
        /// <param name="theGameTime"></param>
        public override void Update(GameTime theGameTime)
        {
            KeyboardState keyState = Keyboard.GetState();

            player.Update(theGameTime);
            life.Update(theGameTime);
            score1.Update(theGameTime);
            score2.Update(theGameTime);

            //Laat de game resetten indien ESC wordt ingedrukt
            if (keyState.IsKeyDown(Keys.Escape))
                ResetGame();

            //Update alle objecten ACCOLADES!!!
            foreach (Goomba aGoomba in goombas)
            {
                if (aGoomba.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aGoomba.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                && !aGoomba.IsDead
                || aGoomba.mSpritePosition.Y > ScreenManager.Instance.Dimensions.Y)
                    aGoomba.Update(theGameTime);
            }
            foreach (Koopa aKoopa in koopas)
            {
                if (aKoopa.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aKoopa.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                && !aKoopa.IsDead
                || aKoopa.mSpritePosition.Y > ScreenManager.Instance.Dimensions.Y)
                    aKoopa.Update(theGameTime);
            }
            foreach (Block aBlock in blocks)
            {
                if (aBlock.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aBlock.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                && !aBlock.IsDead)
                    aBlock.Update(theGameTime);
            }
            foreach (Coin aCoin in coins)
            {
                if (!aCoin.IsDead)
                    aCoin.Update(theGameTime);
            }
            foreach (Mushroom aMushroom in mushrooms)
            {
                if (!aMushroom.IsDead)
                    aMushroom.Update(theGameTime);
            }

            //Laat de HUD elementen meebewegen met de camera
            hud.mSpritePosition = new Vector2(Camera.Instance.mCameraPosition.X + ((int)ScreenManager.Instance.Dimensions.X - hud.Source.Width) / 2, Camera.Instance.mCameraPosition.Y + 20);
            life.mSpritePosition = new Vector2(Camera.Instance.mCameraPosition.X + ((int)ScreenManager.Instance.Dimensions.X - hud.Source.Width), Camera.Instance.mCameraPosition.Y + 40);
            score1.mSpritePosition = new Vector2(Camera.Instance.mCameraPosition.X + ((int)ScreenManager.Instance.Dimensions.X - hud.Source.Width) * 5 - 30, Camera.Instance.mCameraPosition.Y + 22);
            score2.mSpritePosition = new Vector2(Camera.Instance.mCameraPosition.X + ((int)ScreenManager.Instance.Dimensions.X - hud.Source.Width) * 5 - 48, Camera.Instance.mCameraPosition.Y + 22);

            //Check de collision
            CheckCollision();

            //Laat de tijd lopen voor de delay tussen het sterfen van mario en het laden van het volgende scherm
            if (player.IsDead)
                time += (float)theGameTime.ElapsedGameTime.TotalMilliseconds;

            //Als de tijd 2000 of boven is
            if (time >= 2000)
            {
                if (Game1.Instance.Life < 0)
                    ResetGame();
                else
                    ResetLevel();
            }

            //Als de Player het scherm verlaat via de onderkant van het scherm, wordt het spel beëindingd
            if (player.mSpritePosition.Y >= ScreenManager.Instance.Dimensions.Y)
                EndGame();
        }

        /// <summary>
        /// Tekent alle objecten op het scherm
        /// </summary>
        /// <param name="theSpriteBatch"></param>
        public override void Draw(SpriteBatch theSpriteBatch)
        {
            background.Draw(theSpriteBatch);
            castle.Draw(theSpriteBatch);
            castledoor.Draw(theSpriteBatch);

            foreach (Pipe aPipe in pipes)
                if (aPipe.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aPipe.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X)
                    aPipe.Draw(theSpriteBatch);

            foreach (Ground aGround in ground)
                if (aGround.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aGround.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X)
                    aGround.Draw(theSpriteBatch);

            foreach (Block aBlock in blocks)
                if (aBlock.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aBlock.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                && !aBlock.IsDead)
                    aBlock.Draw(theSpriteBatch);

            foreach (Coin aCoin in coins)
                if (aCoin.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aCoin.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                && !aCoin.IsDead)
                    aCoin.Draw(theSpriteBatch);

            foreach (Goomba aGoomba in goombas)
                if (aGoomba.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aGoomba.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                && !aGoomba.IsDead
                || aGoomba.mSpritePosition.Y > ScreenManager.Instance.Dimensions.Y)
                    aGoomba.Draw(theSpriteBatch);

            foreach (Koopa aKoopa in koopas)
                if (aKoopa.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aKoopa.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                && !aKoopa.IsDead
                || aKoopa.mSpritePosition.Y > ScreenManager.Instance.Dimensions.Y)
                    aKoopa.Draw(theSpriteBatch);

            foreach (Mushroom aMushroom in mushrooms)
                if (aMushroom.mSpritePosition.X > Camera.Instance.PositionX - tileSize
                && aMushroom.mSpritePosition.X < Camera.Instance.PositionX + ScreenManager.Instance.Dimensions.X
                && !aMushroom.IsDead
                && !aMushroom.Hidden
                || aMushroom.mSpritePosition.Y > ScreenManager.Instance.Dimensions.Y)
                    aMushroom.Draw(theSpriteBatch);

            hud.Draw(theSpriteBatch);
            life.Draw(theSpriteBatch);
            score1.Draw(theSpriteBatch);
            score2.Draw(theSpriteBatch);
            player.Draw(theSpriteBatch);
        }
    }
}