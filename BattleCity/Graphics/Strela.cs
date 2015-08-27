using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BattleCity.Graphics;

namespace BattleCity
{
    //Trieda starajuca sa o vystrelenu strelu, hracom, alebo nepriatelskym tankom
    public class Strela : Image
    {        
        #region Fields(smer, speed, alive, shotBy)

        int smer; //smer, v ktorom strela leti, smery rovnake ako pri smere tanku 1..4
        int speed; //rychlost strely
        bool alive; //indikuje, ci strela este nenarazila
        Tank shotBy; //urcuje kto strelu vystrelil, referencia na Tank      

        #endregion

        #region Properties (Alive, ShotBy)

            public bool Alive        
            {
                get { return alive; }
                set { alive = value; }
            }

            public Tank ShotBy
            {
                get { return shotBy; }
            }

        #endregion

        #region Methods(Initialize, LoadGraphicsContent, Update, CheckCollision, CheckCollisionShot, CheckCollisionTank)

        public void Initialize(Tank Player, int speed, int smer)
        {
            base.Initialize(Player.PositionX ,Player.PositionY, 10, 10 );
            this.speed = speed;
            this.alive = true;
            this.smer = smer;            
            this.shotBy = Player;
        }

        public override void LoadGraphicsContent(SpriteBatch spriteBatch, Texture2D texture)
        {
            base.LoadGraphicsContent(spriteBatch, texture);
            switch (smer)
            {
                case 1: this.CalculateSource(2); this.Effects = SpriteEffects.None; break;
                case 2: this.CalculateSource(1); this.Effects = SpriteEffects.FlipHorizontally; break;
                case 3: this.CalculateSource(2); this.Effects = SpriteEffects.FlipVertically; break;
                case 4: this.CalculateSource(1); this.Effects = SpriteEffects.FlipVertically; break;

            }
            
        }

        public void Explode()
        {
            this.ShotBy.BulletsShot -= 1;
            this.UnloadGraphicsContent();
        }

        //pohne strelou v jej smere
        public void Update()
        {            
            bool pom = alive;
            if (alive)
            {
                switch (smer)
                {
                    default: break;
                    case 1: PositionY -= speed; break;
                    case 2: PositionX += speed; break;
                    case 3: PositionY += speed; break;
                    case 4: PositionX -= speed; break;
                }
            }            
        }

        //skontroluje koliziu s tehlou brick, vracia true ak kolizia nastala
        public bool CheckCollisions(Brick brick)
        {
            if (this.BoundingRectangle.Intersects(brick.BoundingRectangleBrick))
            {
                this.alive = false;                

                if (brick.Shootable)
                {
                    if (brick.State == 1)
                    {
                        switch (this.smer)
                        {
                            case 1: brick.State = 5; break;
                            case 2: brick.State = 2; break;
                            case 3: brick.State = 4; break;
                            case 4: brick.State = 3; break;
                        }
                    }
                    else if (brick.State == 2)
                    {
                        switch (this.smer)
                        {
                            case 1: brick.State = 0; break;
                            case 2: brick.State = 0; break;
                            case 3: brick.State = 0; break;
                            case 4: brick.State = 0; break;
                        }
                    }
                    else if (brick.State == 3)
                    {
                        switch (this.smer)
                        {
                            case 1: brick.State = 0; break;
                            case 2: brick.State = 0; break;
                            case 3: brick.State = 0; break;
                            case 4: brick.State = 0; break;
                        }
                    }
                    else if (brick.State == 4)
                    {
                        switch (this.smer)
                        {
                            case 1: brick.State = 0; break;
                            case 2: brick.State = 0; break;
                            case 3: brick.State = 0; break;
                            case 4: brick.State = 0; break;
                        }
                    }
                    else if (brick.State == 5)
                    {
                        switch (this.smer)
                        {
                            case 1: brick.State = 0; break;
                            case 2: brick.State = 0; break;
                            case 3: brick.State = 0; break;
                            case 4: brick.State = 0; break;
                        }
                    }


                    if (brick.State == 0)
                    {
                        brick.Alive = false;
                        brick.Explode();
                    }
                    else
                    {
                        brick.CalculateSource(brick.State);
                    }
                }
                return true;
            }
            return false;
        }

        //skontroluje koliziu s tankom Tank, vracia true ak kolizia nastala   
        internal bool CheckCollisionsTank(Tank temp)
        {
            if (this.BoundingRectangle.Intersects(temp.BoundingRectangle)&&(temp.Alive))
            {
                this.Alive = false;
                if (!temp.Indestructable)
                {
                    if (temp.Resistance > 0)
                    {
                         temp.Resistance -= 1;
                         return false;
                    }
                    else
                    {
                        temp.Alive = false;
                        return true;
                    }
                }                           
                                
                return true;
            }
            else
            {
                return false;
            }
        }

        //skontroluje koliziu so strelou strelicka
        public bool CheckCollisionsShot(Strela strelicka)
        {
            if (this.BoundingRectangle.Intersects(strelicka.BoundingRectangle) && (strelicka.Alive))
            {
                strelicka.Alive = false;
                this.Alive = false;
                //this.ShotBy.BulletsShot -= 1; 
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion 
    }
}

