using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using BattleCity.Graphics;

namespace BattleCity
{
    //Trieda starajuce sa o vsetky tanky. Riesi ich smer, rychlost, ci su este nazive, pocita NextFrameBound, ktory sa 
    //pouziva pri urcovani kolizii.
    public class Tank: Image
    {
        #region Fields

            int smer; //smer natocenia tanku 1..4  1..hore, 2..doprava, 3..dole, 4..dolava
            int bulletsShot; //pocet striel, ktore tank vystrelil, ale este nenarazili
            int x,y; //pozicia tanku
            int lives; // pocet zivotov, ak tank patri hracovi
            int resistance; //pocet striel potrebnych na znicenie stitu tanku
            protected int speed; //aktualna rychlost tanku
            int score; //pokial ide o hraca, drzi si skore, ktore dosiahol
            int type; //pokial ide o nepriatelsky tank, udava o aky typ tanku ide 1..5
            int maxShots; //maximum striel, ktore hrac moze vystrelit naraz
            int shotSpeed; //rychlost strely vystrelenej z tohto tanku

            bool alive; //indikuje, ci tank este zije
            bool indestructable=false; //indikuje, ci je tank momentalne neznicitelny (narodenie hraca)

            public bool Collision; //indikuje koliziu s cudzim predmetom
            public bool CollisionTank; //indikuje koliziu s cudzim tankom

            Rectangle NextFrameBoundTank; //vracia obdlznik vymedzujuci uzemie, v ktorom sa bude tank nachadzat, v dalsom snimu hry, ak nezmeni smer        
    
        #endregion 

        #region Properties

            public int Resistance
            {
                get { return resistance; }
                set { resistance = value; }
            }

            public bool Indestructable
            {
                get { return indestructable; }
                set { indestructable = value; }
            }

            public int Type
            {
                get { return type; }
                set { type = value; }
            }

            public int Score
            {
                get{return score;}
                set{score=value;}
            }

            public int Lives
            {
                get { return lives; }
                set { lives = value; }
            }

            public Rectangle BoundingRectangleTop
                {
                    get
                    {
                        float l=0, r=0, t=0, b=0;
                        if (this.Smer == 1)
                        {
                            l = this.PositionX - OriginX;
                            r = this.PositionX + OriginX;
                            t = this.PositionY - OriginY;
                            b = this.PositionY - OriginY + 1;
                        }
                        else if (this.Smer == 2)
                        {
                            l = this.PositionX + OriginX-1;
                            r = this.PositionX + OriginX;
                            t = this.PositionY - OriginY;
                            b = this.PositionY + OriginY;
                        }
                        else if (this.Smer == 3)
                        {
                            l = this.PositionX - OriginX;
                            r = this.PositionX + OriginX ;
                            t = this.PositionY + OriginY-1;
                            b = this.PositionY + OriginY;
                        }
                        else if (this.Smer == 4)
                        {
                            l = this.PositionX - OriginX;
                            r = this.PositionX - OriginX+1;
                            t = this.PositionY - OriginY;
                            b = this.PositionY + OriginY;
                        }
                        return new Rectangle((int)l, (int)t, (int)(r - l), (int)(b - t));
                    }
                }

            public int Smer
            {
                get { return smer; }
                set { smer = value; }
            }

            public int BulletsShot
            {
                get { return bulletsShot; }
                set { bulletsShot = value; }
            }

            public int X
            {
                get { return x; }
                set { x = value; }
            }

            public int Y
            {
                get { return y; }
                set { y = value; }
            }

            public bool Alive
            {
                get { return alive; }
                set { alive = value; }
            }

            public int MaxShots
            {
                get { return maxShots; }
                set { maxShots = value; }
            }
            
            public int ShotSpeed
            {
                get { return shotSpeed; }
                set { shotSpeed = value; }
            }

        #endregion 

        #region Methods

        public void Initialize(float X, float Y, float W, float H, int speed, bool alive,int lives,int maxShots,int shotSpeed)
        {
            base.Initialize(X, Y, W, H);
            this.smer = 1;
            this.alive = alive;
            this.Collision = false;
            this.speed = speed;
            this.lives = lives;
            this.Effects=SpriteEffects.None;
            this.indestructable = true;
            this.resistance = 0;
            this.maxShots = maxShots;
            this.shotSpeed = shotSpeed;
        }

        public override void LoadGraphicsContent(SpriteBatch spriteBatch, Texture2D texture)
        {
            base.LoadGraphicsContent(spriteBatch, texture);
            this.CalculateSource(1);
        }

        //spocita o kolko sa ma tank pohnut podla aktualneho smeru
        public void CalculateXY(int TankSpeedPlayer)
        {            
            if ((this.smer == 1) || (this.smer == 3))
            {
                x = 0;                
                y = TankSpeedPlayer;

                if (this.smer == 1)
                {
                    y *= -1;
                }
            }
            else
            {
                x = TankSpeedPlayer;
                y = 0;
                if (this.smer == 4)
                {
                    x *= -1;
                }
            }
        }

        virtual public Rectangle NextFrameBound()
        {
            if (smer == 1)
            {
                NextFrameBoundTank = new Rectangle((int)BoundingRectangle.Left,
                         (int)BoundingRectangle.Top - speed,
                         (int)Size.X,
                         (int)Size.Y);
            }
            else if (smer == 2)
            {
                NextFrameBoundTank = new Rectangle((int)BoundingRectangle.Left + speed, (int)BoundingRectangle.Top, (int)Size.X, (int)Size.Y);
            }
            else if (smer == 3)
            {
                NextFrameBoundTank = new Rectangle((int)BoundingRectangle.Left, (int)BoundingRectangle.Top + speed, (int)Size.X, (int)Size.Y);
            }
            else if (smer == 4)
            {
                NextFrameBoundTank = new Rectangle((int)BoundingRectangle.Left - speed, (int)BoundingRectangle.Top, (int)Size.X, (int)Size.Y);
            }
            return NextFrameBoundTank;

        }

        virtual public void Explode()
        {
            this.UnloadGraphicsContent();           
        }

        #endregion 

    }
}
