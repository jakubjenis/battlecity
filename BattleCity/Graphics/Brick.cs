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
    public class Brick:Image
    {
        #region Fields (State, BoundingRectangleBrick, Alive, Passable, Shootable, ShotPassable)

            bool passable; //prejde cez neho tank
            bool shootable; //da sa zostrelit  
            bool shotPassable; //prejde cez neho strela 
            bool alive;
            int state;

        #endregion 

        #region Properties (State, BoundingRectangleBrick, Alive, Passable, Shootable, ShotPassable)

        public int State
            {
                get { return state; }
                set { state=value; } 
            }

            public Rectangle BoundingRectangleBrick
            {
                get
                {
                    int left = (int)(PositionX - OriginX);
                    int top = (int)(PositionY - OriginY);
                    int w = (int)this.size.X;
                    int h = (int)this.size.Y;

                    if (state == 2)
                    {
                        left += w / 2;
                        w /= 2;
                    }
                    else if (state == 3)
                    {
                        w = (int)(w / 2);
                    }
                    else if (state == 4)
                    {
                        top += h / 2;
                        h /= 2;
                    }
                    else if (state == 5)
                    {
                        h = (int)(h / 2);
                    }

                    return new Rectangle(left, top, w, h);
                }
            }

            public bool Alive
            {
                get { return alive; }
                set { alive = value; }
            }

            public bool Passable
            {
                get { return passable; }
                set { passable = value; }
            }

            public bool Shootable
            {
                get { return shootable; }
                set { shootable = value; }
            }

            public bool ShotPassable
            {
                get { return shotPassable; }
                set { shotPassable = value; }
            }

        #endregion 

        public void Initialize(float X, float Y, float W, float H, bool shootable, bool passable, bool shotpassable)
        {
            base.Initialize(X, Y, W, H);
            this.Alive = true;
            this.shootable = shootable;
            this.passable = passable;
            this.shotPassable = shotpassable;
            state = 1;
        }

        public void Explode()
        {    
            this.UnloadGraphicsContent();
        }

    }
}
