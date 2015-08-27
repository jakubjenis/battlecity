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
    class Event
    {
        public string What;
        public int When;
        public int Par;
        public Event(string What, int When, int Par)
        {
            this.What = What;
            this.When = When;
            this.Par = Par;
        }

    }
    public class Planner : Microsoft.Xna.Framework.GameComponent
    {
        Engine game;
        List<Event> Events;
        public Planner(Game gam, Engine game)
            : base(gam)
        {
            Events=new List<Event>();
            this.game = game;
        }
      
        public override void Initialize()
        {            
            base.Initialize();
        }       
      
        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < Events.Count; i++)
            {
                Event temp = Events[i];
                int x, y;
                temp.When -= gameTime.ElapsedGameTime.Milliseconds;

                
                if (temp.When <= 0)
                {
                    if (temp.What == "enemyBorn")
                    {
                        x = 0;
                        y = 0;
                        if (temp.Par == 1)
                        {
                            y = game.Constants.BrickSize * 2;
                            x = 0;
                            switch ((20 - game.EnemiesToBeBorn) % 3)
                            {
                                case 0: x = game.Constants.BrickSize * 2; break;
                                case 1: x = game.Constants.BrickSize * 14; break;
                                case 2: x = game.Constants.BrickSize * 26; break;
                            }
                        }
                        else if (temp.Par == 2)
                        {
                            x = game.Constants.TankPositionPlayer1X;
                            y = game.Constants.TankPositionPlayer1Y;
                        }
                        else if (temp.Par == 3)
                        {
                            x = game.Constants.TankPositionPlayer2X;
                            y = game.Constants.TankPositionPlayer2Y;
                        }
                        game.NewTankAnim(x,y);
                    }
                    if (temp.What == "newEnemy")
                    {
                        y = game.Constants.BrickSize * 2;
                        x = 0;
                        switch ((20-game.EnemiesToBeBorn) % 3)
                        {
                            case 0: x = game.Constants.BrickSize * 2; break;
                            case 1: x = game.Constants.BrickSize * 14; break;
                            case 2: x = game.Constants.BrickSize * 26; break;
                        }
                        switch (temp.Par)
                        {
                            case 1:                            
                            game.Enemies.Add(game.CreateEnemy(x,y,game.EnemyTypes[20-game.EnemiesToBeBorn]));
                            game.EnemiesToBeBorn -= 1;
                            if ((game.EnemiesToBeBorn > 0))
                            {
                                PlanEvent("enemyBorn", 1300, 1);
                                PlanEvent("newEnemy", Engine.IntervalEnemy, 1);                                
                            }                          
                            break;
                        }
                        
                    } else if(temp.What=="bornPlayer")
                    {
                        
                        if (temp.Par == 1)
                        {
                            x = game.Constants.TankPositionPlayer1X;
                            y = game.Constants.TankPositionPlayer1Y;

                            game.Player1.Initialize(x, y, game.Constants.TankSizePlayer, game.Constants.TankSizePlayer, game.Constants.TanksSpeed[0], true, game.Player1.Lives,1,game.Constants.BulletsSpeed[0]);
                            PlanEvent("makeDestructable", 4000, 1);
                            game.Player1.CalculateSource(1);
                        }
                        else
                        {
                            x = game.Constants.TankPositionPlayer2X;
                            y = game.Constants.TankPositionPlayer2Y;

                            game.Player2.Initialize(x, y, game.Constants.TankSizePlayer, game.Constants.TankSizePlayer, game.Constants.TanksSpeed[0], true, game.Player2.Lives, 1, game.Constants.BulletsSpeed[0]);
                            PlanEvent("makeDestructable", 4000, 2);
                            game.Player2.CalculateSource(1);
                        }

                    }
                    else if (temp.What == "cancelReward")
                     {
                         if (game.Rewards.Count > 0)
                         {
                             for (int j = 0; j < game.Rewards.Count; j++)
                             {
                                 for (int k = 0; k < game.Animations.Count; k++)
                                 {
                                     if (game.Rewards[j] == game.Animations[k])
                                     {
                                         game.Animations.RemoveAt(k);
                                     }
                                 }
                                 game.Rewards.RemoveAt(j);
                                 game.rewardOnScene = false;
                             }

                         }
                    }
                   

                    Events.RemoveAt(i);
                }
            }
            base.Update(gameTime);
        }

        public void PlanEvent(string What, int When, int Par)
        {            
            if (What == "newEnemy")
            {               
                if ((game.EnemiesToKill - game.EnemiesToBeBorn + CountEvent("newEnemy")<=4)&&(game.EnemiesToBeBorn>0))
                {
                    Events.Add(new Event(What, When, Par));
                }
            }
            else if (What == "bornPlayer")
            {
                Events.Add(new Event(What, When, Par));
            }
            else if (What == "enemyBorn")
            {
                if ((game.EnemiesToKill - game.EnemiesToBeBorn + CountEvent("newEnemy")<=4)&&(game.EnemiesToBeBorn>0))
                {
                    Events.Add(new Event(What, When, Par));
                }
            }
            else
            {
                Events.Add(new Event(What, When, Par));
            }
        }

        public void Clear()
        {
            this.Events.Clear();
        }

        public int CountEvent(string what)
        {
            int i = 0;
            foreach (Event temp in Events)
            {
                if (temp.What == what)
                {
                    i++;
                }
            }
            return i;
        }
    }
}