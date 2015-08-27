using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleCity.Graphics;
using BattleCity.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BattleCity
{
    public class Resources
    {
        private ContentManager Content;

        public List<Animation> Animationns = new List<Animation>();
        public SortedList<String, Texture2D> Textures = new SortedList<String, Texture2D>();


        public Resources(ContentManager C)
        {
            Content = C;
        }

        public void LoadTextures()
        {
            Textures["Player1"] = Content.Load<Texture2D>(".\\Graphics\\Player1");
            Textures["Player2"] = Content.Load<Texture2D>(".\\Graphics\\Player2");
            Textures["Bullet"] = Content.Load<Texture2D>(".\\Graphics\\bullet");
            Textures["Vybuch_Velky"] = Content.Load<Texture2D>(".\\Graphics\\vybuch");
            Textures["Vybuch_Maly"] = Content.Load<Texture2D>(".\\Graphics\\vybuch2");
            Textures["Vybuch_Narodenie"] = Content.Load<Texture2D>(".\\Graphics\\odmena4");
            Textures["Reward_1"] = Content.Load<Texture2D>(".\\Graphics\\odmena1");
            Textures["Reward_2"] = Content.Load<Texture2D>(".\\Graphics\\odmena2");
            Textures["Reward_3"] = Content.Load<Texture2D>(".\\Graphics\\odmena3");
            Textures["Reward_4"] = Content.Load<Texture2D>(".\\Graphics\\odmena4");
            Textures["Enemy_1"] = Content.Load<Texture2D>(".\\Graphics\\Enemy1");
            Textures["Enemy_2"] = Content.Load<Texture2D>(".\\Graphics\\Enemy2");
            Textures["Enemy_3"] = Content.Load<Texture2D>(".\\Graphics\\Enemy3");
            Textures["Brick_0"] = Content.Load<Texture2D>(".\\Graphics\\Brick");
            Textures["Brick_1"] = Content.Load<Texture2D>(".\\Graphics\\Rock");
            Textures["Brick_2"] = Content.Load<Texture2D>(".\\Graphics\\Water");
            Textures["Brick_3"] = Content.Load<Texture2D>(".\\Graphics\\Grass");
            Textures["Brick_Eagle"] = Content.Load<Texture2D>(".\\Graphics\\Eagle");
            Textures["BeforeLevel_Background1"] = Content.Load<Texture2D>(".\\Graphics\\levelTexture1");
            Textures["BeforeLevel_Background2"] = Content.Load<Texture2D>(".\\Graphics\\levelTexture2");
            Textures["Menu_1"] = Content.Load<Texture2D>(".\\Graphics\\Menu1");
            Textures["Menu_2"] = Content.Load<Texture2D>(".\\Graphics\\Menu2");
            Textures["Menu_3"] = Content.Load<Texture2D>(".\\Graphics\\Menu3");
            Textures["Menu_4"] = Content.Load<Texture2D>(".\\Graphics\\Menu4");
            Textures["Menu_Background"] = Content.Load<Texture2D>(".\\Graphics\\MenuPozadie");
            Textures["Menu_Enter"] = Content.Load<Texture2D>(".\\Graphics\\enterTexture");
            Textures["Level_Background"] = Content.Load<Texture2D>(".\\Graphics\\pozadie");
            Textures["GameOver_1"] = Content.Load<Texture2D>(".\\Graphics\\gameOver1");
            Textures["GameOver_2"] = Content.Load<Texture2D>(".\\Graphics\\gameOver2");
            Textures["GameOver_Mini"] = Content.Load<Texture2D>(".\\Graphics\\GameOver");
            Textures["HowToPlay"] = Content.Load<Texture2D>(".\\Graphics\\howtoplay");
            Textures["Enemy_Thumbnail"] = Content.Load<Texture2D>(".\\Graphics\\EnemyThumb");
            Textures["Level_Pozadie"] = Content.Load<Texture2D>(".\\Graphics\\MenuPozadie");
;
        }



        //Texture2D bulletTexture;
        //Texture2D backgroundTexture;
        //Texture2D[] menuCursors;
        //Texture2D vybuchTexture;
        //Texture2D vybuch2Texture;
        //Texture2D bornTexture;
        //Texture2D player1Texture1, player1Texture2, player2Texture1, player2Texture2;
        //Texture2D levelTexture2, levelTexture1, gameOver1, gameOver2, congratTexture;
        //Texture2D howToPlayTexture;
    }
}
