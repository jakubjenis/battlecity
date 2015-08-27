using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BattleCity;
using BattleCity.Graphics;

namespace BattleCity.Input
{
    
    public class Klavesnica
    {
        #region Fields (keyPressed, keyboardState, previousState, Keys, Player, zvysok)

        bool keyPressed;       
        KeyboardState keyboardState;
        KeyboardState previousState;
        Keys Space, Up, Down, Left, Right;
        Tank Player;
        int zvysok;
        
        #endregion 

        public bool KeyPressed
        {
            get { return keyPressed; }
        }

        public Klavesnica(Keys Space, Keys Up, Keys Down, Keys Left, Keys Right, Tank Player)
        {
            this.Space = Space;
            this.Up= Up;
            this.Down= Down;
            this.Right= Right;
            this.Left = Left;
            this.Player = Player;
        }

        public void Update(Engine game,   GameTime gameTime)
        {
            if (!game.KeyboardBlocked)
            {
                keyboardState = Keyboard.GetState();
                keyPressed = false;

                if ((keyboardState.IsKeyDown(Space) && (previousState.IsKeyUp(Space))))
                {
                    if ((Player.BulletsShot < Player.MaxShots)&&(Player.Alive))
                    {
                        game.Fire(Player);
                    }
                }

                if (keyboardState.IsKeyDown(Left) && (keyPressed == false))
                {
                    if (Player.Smer != 4)
                    {
                        Player.CalculateSource(3);
                        Player.Effects = SpriteEffects.None;
                    }

                    Player.Smer = 4;

                    if (!previousState.IsKeyDown(Left))
                    {
                        zvysok = (int)Player.PositionY % game.Constants.BrickSize;
                        Player.PositionY = (int)((int)Player.PositionY / game.Constants.BrickSize);
                        Player.PositionY = (int)Player.PositionY * game.Constants.BrickSize;

                        if (zvysok > game.Constants.BrickSize / 2)
                        {
                            Player.PositionY = Player.PositionY + game.Constants.BrickSize;
                        }
                    }
                    keyPressed = true;
                }
                if (keyboardState.IsKeyDown(Right) && (keyPressed == false))
                {
                    if (Player.Smer != 2)
                    {
                        Player.CalculateSource(3);
                        Player.Effects = SpriteEffects.FlipHorizontally;
                    }

                    Player.Smer = 2;

                    if (!previousState.IsKeyDown(Right))
                    {
                        zvysok = (int)Player.PositionY % game.Constants.BrickSize;
                        Player.PositionY = (int)((int)Player.PositionY / game.Constants.BrickSize);
                        Player.PositionY = (int)Player.PositionY * game.Constants.BrickSize;

                        if (zvysok > game.Constants.BrickSize / 2)
                        {
                            Player.PositionY = Player.PositionY + game.Constants.BrickSize;
                        }
                    }
                    keyPressed = true;
                }
                if (keyboardState.IsKeyDown(Down) && (keyPressed == false))
                {
                    if (Player.Smer != 3)
                    {
                        Player.CalculateSource(1);
                        Player.Effects = SpriteEffects.FlipVertically;
                    }

                    Player.Smer = 3;

                    if ((!previousState.IsKeyDown(Down)) || ((previousState.IsKeyDown(Right) && (previousState.IsKeyDown(Down))))
                        || ((previousState.IsKeyDown(Left) && (previousState.IsKeyDown(Down)))))
                    {
                        zvysok = (int)Player.PositionX % game.Constants.BrickSize;
                        Player.PositionX = (int)((int)Player.PositionX / game.Constants.BrickSize);
                        Player.PositionX = (int)Player.PositionX * game.Constants.BrickSize;

                        if (zvysok > game.Constants.BrickSize / 2)
                        {
                            Player.PositionX = Player.PositionX + game.Constants.BrickSize;
                        }
                    }
                    keyPressed = true;
                }
                if (keyboardState.IsKeyDown(Up) && (keyPressed == false))
                {
                    if (Player.Smer != 1)
                    {
                        Player.CalculateSource(1);
                        Player.Effects = SpriteEffects.None;
                    }

                    Player.Smer = 1;


                    zvysok = (int)Player.PositionX % game.Constants.BrickSize;
                    Player.PositionX = (int)((int)Player.PositionX / game.Constants.BrickSize);
                    Player.PositionX = (int)Player.PositionX * game.Constants.BrickSize;

                    if (zvysok > game.Constants.BrickSize / 2)
                    {
                        Player.PositionX = Player.PositionX + game.Constants.BrickSize;
                    }

                    keyPressed = true;
                }

                previousState = keyboardState;

            }   
        }
    }
}
