using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BattleCity;
using BattleCity.Graphics;

namespace BattleCity.Graphics
{
    public class Animation : Image
    {
        #region Properties (IsStarted)

        public bool IsStarted
        {
            get { return isStarted; }
            set { isStarted = value; }
        }
        public bool Removable
        {
            get { return removable; }
        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        #endregion

        #region Fields (frames, isLooped, isStarted, currentFrame, framesPerSecond)

        private List<Rectangle> frames;
        private bool isLooped;
        private bool isStarted;
        private float currentFrame;
        private float framesPerSecond;
        bool removable;
        int firstFrame;
        int lastFrame;
        int type;

        #endregion

        #region Main Method (Constructor, Initialize, Update)

        public Animation()
        {
            this.frames = new List<Rectangle>();         
        }
        public void Initialize(float X, float Y, float W, float H, int framesPerSecond, bool isLooped, bool isStarted, int totalFrames)
        {            
            base.Initialize(X, Y, W, H);
            this.framesPerSecond = framesPerSecond;
            this.currentFrame = 0f;
            this.isLooped = isLooped;
            this.isStarted = isStarted;
            this.origin = new Vector2(this.size.X / 2, this.size.Y / 2);

            firstFrame = 1;
            removable = false;
            lastFrame = totalFrames;       
           
        }
        public override void LoadGraphicsContent(SpriteBatch spriteBatch, Texture2D texture)
        {
            base.LoadGraphicsContent(spriteBatch, texture);
            CalculateAnimation();

            for (int index = firstFrame; index <= lastFrame; index++)
            {
                this.frames.Add(CalculateSource(index));
            }
            CalculateSource(1);
        }

        public void CalculateAnimation()
        {
           
        }
              

        public void Update(GameTime gameTime)
        {
            if (this.isStarted)
            {
                this.currentFrame += this.framesPerSecond * (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (this.isLooped)
                {
                    this.currentFrame %= this.frames.Count;
                }
                else if (this.currentFrame > this.frames.Count)
                {
                    this.currentFrame = this.frames.Count-1;
                    this.isStarted = false;
                }
            }
            else
            {
                if (this.currentFrame >= this.frames.Count-1)
                {
                    removable = true;
                }
            }
           
            this.source = frames[(int)this.currentFrame];
        }

        #endregion
    }
}
