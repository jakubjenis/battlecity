using System;
using System.Collections.Generic;

namespace BattleCity.Input
{
    //Trieda starajuca sa o nastavenie konstant potrebnych pri hre
    public class Constants
    {
        public int maxStriel; //kolko striel moze hrac naraz vystrelit
        public int[] BulletsSpeed; // primarna rychlost strely hraca + jednotlivych typov nepriatelov
        public int[] TanksSpeed; // primarna rychlost tanku hraca + jednotlivych typov nepriatelov 
        public int TankSizePlayer; // velkost textury tanku
        public int TankPositionPlayer1X; // Xpozicia kde sa rodi player1
        public int TankPositionPlayer1Y; // Ypozicia kde sa rodi player1
        public int TankPositionPlayer2X; // Xpozicia kde sa rodi player2
        public int TankPositionPlayer2Y; // Ypozicia kde sa rodi player2
        public int BrickSize; // veklost textury tehly
        public int LevelWidth; // sirka levelu
        public int LevelHeigth; //vyska levelu
        public int ResolutionX; // rozlisenie okna
        public int ResolutionY;
        public int OriginX, OriginY; //suradnice laveho okraja hratelneho pola
        public int TotalLevels; //pocet vytvorenych levelov
        public int[] EnemyResistance; //pocet striel potrebnych na to, aby bol nepriatel smrtelny... resistance=3 znamena
        // ze treba 4 strely na jeho zabitie

        public Constants()
        {
            maxStriel = 1;
            BulletsSpeed =new int[5]{7,6,8,8,12};
            EnemyResistance = new int[5] {-1, 0, 0, 3, 1 };
            BrickSize = 24;
            TankSizePlayer = 48;
            LevelHeigth = 624;
            LevelWidth = 624;
            ResolutionX=  872;
            ResolutionY = 672;
            OriginX = 24;
            OriginY = 24;
            TankPositionPlayer1X = BrickSize * 10 - TankSizePlayer/2+OriginX;
            TankPositionPlayer1Y = BrickSize * 26- TankSizePlayer/2+OriginY;
            TankPositionPlayer2X = BrickSize * 18 - TankSizePlayer/2+OriginX;
            TankPositionPlayer2Y = BrickSize * 26 - TankSizePlayer / 2+OriginY;
            TotalLevels = 15;
            TanksSpeed = new int[5]{2,2,3,1,3};
        }
    }
}
