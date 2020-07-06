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

            do
            {
                RedrawBoards(playerBoard, enemyBoard);
                Console.ReadLine();
                PlayersShoot(playerBoard, enemyBoard);
            } while (!IsGameOver(playerBoard, enemyBoard));

            if (playerBoard.LostAllFleet())
            {
                RedrawBoards(playerBoard, enemyBoard);
                Console.Write("Przegrałeś!");
            }
            else
            {
                RedrawBoards(playerBoard, enemyBoard);
                Console.Write("Wygrałeś!");
            }
            Console.ReadLine();
        }

        public static void RedrawBoards(PlayerBoard playerBoard, EnemyBoard enemyBoard)
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

        public static bool IsGameOver(PlayerBoard playerBoard, EnemyBoard enemyBoard)
        {
            return playerBoard.LostAllFleet() || enemyBoard.LostAllFleet();
        }
    }
}
