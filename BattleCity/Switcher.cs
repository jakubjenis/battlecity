using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace BattleCity
{
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class Switcher : Microsoft.Xna.Framework.GameComponent
    {
        int timeToGo, switchToState;
        bool timer;
        Engine game;
        public bool switching;
        KeyboardState previousState;
        public Switcher(Game gam, Engine game): base(gam)
        {
            timeToGo = -1;
            timer = false;
            this.game=game;
            switching = false;
        }

        public override void Initialize()
        {
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (timer)
            {
                timeToGo -= gameTime.ElapsedGameTime.Milliseconds;
                if (timeToGo <= 0)
                {
                    game.GameState = switchToState;
                    timer = false;
                    switching = false;
                }

            }
            base.Update(gameTime);
        }
        public void SwitchState( int switchToState, int time)
        {
            switching = true;
            timer = true;
            this.switchToState = switchToState;
            timeToGo = time;

            if ((switchToState == 0)||(switchToState==4))
            {
                game.GameOver = false;
                game.Player1.Lives = 3;
                game.Player2.Lives = 3;
                //game.Player1.Alive = true;
            }
            if (switchToState == 0)
            {
                MediaPlayer.Stop(); 
            }

        }
        public void CheckStates()
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                //GameState = 0;
                SwitchState(0, 0);
            }

            if (game.GameOver) //game over
            {
                game.KeyboardBlocked = true;
                if ((Keyboard.GetState().IsKeyDown(Keys.Escape)) || ((Keyboard.GetState().IsKeyDown(Keys.Enter)) && (previousState.IsKeyUp(Keys.Enter))))
                {
                    SwitchState(4, 0);
                    game.GameOver = false;
                }               
            }
            if (game.GameState == 4)
            {
                game.KeyboardBlocked = true;
                if ((Keyboard.GetState().IsKeyDown(Keys.Escape)) || ((Keyboard.GetState().IsKeyDown(Keys.Enter)) && (previousState.IsKeyUp(Keys.Enter))))
                {
                    SwitchState(0, 0);
                } 
            }

            if (game.GameState == 3) //cakanie pred levelom
            {
          
                game.KeyboardBlocked = true;
                if (!switching)
                {
                    SwitchState(1, 3500);                    
                }         
            }

            if (game.GameState == 1) //zacni hru
            {               
                    game.newLevel(game.CurrentLevel);
                    SwitchState(2, 0);
                
            }
            previousState = Keyboard.GetState();

            
        }
    }
}