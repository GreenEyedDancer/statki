using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static statki.Board;

namespace statki
{
    class Game
    {
        public static void Start()
        {
            PlayerBoard playerBoard = new PlayerBoard();
            EnemyBoard enemyBoard = new EnemyBoard();

            Console.WriteLine("Witaj w grze w statki!");
            playerBoard.CreateFleet();
            enemyBoard.CreateFleet();
            bool gameOver;

            do
            {
                RefreshScreen(playerBoard, enemyBoard);
                Console.ReadLine();
                PlayersShoot(playerBoard, enemyBoard);
                gameOver = CheckIfGameIsOver(playerBoard, enemyBoard);
            } while (!gameOver);

            if (playerBoard.LostAllFleet())
            {
                RefreshScreen(playerBoard, enemyBoard);
                Console.Write("Przegrałeś!");
            }
            else
            {
                RefreshScreen(playerBoard, enemyBoard);
                Console.Write("Wygrałeś!");
            }
            Console.ReadLine();
        }
        public static void RefreshScreen(PlayerBoard playerBoard, EnemyBoard enemyBoard)
        {
            Console.Clear();
            playerBoard.DrawBoard();
            enemyBoard.DrawBoard();
        }

        public static void PlayersShoot(PlayerBoard playerBoard, EnemyBoard enemyBoard)
        {
            playerBoard.EnemyShoots(playerBoard);
            enemyBoard.ShootEnemy(enemyBoard);
        }

        public static bool CheckIfGameIsOver(PlayerBoard playerBoard, EnemyBoard enemyBoard)
        {
            if (playerBoard.LostAllFleet() || enemyBoard.LostAllFleet())
                return true;

            return false;
        }
    }
}
