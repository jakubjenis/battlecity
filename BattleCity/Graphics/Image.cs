using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BattleCity.Graphics
{
    public class Image
    {
        #region Properties (PositionX, PositionY, Effects, OriginX, OriginY)

        public float PositionX
        {
            get { return positionX; }
            set { positionX = value; }
        }

        public float PositionY
        {
            get { return positionY; }
            set { positionY = value; }
        }

        public SpriteEffects Effects
        {
            get { return effects; }
            set { effects = value; }
        }
        public float OriginX
        {
            get { return origin.X; }
        }
        public float OriginY
        {
            get { return origin.Y; }
        }
        public Vector2 Size
        {
            get { return size; }
        }
        public Texture2D Textura
        {
            get { return texture; }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(PositionX - OriginX);
                int top = (int)Math.Round(PositionY - OriginY);

                return new Rectangle(left, top, (int)this.size.X, (int)this.size.Y);
            }
        }
        
        #endregion

        #region Field (spriteBatch, texture, position, positionX, positionY, source, color, rotation, origin, scale, effects, layer, size)

        private SpriteBatch spriteBatch; //spriteBatch, potrebny pre vykreslenie textury
        protected Texture2D texture; //samotna textura
        private Vector2 position; //pozicia stredu textury
        private float positionX; 
        private float positionY;
        protected Rectangle source; //pokial ide o viacframovu texturu, urcuje, ktora cast sa ma pouzit
        private Color color; //tint textury
        private float rotation; //uhol rotacie
        protected Vector2 origin; //suradnice stredu textury
        private Vector2 scale; //pomer zvacsenia textury
        private SpriteEffects effects; //efetky, prekopenie podla horizontalnej alebo vertikalnej osy
        private float layer; //vrstva, na ktorej sa textura ma vykreslit, urcuje poradie obrazkov na ploche
        protected Vector2 size; //velkost textury

        #endregion

        #region Main Methods (LoadGraphicsContent, Initialize, Draw)  

        // priradi objektu image texturu a spriteBatch, nutne ak chcem volat metodu Draw
        virtual public void LoadGraphicsContent(SpriteBatch spriteBatch, Texture2D texture)
        {
            this.spriteBatch = spriteBatch;
            this.texture = texture;
            this.CalculateSource(1);
            
        }

        //nastavi hodnotu vsetkych okrem textury a spriteBatch, ktore je mozne nastavit az po nacitani grafiky
        virtual public void Initialize(float X, float Y, float W, float H)
        {           
            this.positionX = X;
            this.positionY = Y;                       
            this.color = Color.White;
            this.rotation = 0f;            
            this.scale = Vector2.One;
            this.effects = SpriteEffects.None;
            this.layer = 0.5f;
            this.size = new Vector2(W, H);
            this.origin = new Vector2(size.X / 2, size.Y / 2);
            this.position = new Vector2(X,Y);             
        }

        //vykreslenie textury 
        public void Draw(GameTime gameTime)
        {
            this.position = new Vector2(positionX, positionY);
            this.spriteBatch.Draw(this.texture, this.position, this.source, this.color, this.rotation, this.origin, this.scale, this.effects, this.layer);
        }

        virtual public void UnloadGraphicsContent()
        {
            this.texture = null;
            this.spriteBatch = null;
        }

        #endregion

        //metoda, ktora pri viac framemovej texture urci source, tj obdlznik, 
        //ktory urci, ktora cast textury sa ma vykreslit. Pri animaciach opakovane 
        //volam tuto proceduru, stale s inym indexom.
        public Rectangle CalculateSource(int index)
        {
            if (index <= 0)
            {
                this.source= new Rectangle(0, 0, (int)this.texture.Width, (int)this.texture.Height);
            }
            else
            {                
                int frameRow = 1;
                int frameColumn = index;
                int frameWidth = (int)this.size.X;
                int frameHeight = (int)this.size.Y;
                int totalColumns = this.texture.Width / frameWidth;
                int totalRows = this.texture.Height / frameHeight;

                while (frameColumn > totalColumns)
                {
                    frameRow++;
                    frameColumn -= totalColumns;
                }

                if (index > totalRows * totalColumns)
                {
                    frameColumn = totalColumns;
                    frameRow = totalRows;
                }

                this.source= new Rectangle(
                    (frameColumn - 1) * frameWidth,
                    (frameRow - 1) * frameHeight,
                    frameWidth,
                    frameHeight);
            }
            return this.source;
        }
       
    }
}
