using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BattleCity;
using BattleCity.Graphics;


namespace BattleCity
{
    public class Menu
    {
        Engine game;
        int cursorState;
        KeyboardState keyboardState, previousState;
        Animation enter;
        Resources Res;
        
        int CursorState
        {
            get { return cursorState; }
            set
            {
                cursorState=value;
                if(cursorState>3)
                {
                    cursorState=0;
                }
                else if(cursorState<0)
                {
                    cursorState=3;
                }
            }
        }

        public Menu(Engine game, Animation enter, Resources R)
        {
            this.game = game;
            cursorState = 0;
            keyboardState = Keyboard.GetState();
            previousState = Keyboard.GetState();
            this.enter = enter;
            Res = R;
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {            
            spriteBatch.Draw(Res.Textures["Menu_Background"], game.viewportRect, Color.White);
            spriteBatch.Draw(Res.Textures["Menu_" + (cursorState+1)] , new Vector2(220, 509), Color.White);
            //enter.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();
            //enter.Update(gameTime);

            if (keyboardState.IsKeyDown(Keys.Left)&&(previousState.IsKeyUp(Keys.Left)))
            {
                CursorState -= 1;
            }
            if (keyboardState.IsKeyDown(Keys.Right) && (previousState.IsKeyUp(Keys.Right)))
            {
                CursorState += 1;
            }

            if ((keyboardState.IsKeyDown(Keys.Enter))&&(previousState.IsKeyUp(Keys.Enter)))
            {
                switch (cursorState)
                {
                    case 0: game.GameState= 3;                        
                        game.Player1.Alive = true;
                        game.Player2.Alive = false;
                        game.numberOfPlayers = 1;
                        game.Player1.Score = 0;
                        game.Player2.Score = 0;
                        MediaPlayer.Resume();    
                        break;
                    case 1: game.GameState = 3;
                        game.Player1.Alive = true;
                        game.Player2.Alive = true;
                        game.numberOfPlayers = 2;
                        game.Player1.Score = 0;
                        game.Player2.Score = 0;
                        MediaPlayer.Resume();           
                    break;
                    case 2: game.GameState = 6; break;
                    case 3: game.Exit(); break;
                }
            }


            previousState = keyboardState;
        }
    }
}