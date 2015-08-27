using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BattleCity.Graphics;
using BattleCity.Input;
//level 14*13 tehliciek


namespace BattleCity
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Engine : Microsoft.Xna.Framework.Game
    {
        #region Fields ( graphics, spriteBatch, myTexture, bulletTexture, player1, player2, strela, strely, myKeyboard )

        Resources Res;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Brick brick, Eagle;
        public Rectangle viewportRect;
        Menu menu;

        Animation vybuch, reward, enter; //NN

        //CONTEXT
        public bool rewardOnScene;
        public bool won;
        Switcher switcher;
        Planner planner;
        public bool GameOver;
        int enemiesToKill;
        int enemiesToBeBorn;
        bool keyboardBlocked;
        public int CurrentLevel;
        Random rnd= new Random();
        int scoreBoard;

        //CONST
        public const int IntervalEnemy = 2000;
        public const int IntervalPlayer = 2000;
        public const int milisecondsPerFrame = 7;
        public const int animationFrameRate = 15;


        int timeSinceLastFrame;
        public int numberOfPlayers;

        public int EnemiesToBeBorn
        {
            get { return enemiesToBeBorn; }
            set { enemiesToBeBorn = value; }
        }
        public int EnemiesToKill
        {
            get { return enemiesToKill; }
        }

        public enum EventTypes
        {
            newEnemy, newRevard
        }

        public bool KeyboardBlocked
        { 
            get { return keyboardBlocked; }
            set { keyboardBlocked = value; }
        }

        public int GameState;

        Tank player1, player2;

        Strela strela;
        List<Strela> Strely = new List<Strela>();
        List<Brick> Bricks= new List<Brick>();
        public List<Animation> Animations = new List<Animation>();
        public List<Animation> Rewards= new List<Animation>();
        List<Tank> Players= new List<Tank>();
        public List<EnemyTank> Enemies= new List<EnemyTank>();
        List<List<int>> LevelLayout;        
        
        public int[] EnemyTypes;

        SpriteFont TitleFont;
        SpriteFont LevelFont;

        Klavesnica myKeyboard, myKeyboard2;
        KeyboardState previousState;
        public Constants Constants;

        #endregion

        #region Properties ( Player1, Player2 )

        public Tank Player1
        {
            get { return player1; }
            set { player1 = value; }
        }

        public Tank Player2
        {
            get { return player2; }
            set { player2 = value; }
        }

        public SpriteBatch Sprite
        {
            get { return spriteBatch; }
        }

        #endregion

        #region MainMethods ( Constructor, Initialize, LoadContent, Update, UnloadContent, Draw )

        public Engine()
        {
            Res = new Resources(Content);

            this.Constants = new Constants();
            numberOfPlayers = 0;
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";
            
            LevelLayout = new List<List<int>>();

            viewportRect = new Rectangle(0, 0, Constants.ResolutionX, Constants.ResolutionY);

            //ToDo: resolve
            enter = new Animation();
            menu = new Menu(this, enter, Res);
            switcher = new Switcher(this,this);
            planner = new Planner(this,this);

            //FIXME: naco?????
            Components.Add(switcher);
            Components.Add(planner);

            keyboardBlocked = false;
            this.Player1 = new Tank();
            this.Player2 = new Tank();         
           
            Players.Add(Player1);
            Players.Add(Player2);
            this.myKeyboard = new Klavesnica(Keys.Space, Keys.Up, Keys.Down, Keys.Left, Keys.Right, Player1);
            this.myKeyboard2 = new Klavesnica(Keys.Q, Keys.W, Keys.S, Keys.A, Keys.D, Player2);


            graphics.PreferredBackBufferWidth = Constants.ResolutionX;
            graphics.PreferredBackBufferHeight = Constants.ResolutionY;

        }

        protected override void Initialize()
        {
            GameState = 0; CurrentLevel = 0;          
            this.Player1.Initialize(Constants.TankPositionPlayer1X, Constants.TankPositionPlayer1Y, Constants.TankSizePlayer, Constants.TankSizePlayer, Constants.TanksSpeed[0], true, 3, 1, Constants.BulletsSpeed[0]);
            this.Player2.Initialize(Constants.TankPositionPlayer2X, Constants.TankPositionPlayer2Y, Constants.TankSizePlayer, Constants.TankSizePlayer, Constants.TanksSpeed[0], true, 3, 1, Constants.BulletsSpeed[0]);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Res.LoadTextures();

            this.Player1.LoadGraphicsContent(spriteBatch, Res.Textures["Player1"]);
            this.Player2.LoadGraphicsContent(spriteBatch, Res.Textures["Player2"]);
            TitleFont = Content.Load<SpriteFont>(".\\Font\\MenuTitle");
            LevelFont = Content.Load<SpriteFont>(".\\Font\\NewLevel");

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Content.Load<Song>(".\\loop"));
            MediaPlayer.Pause();
            

            //ToDo: enter animace
            //enter.Initialize(420, 575,279, 40, 10, true, true, 5);
            //enter.LoadGraphicsContent(this.spriteBatch, enterTexture);
            
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {            
            switcher.CheckStates();           

            if (GameState == 0) //menu
            {
                menu.Update(gameTime);
                CurrentLevel = 1;
                scoreBoard = 0;
                won = false;
            }
            else if (GameState == 6) //howtoplay
            {                
                if ((Keyboard.GetState().IsKeyDown(Keys.Escape)) || (Keyboard.GetState().IsKeyDown(Keys.Back)))
                {
                    GameState = 0;
                }
            }
            if ((GameState == 2)&&(!GameOver))
            {
                if ((enemiesToKill == 0)&&(!GameOver)) //prechod do dalsieho levelu
                {
                    if (!switcher.switching)
                    {
                        CurrentLevel += 1;
                        if (CurrentLevel > Constants.TotalLevels)
                        {
                            switcher.SwitchState(4, 0);
                            won = true;
                        }
                        else
                        {
                            switcher.SwitchState(3, 3000);
                        }  
                    }                      
                }

                if((scoreBoard/10>0)&&(!rewardOnScene))                
                //if ((!rewardOnScene)&&((Player1.Score+player2.Score) % 1600 == 0)&&(Player1.Score+player2.Score>=0))
                {
                    scoreBoard -= rnd.Next(8)+4;
                    int x=rnd.Next(Constants.LevelWidth-Constants.BrickSize/2)+Constants.OriginX+Constants.BrickSize;
                    int y=rnd.Next(Constants.LevelHeigth-Constants.BrickSize*2)+Constants.OriginY+Constants.BrickSize;

                    reward = new Animation();
                    reward.Initialize(x, y, 40, 40, 20, true, true, 7);
                    reward.Type=rnd.Next(4);
                    reward.LoadGraphicsContent(spriteBatch, Res.Textures["Reward_" + reward.Type]);
                    Animations.Add(reward);
                    Rewards.Add(reward);
                    planner.PlanEvent("cancelReward", rnd.Next(3000) + 6000, 0);
                    rewardOnScene = true;
                }
            }


            for (int i = 0; i < Animations.Count;i++ )
            {
                Animations[i].Update(gameTime);
                if (Animations[i].Removable)
                {
                    Animations.RemoveAt(i);
                }
            }


            if (GameState== 2) //hraj
            {
                timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds/2;
                if (timeSinceLastFrame > milisecondsPerFrame)
                {
                    timeSinceLastFrame = 0;

                    myKeyboard.Update(this, gameTime);
                    myKeyboard2.Update(this, gameTime);                   

                    //nastavi x,y prirastok k suradniciam tanku podla daneho smeru      
                    foreach (Tank Player in Players)
                    {
                        if (Player.Alive)
                        {
                            Player.CalculateXY(Constants.TanksSpeed[Player.Type]);
                        }
                    }

                    if ((Player1.Lives == 0) && (Player2.Lives == 0))
                    {
                        GameOver = true;
                    }

                    #region CheckRewardsCollision

                    foreach (Tank Player in Players)
                    {
                        if (Player.Alive)
                        {
                            Animation temp;
                            for (int i=0; i < Rewards.Count; i++)
                            {
                                temp = Rewards[i];
                                if (Player.BoundingRectangle.Intersects(temp.BoundingRectangle))
                                {
                                    Player.Score += 400;                                    
                                    for (int j = 0; j < Animations.Count; j++)
                                    {
                                        if (Rewards[i] == Animations[j])
                                        {
                                            Animations.RemoveAt(j);
                                        }
                                    }
                                    Rewards.RemoveAt(i);
                                    rewardOnScene = false;
                                    switch (temp.Type)
                                    {
                                        case 0: Player.Lives += 1; break;
                                        case 1: if (Player.MaxShots < 3)
                                            {
                                                Player.MaxShots += 1;
                                            }
                                        break;
                                        case 2: if (Player.ShotSpeed <= 12) { Player.ShotSpeed += 2; } break;

                                    }
                                }
                            }
                        }
                    }

                    #endregion

                    //skontroluje, ci hrac narazi v danom smere do nejakej tehly, ak ano nastavi Collision na true
                    #region CheckCollisionTank

                    Player1.Collision = false;
                    Player2.Collision = false;
                    if ((player2.Alive)&&(player1.NextFrameBound().Intersects(player2.BoundingRectangle)))
                    {
                        player1.Collision = true;
                    }
                    if ((player1.Alive)&&(player2.NextFrameBound().Intersects(player1.BoundingRectangle)))
                    {
                        player2.Collision = true;
                    }

                    if ((player1.BoundingRectangle.Intersects(player2.BoundingRectangle)))
                    {
                        player1.Collision = false;
                        player2.Collision = false;
                    }

                    foreach (Tank Player in Players)
                    {
                        if (Player.Alive)
                        {
                            for (int j = 0; j < Bricks.Count; j++)
                            {
                                brick = Bricks[j];
                                if (!brick.Alive)
                                {
                                    Bricks.RemoveAt(j--);
                                }
                                if ((!brick.Passable) && (brick.BoundingRectangle.Intersects(Player.NextFrameBound())))
                                {
                                    Player.Collision = true;

                                }
                            }

                            if ((Player.NextFrameBound().Top < Constants.OriginY) || (Player.NextFrameBound().Bottom > Constants.LevelHeigth + Constants.OriginY)
                                || (Player.NextFrameBound().Left < Constants.OriginX) || (Player.NextFrameBound().Right > Constants.LevelWidth + Constants.OriginX)
                                )
                            {
                                Player.Collision = true;

                            }

                            for (int j = 0; j < Enemies.Count; j++)
                            {
                                if (Player.NextFrameBound().Intersects(Enemies[j].BoundingRectangle))
                                {
                                    Player.Collision = true;
                                }
                                if (Player.BoundingRectangle.Intersects(Enemies[j].BoundingRectangle))
                                {
                                    Player.Collision = false;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < Enemies.Count; i++)
                    {
                        EnemyTank tempEnemy = Enemies[i];
                        if (tempEnemy.Alive)
                        {
                            for (int j = 0; j < Bricks.Count; j++)
                            {
                                brick = Bricks[j];
                                if (!brick.Alive)
                                {
                                    Bricks.RemoveAt(j--);
                                }
                                if ((!brick.Passable) && (brick.BoundingRectangle.Intersects(tempEnemy.NextFrameBound())))
                                {
                                    tempEnemy.Collision = true;
                                }
                            }

                            if ((tempEnemy.NextFrameBound().Top < Constants.OriginY) || (tempEnemy.NextFrameBound().Bottom > Constants.LevelHeigth + Constants.OriginY)
                                || (tempEnemy.NextFrameBound().Left < Constants.OriginX) || (tempEnemy.NextFrameBound().Right > Constants.LevelWidth + Constants.OriginX)
                                )
                            {
                                tempEnemy.Collision = true;

                            }

                            for (int j = 0; j < Enemies.Count; j++)
                            {
                                if ((tempEnemy.BoundingRectangleTop.Intersects(Enemies[j].BoundingRectangle)) && (j != i))
                                {
                                    tempEnemy.Otoc();
                                }
                            }

                            foreach (Tank Player in Players)
                            {
                                if (Player.Alive)
                                {
                                    if ((tempEnemy.NextFrameBound().Intersects(Player.BoundingRectangle)))
                                    {
                                        tempEnemy.Collision = true;
                                    }
                                }
                            }

                        }

                    }

                    #endregion

                    //ak je Collision true, nepusti ho v danom smere

                    #region MoveTanks

                    if (Player1.Alive)
                    {
                        if ((myKeyboard.KeyPressed) && (!Player1.Collision))
                        {
                            Player1.PositionX += Player1.X;
                            Player1.PositionY += Player1.Y;
                        }
                    }

                    if (Player2.Alive)
                    {
                        if ((myKeyboard2.KeyPressed) && (!Player2.Collision))
                        {
                            Player2.PositionX += Player2.X;
                            Player2.PositionY += Player2.Y;
                        }
                    }

                    for (int j = 0; j < Enemies.Count; j++)
                    {
                        if (Enemies[j].Alive)
                        {
                            Enemies[j].UpdateEnemy(gameTime);
                        }
                        else
                        {
                            Enemies.RemoveAt(j);
                        }
                    }
                    #endregion

                    //skontroluje kolizie striel s tehlami, ak nastane, strela.Expode();
                    #region CheckCollisionsShots

                    for (int i = 0; i < Strely.Count; i++)
                    {
                        strela = Strely[i];

                        for (int j = 0; j < Strely.Count; j++)
                        {
                            if ((i != j))
                            {
                                if (strela.CheckCollisionsShot(Strely[j]))
                                {
                                    Vybuch((int)strela.PositionX, (int)strela.PositionY);
                                }
                            }
                        }

                        if (strela.Alive)
                        {
                            if ((strela.BoundingRectangle.Intersects(Eagle.BoundingRectangleBrick)) && (strela.Alive))
                            {
                                Eagle.CalculateSource(2);
                                GameOver = true;
                                strela.Alive = false;
                            }

                            for (int j = 0; j < Bricks.Count; j++)
                            {
                                brick = Bricks[j];
                                if (!brick.ShotPassable)
                                {
                                    if(strela.CheckCollisions(brick))
                                    {
                                        Vybuch((int)strela.PositionX,(int)strela.PositionY);
                                    }
                                }
                            }

                            EnemyTank temp;

                            if ((strela.ShotBy == player1) || (strela.ShotBy == player2))
                            {
                                for (int j = 0; j < Enemies.Count; j++)
                                {
                                    temp = Enemies[j];
                                    if (temp.Alive)
                                    {
                                        if (strela.CheckCollisionsTank(temp))
                                        {
                                            int value=0;
                                            switch(temp.Type)
                                            {
                                                case 1: value=100; break;
                                                case 2: value=200; break;
                                                case 3: value=400; break;                                                                       
                                            }

                                            enemiesToKill -= 1;
                                            temp.Explode();
                                            strela.ShotBy.Score += value;
                                            if (!rewardOnScene)
                                            {
                                                scoreBoard += 1;
                                            }
                                            if (planner.CountEvent("newEnemy") < 1)
                                            {
                                                planner.PlanEvent("enemyBorn", 1300, 1);
                                                planner.PlanEvent("newEnemy", IntervalEnemy, 1);                                                
                                            }
                                            for (int k = 0; k < Strely.Count; k++)
                                            {
                                                if((Strely[k].ShotBy==temp)&&(strela.BoundingRectangle.Intersects(temp.BoundingRectangle)))
                                                {
                                                    Strely[k].Alive=false;
                                                }

                                            }
                                            VybuchTank((int)temp.PositionX, (int)temp.PositionY);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j <= 1; j++)
                                {
                                    if (strela.CheckCollisionsTank(Players[j]))
                                    {
                                        if (!Players[j].Indestructable)
                                        {
                                            Players[j].Lives -= 1;
                                            Players[j].Alive = false;
                                            if (Players[j].Lives > 0)
                                            {
                                                planner.PlanEvent("bornPlayer", IntervalPlayer, j + 1);
                                                planner.PlanEvent("enemyBorn", 1300, j + 2);
                                            }
                                            VybuchTank((int)Players[j].PositionX, (int)Players[j].PositionY);
                                        }
                                        else
                                        {
                                            Vybuch((int)strela.PositionX, (int)strela.PositionY);
                                        }
                                        strela.Explode();
                                        Strely.RemoveAt(i);   
                                    }
                                }
                            }                            

                            if ((strela.PositionY < Constants.OriginY + strela.Size.Y / 2) || (strela.PositionY > Constants.LevelHeigth + Constants.OriginY - strela.Size.Y / 2) || (strela.PositionX < Constants.OriginX + strela.Size.X / 2) || (strela.PositionX > Constants.LevelWidth + Constants.OriginX - strela.Size.X / 2))
                            {
                                strela.Alive = false;
                                Vybuch((int)strela.PositionX, (int)strela.PositionY);
                            }

                            strela.Update();

                        }
                        else
                        {
                            strela.Explode();
                            Strely.RemoveAt(i--);
                        }
                    }
                }

                previousState = Keyboard.GetState();
                #endregion                
            }

            previousState = Keyboard.GetState();
            base.Update(gameTime);
        }
              
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            if (GameState == 0)
            {                
                menu.Draw(spriteBatch, gameTime);
            }
            else if (GameState == 6)
            {
                spriteBatch.Draw(Res.Textures["HotToPlay"], Vector2.One, Color.White);
            }
            else if ((GameState == 2) || (GameState == 10))
            {
                spriteBatch.Draw(Res.Textures["BeforeLevel_Background1"], viewportRect, Color.White);

                if (enemiesToBeBorn > 10)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        spriteBatch.Draw(Res.Textures["Enemy_Thumbnail"], new Vector2(725, 25 + i * 27), Color.White);
                    }
                    for (int i = 0; i < enemiesToBeBorn - 10; i++)
                    {
                        spriteBatch.Draw(Res.Textures["Enemy_Thumbnail"], new Vector2(775, 25 + i * 27), Color.White);
                    }
                }
                else
                {
                    for (int i = 0; i < enemiesToBeBorn; i++)
                    {
                        spriteBatch.Draw(Res.Textures["Enemy_Thumbnail"], new Vector2(725, 25 + i * 27), Color.White);
                    }
                }

                foreach (Tank Player in Players)
                {
                    if (Player.Alive)
                    {
                        Player.Draw(gameTime);
                    }
                }

                foreach (EnemyTank Enemy in Enemies)
                {
                    if (Enemy.Alive)
                    {
                        Enemy.Draw(gameTime);
                    }
                }

                for (int i = 0; i < Bricks.Count; i++)
                {
                    brick = Bricks[i];
                    if ((brick.Alive) && (brick.Textura != Res.Textures["Brick_3"]))
                    {
                        brick.Draw(gameTime);
                    }
                }

                foreach (Strela strela in Strely)
                {
                    if (strela.Alive)
                    {
                        strela.Draw(gameTime);
                    }
                }

                for (int i = 0; i < Bricks.Count; i++)
                {
                    brick = Bricks[i];
                    if ((brick.Alive) && (brick.Textura == Res.Textures["Brick_3"]))
                    {
                        brick.Draw(gameTime);
                    }
                }

                foreach (Animation temp in Animations)
                {
                    temp.Draw(gameTime);
                }

                spriteBatch.DrawString(TitleFont, CurrentLevel.ToString(), new Vector2(805, 330), new Color(255, 222, 0));
                spriteBatch.DrawString(TitleFont, enemiesToKill.ToString(), new Vector2(795, 410), new Color(255, 222, 0));
                spriteBatch.DrawString(TitleFont, Player1.Lives.ToString(), new Vector2(805, 493), new Color(255, 222, 0));
                spriteBatch.DrawString(TitleFont, Player2.Lives.ToString(), new Vector2(805, 583), new Color(255, 222, 0));

            }
            else if (GameState == 3)
            {
                if (numberOfPlayers == 2)
                {
                    spriteBatch.Draw(Res.Textures["BeforeLevel_Background2"], new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(LevelFont, (Player2.Score).ToString(), new Vector2(610, 380), new Color(255, 222, 0));
                }
                else
                {
                    spriteBatch.Draw(Res.Textures["BeforeLevel_Background1"], new Vector2(0, 0), Color.White);
                }
                spriteBatch.DrawString(LevelFont, (Player1.Score).ToString(), new Vector2(610, 330), new Color(255, 222, 0));
                spriteBatch.DrawString(LevelFont, CurrentLevel.ToString(), new Vector2(350, 285), new Color(255, 222, 0));
            }
            else if (GameState == 4)
            {
                if (numberOfPlayers == 2)
                {
                    spriteBatch.Draw(Res.Textures["GameOver_2"], new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(TitleFont, Player1.Score.ToString(), new Vector2(340, 350), new Color(255, 222, 0));
                    spriteBatch.DrawString(TitleFont, Player2.Score.ToString(), new Vector2(340, 400), new Color(255, 222, 0));
                    if (player1.Score >= player2.Score)
                    {
                        spriteBatch.DrawString(TitleFont, "1", new Vector2(420, 480), new Color(255, 222, 0));
                    }
                    else
                    {
                        spriteBatch.DrawString(TitleFont, "2", new Vector2(420, 480), new Color(255, 222, 0));
                    }
                }
                else
                {
                    spriteBatch.Draw(Res.Textures["GameOver_1"], new Vector2(0, 0), Color.White);
                    spriteBatch.DrawString(TitleFont, Player1.Score.ToString(), new Vector2(340, 350), new Color(255, 222, 0));
                }
                if (won)
                {
                    spriteBatch.Draw(Res.Textures["Contratulations"], new Vector2(0, 0), Color.White);
                }

            }

            if (GameOver)
            {
                spriteBatch.Draw(Res.Textures["GameOver_Mini"], new Vector2(150, 200), Color.White);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }

        #endregion

        void Vybuch(int x,int y)
        {
            vybuch = new Animation();

            vybuch.Initialize(x, y, 90, 90, animationFrameRate, false, true, 7);
            vybuch.LoadGraphicsContent(this.spriteBatch, Res.Textures["Vybuch_Maly"]);

            Animations.Add(vybuch);
        }


        void VybuchTank(int x, int y)
        {
          
        }

        public void NewTankAnim(int x, int y)
        {
            vybuch = new Animation();

            vybuch.Initialize(x, y, 90, 90, 8, false, true, 7);
            vybuch.LoadGraphicsContent(this.spriteBatch, Res.Textures["Vybuch_Narodenie"]);

            Animations.Add(vybuch);
        }

        public void Fire(Tank Player)
        {
            strela = new Strela();

            Strely.Add(strela);            
            if ((Player == player1) || (Player == player2))
            {
                strela.Initialize(Player, Player.ShotSpeed, Player.Smer);
            }
            else
            {
                strela.Initialize(Player, (int)(Constants.BulletsSpeed[Player.Type]), Player.Smer);
            }
            strela.LoadGraphicsContent(spriteBatch, Res.Textures["Bullet"]);

            Player.BulletsShot += 1;
        }

        public EnemyTank CreateEnemy(int x,int y,int i)
        {
            EnemyTank temp;
            temp = new EnemyTank(Constants.TanksSpeed[i], Bricks, this);
            temp.Initialize(x, y, Constants.TankSizePlayer, Constants.TankSizePlayer);

            Random rnd = new Random();
            temp.Smer = rnd.Next(4)+1;
            temp.Resistance = Constants.EnemyResistance[i];
            temp.LoadGraphicsContent(spriteBatch, Res.Textures["Enemy_" + i]);
            temp.Type = i;
            temp.CalculateSource(5);

            return temp;
        }

        
        //ToDo: rewrite
        void drawLevel(string fileName)
        {
            using(StreamReader reader = new StreamReader(fileName))
            {
                LevelLayout.Clear();
                bool readLevel = false;
                bool readTanks = false;

                while (!reader.EndOfStream)
                { 
                    string line = reader.ReadLine().Trim();
                    if (line == "<levelLayout>")
                    {
                        readLevel = true;
                        readTanks = false;
                    }
                    else if (line == "<tankLayout>")
                    {
                        readLevel = false;
                        readTanks = true;
                    } else if (readLevel)
                    {
                        List<int> row = new List<int>();
                        string[] cells = line.Split(' ');

                        foreach (string c in cells)
                        {
                            row.Add(int.Parse(c));
                        }

                        LevelLayout.Add(row);
                    }
                    else if (readTanks)
                    {
                        string[] cells = line.Split(' ');
                        EnemyTypes = new int[cells.Length];

                        for (int i = 0; i < cells.Length; i++)
                        {
                            EnemyTypes[i] = int.Parse(cells[i]);
                        }
                        //EnemyTypes = int.Parse(line.Split(' '));
                    }

                }
                reader.Close();

                LevelLayout[0][0] = 0;
                LevelLayout[0][6] = 0;
                LevelLayout[0][12] = 0;
                LevelLayout[12][4] = 0;
                LevelLayout[12][5] = 21;               
                LevelLayout[12][6] = 0;
                LevelLayout[12][7] = 31;
                LevelLayout[12][8] = 0;
                LevelLayout[11][5] = 41;      
                LevelLayout[11][6] = 51;
                LevelLayout[11][7] = 61;
            }

            for (int j=0; j<LevelLayout.Count; j++)
            {
                List<int> row = LevelLayout[j];
                for (int i = 0; i < row.Count; i++)
                {
                    if ((row[i]%10) == 1)
                    {
                        if ((row[i] / 10 == 3)||(row[i]/10==0))
                        {
                            this.brick = new Brick();
                            this.brick.Initialize(2 * i * Constants.BrickSize + Constants.BrickSize / 2 + Constants.OriginX,
                                j * Constants.BrickSize * 2 + Constants.BrickSize / 2 + Constants.OriginY,
                                Constants.BrickSize, Constants.BrickSize,
                                true, false, false);
                            this.brick.LoadGraphicsContent(spriteBatch, Res.Textures["Brick_" + 0]);
                            Bricks.Add(this.brick);
                        }
                        if ((row[i] / 10 == 2)||(row[i]/10==0))
                        {
                        this.brick = new Brick();
                        this.brick.Initialize(i * Constants.BrickSize * 2 + Constants.BrickSize + Constants.BrickSize / 2 + Constants.OriginX,
                            j * Constants.BrickSize * 2 + Constants.BrickSize / 2 + Constants.OriginY, 
                            Constants.BrickSize, Constants.BrickSize,
                            true, false, false);
                        this.brick.LoadGraphicsContent(spriteBatch, Res.Textures["Brick_" + 0]);
                        Bricks.Add(this.brick);
                        }
                        this.brick = new Brick();
                        if ((row[i] / 10 == 5) || (row[i] / 10 == 3) || (row[i] / 10 == 0) || (row[i] / 10 == 6))
                        {
                            this.brick.Initialize(i * Constants.BrickSize * 2 + Constants.BrickSize / 2 + Constants.OriginX,
                                j * Constants.BrickSize * 2 + Constants.BrickSize + Constants.BrickSize / 2 + Constants.OriginY,
                                Constants.BrickSize, Constants.BrickSize,
                                true, false, false);
                            this.brick.LoadGraphicsContent(spriteBatch, Res.Textures["Brick_" + 0]);
                            Bricks.Add(this.brick);
                        }
                        if ((row[i] / 10 == 2)||(row[i]/10==4) || (row[i] / 10 == 0)||(row[i]/10==5))
                        {
                            this.brick = new Brick();
                            this.brick.Initialize(i * Constants.BrickSize * 2 + Constants.BrickSize + Constants.BrickSize / 2 + Constants.OriginX,
                                j * Constants.BrickSize * 2 + Constants.BrickSize + Constants.BrickSize / 2 + Constants.OriginY,
                                Constants.BrickSize,
                                Constants.BrickSize, true, false, false);
                            this.brick.LoadGraphicsContent(spriteBatch, Res.Textures["Brick_" + 0]);
                            Bricks.Add(this.brick);
                        }

                    }
                    else if (row[i] == 2)
                    {
                        this.brick = new Brick();
                        this.brick.Initialize(2 * i * Constants.BrickSize + Constants.BrickSize + Constants.OriginX,
                            j * Constants.BrickSize * 2 + Constants.BrickSize + Constants.OriginY,
                            Constants.BrickSize * 2,
                            Constants.BrickSize * 2, 
                            false, false, false);
                        this.brick.LoadGraphicsContent(spriteBatch,  Res.Textures["Brick_" + (row[i]-1)]);
                        Bricks.Add(this.brick);
                    }
                    else if (row[i] == 3)
                    {
                        this.brick = new Brick();
                        this.brick.Initialize(2 * i * Constants.BrickSize + Constants.BrickSize + Constants.OriginX,
                            j * Constants.BrickSize * 2 + Constants.BrickSize + Constants.OriginY,
                            Constants.BrickSize * 2,
                            Constants.BrickSize * 2, 
                            false, false, true);
                        this.brick.LoadGraphicsContent(spriteBatch, Res.Textures["Brick_" + (row[i] - 1)]);
                        Bricks.Add(this.brick);
                    }
                    else if (row[i] == 4)
                    {
                        this.brick = new Brick();
                        this.brick.Initialize(2 * i * Constants.BrickSize + Constants.BrickSize + Constants.OriginX,
                            j * Constants.BrickSize * 2 + Constants.BrickSize + Constants.OriginY, 
                            Constants.BrickSize * 2,
                            Constants.BrickSize * 2,
                            false, true, true);
                        this.brick.LoadGraphicsContent(spriteBatch, Res.Textures["Brick_" + (row[i] - 1)]);
                        Bricks.Add(this.brick);
                    }
                }                
            }
            Eagle = new Brick();
            Eagle.Initialize(2 * 6 * Constants.BrickSize + Constants.BrickSize + Constants.OriginX,
                12 * Constants.BrickSize * 2 + Constants.BrickSize + Constants.OriginY,
                Constants.BrickSize * 2,
                Constants.BrickSize * 2, 
                false, false, false);
            Eagle.LoadGraphicsContent(spriteBatch, Res.Textures["Brick_Eagle"]);
            Bricks.Add(Eagle);            

        }

        public void newLevel(int level)
        {
            Strely.Clear();
            Bricks.Clear();
            Enemies.Clear();
            planner.Clear();
            rewardOnScene = false;
            Animations.Clear();
            Rewards.Clear();

            GameOver = false;

            enemiesToBeBorn = 20;
            enemiesToKill = enemiesToBeBorn;
            timeSinceLastFrame=0;

            this.player1.PositionX = Constants.TankPositionPlayer1X;
            this.player1.PositionY = Constants.TankPositionPlayer1Y;
            this.player1.Effects = SpriteEffects.None;
            this.player1.CalculateSource(1);
            this.player1.Indestructable = true;
            planner.PlanEvent("makeDestructable", 4000, 1);
            
            Player1.Smer = 1;
            Player1.BulletsShot = 0;

            planner.PlanEvent("newEnemy", 2000, 1);
            planner.PlanEvent("enemyBorn", 1300, 1);

            if (player2.Alive)
            {
                this.player2.PositionX = Constants.TankPositionPlayer2X;
                this.player2.PositionY = Constants.TankPositionPlayer2Y;
                this.player2.Effects = SpriteEffects.None;
                this.player2.CalculateSource(1);
                Player2.Smer = 1;
                Player2.BulletsShot = 0;
                this.player2.Indestructable = true;
                planner.PlanEvent("makeDestructable", 4000, 2);
            }
            else
            {
                this.Player2.Lives = 0;
            }

            keyboardBlocked = false;            
            
            drawLevel(Path.Combine("Content","level"+level.ToString()+".txt"));
        }    
    }    
}
