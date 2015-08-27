using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace BattleCity.Graphics
{   
    public class EnemyTank: Tank
    {
        Rectangle  NextFrameBoundTank;
        int x, y;
        
        Random rnd;
        int zvysok;
        List<Brick> bricks;
        Engine game;
        int nextShot;
        int wait;
        
        public EnemyTank(int speed, List<Brick> bricks,Engine game)
        {
            this.speed=speed;
            this.Alive = true;
            rnd = new Random();
            this.bricks = bricks;
            this.Collision = true;
            this.game = game;
            nextShot = 0;
        }

        public override Rectangle NextFrameBound()
        {
            NextFrameBoundTank = new Rectangle(base.BoundingRectangle.Left + x, base.BoundingRectangle.Top + y, (int)base.size.X, (int)base.size.Y);
            return NextFrameBoundTank;
        }

        public void UpdateEnemy(GameTime gameTime)
        {
            if (this.Alive)
            {
                
                if (nextShot>0)
                {
                    nextShot -= gameTime.ElapsedGameTime.Milliseconds;
                }
                if (nextShot < 0)
                {                     
                    game.Fire(this);                    
                    nextShot = 0;
                }

                if ((this.BulletsShot == 0) && (nextShot <= 0))
                {
                    nextShot = rnd.Next(1000)+100;
                }
                
                this.CalculateSource(this.Smer * 2 - 1);

                switch (this.Smer)
                {
                    case 1: x = 0; y = -speed; break;
                    case 2: x = speed; y = 0; break;
                    case 3: x = 0; y = speed; break;
                    case 4: x = -speed; y = 0; break;
                }

                if ((!this.Collision))
                {
                    this.PositionX += x;
                    this.PositionY += y;

                    if ((wait <= 0))
                    {
                        if ((this.Smer == 2) || (this.Smer == 4))
                        {
                            if ((this.PositionX - (game.Constants.BrickSize * 2)) % (this.game.Constants.BrickSize * 2) == 0)
                            {
                                int random = rnd.Next(100);
                                if (random > 50)
                                {
                                    int temp = ZistiSmer(this.Smer);
                                    if (temp != this.Smer)
                                    {
                                        this.Smer = temp;
                                        wait = rnd.Next(1000) + 200;
                                    }

                                }
                            }
                        }
                        else
                        {
                            if ((this.PositionY - game.Constants.BrickSize * 2) % (this.game.Constants.BrickSize * 2) == 0)
                            {
                                int random = rnd.Next(100);
                                if (random > 50)
                                {
                                    int temp = ZistiSmer(this.Smer);
                                    if (temp != this.Smer)
                                    {
                                        this.Smer = temp;
                                        wait = rnd.Next(1000) + 200;
                                    }

                                }
                            }
                        }
                    }
                }
                else
                {
                    if (wait <= 0)
                    {
                        this.Smer = ZistiSmer(this.Smer);
                        wait = rnd.Next(1000);
                    }  
                    zvysok = (int)PositionX % 24;
                    PositionX = (int)((int)PositionX / 24);
                    PositionX = (int)PositionX * 24;

                    if (zvysok > 12)
                    {
                        PositionX = PositionX + 24;
                    }

                    zvysok = (int)PositionY % 24;
                    PositionY = (int)((int)PositionY / 24);
                    PositionY = (int)PositionY * 24;

                    if (zvysok > 12)
                    {
                        PositionY = PositionY + 24;
                    }
                }

                wait -= gameTime.ElapsedGameTime.Milliseconds;
                this.Collision = false;
            }

        }
        public int ZistiSmer(int previous)
        {            
            
                bool[] smery = new bool[5];
                int total = 0;

                for (int i = 1; i <= 4; i++)
                {
                    smery[i] = false;
                }

                #region CheckAvailable
                smery[1] = true;
                for (int i = 1; i < bricks.Count; i++)
                {
                    if (new Rectangle(this.BoundingRectangle.Left, this.BoundingRectangle.Top - 24, (int)this.size.X, (int)this.size.Y).Intersects(bricks[i].BoundingRectangle))
                    {
                        smery[1] = false;
                    }
                }

                smery[2] = true;
                for (int i = 1; i < bricks.Count; i++)
                {
                    if (new Rectangle(this.BoundingRectangle.Left + 24, this.BoundingRectangle.Top, (int)this.size.X, (int)this.size.Y).Intersects(bricks[i].BoundingRectangle))
                    {
                        smery[2] = false;
                    }
                }

                smery[3] = true;
                for (int i = 1; i < bricks.Count; i++)
                {
                    if (new Rectangle(this.BoundingRectangle.Left, this.BoundingRectangle.Top + 24, (int)this.size.X, (int)this.size.Y).Intersects(bricks[i].BoundingRectangle))
                    {
                        smery[3] = false;
                    }
                }

                smery[4] = true;
                for (int i = 1; i < bricks.Count; i++)
                {
                    if (new Rectangle(this.BoundingRectangle.Left - 24, this.BoundingRectangle.Top, (int)this.size.X, (int)this.size.Y).Intersects(bricks[i].BoundingRectangle))
                    {
                        smery[4] = false;
                    }
                }


                if (this.PositionY <= 48) { smery[1] = false; }
                if (this.PositionY >= 624) { smery[3] = false; }
                if (this.PositionX >= 624) { smery[2] = false; }
                if (this.PositionX <= 48) { smery[4] = false; }

                #endregion

                for (int i = 1; i <= 4; i++)
                {
                    if (smery[i] == true)
                    {
                        total++;
                    }
                }

                int[] probability = new int[5];
                for (int i = 1; i <= 4; i++)
                {
                    probability[i] = 32;
                }
                for (int i = 1; i <= 4; i++)
                {
                    if (!smery[i])
                    {
                        probability[i] /= 4;
                    }
                }

                if (previous == 1)
                {
                    probability[3] /= 16;
                }
                if (previous == 2)
                {
                    probability[4] /= 16;
                }
                if (previous == 3)
                {
                    probability[1] /= 16;
                }
                if (previous == 4)
                {
                    probability[2] /= 16;
                }

                if (PositionY < 300)
                {
                    probability[1] /= 4;
                }
                if (PositionX < 100)
                {
                    probability[4] /= 2;
                }
                else if (PositionX > 524)
                {
                    probability[2] /= 2;
                }

                total = 0;
                for (int i = 1; i <= 4; i++)
                {
                    total += probability[i];
                }
                total = rnd.Next(total) + 1;

                int j = 0;
                while (total > 0)
                {
                    j++;
                    total -= probability[j];
                }
                //total = rnd.Next(total) + 1;

                //int j = 0;
                //while (total > 0)
                //{
                //    j++;
                //    if (smery[j] == true)
                //    {
                //        total--;
                //    }                
                //}
                return j;
            
        }
        public void Otoc()
        {
            int j=0;
            switch (this.Smer)
            {
                case 1: j = 3; break;
                case 2: j = 4; break;
                case 3: j = 1; break;
                case 4: j = 2; break;
            }
            this.CollisionTank = false;
            this.Smer= j;

            
        }
    }
}
