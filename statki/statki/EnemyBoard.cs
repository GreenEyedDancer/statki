using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static statki.DataModels;
using static statki.Ship;

namespace statki
{
    public class EnemyBoard : Board
    {
        public void CreateFleet()
        {
            for (int shipSize = 4; shipSize > 0; shipSize--)
            {
                for (int shipQuantity = 1; shipQuantity < 6 - shipSize; shipQuantity++)
                {
                    AllShips.Add(new Ship(shipSize, this));
                }
            }
        }

        public void PrintCell(CellContent cell)
        {
            switch (cell)
            {
                case CellContent.empty:
                    Console.Write(".");
                    break;
                case CellContent.ship:
                    Console.Write(".");
                    break;
                case CellContent.hitShip:
                    Console.Write("X");
                    break;
                case CellContent.destroyedShip:
                    Console.Write("Z");
                    break;
                case CellContent.missedShot:
                    Console.Write("/");
                    break;
                default:
                    Console.Write("Error, wrong enum in cell!");
                    break;
            }
        }

        public void DrawBoard()
        {
            Console.WriteLine("Plansza Przeciwnika:");
            char RowIndex = 'A';
            Console.WriteLine("  1 2 3 4 5 6 7 8 9 10");
            for (int row = 1; row < BoardSize; row++)
            {
                Console.Write(RowIndex);
                RowIndex++;
                for (int column = 1; column < BoardSize; column++)
                {
                    Console.Write(' ');
                    PrintCell(BoardContent[row, column]);
                }
                Console.WriteLine();
            }
        }

        public void ShootEnemy(Board board)
        {
            bool hit;

            do
            {
                Console.Clear();
                DrawBoard();
                hit = ChooseCellToShoot(board);
                if (hit && this.LostAllFleet())
                    break;
                Console.ReadLine();
            } while (hit);

        }

        public bool ChooseCellToShoot(Board board)
        {
            string coordinate;
            do
            {
                Console.Write("Podaj prawidłową współrzędna do strzału: ");
                coordinate = Console.ReadLine().ToUpper();
            }
            while (CellCordsAreIncorrect(coordinate));

            int coordinateNumber;
            int coordinateLetter = (int)coordinate[0] - 64;
            if (coordinate.Length == 3)
                coordinateNumber = 10;
            else
                coordinateNumber = coordinate[1] - 48;

            return DidShotHit(coordinateLetter, coordinateNumber, board);
        }

        private bool DidShotHit(int coordinateLetter, int coordinateNumber, Board board)
        {
            switch (BoardContent[coordinateLetter, coordinateNumber])
            {
                case CellContent.empty:
                    BoardContent[coordinateLetter, coordinateNumber] = CellContent.missedShot;
                    Console.Clear();
                    DrawBoard();
                    Console.Write("Pudło!");
                    return false;

                case CellContent.ship:
                    if (CheckIfShootSinkTheShip(board, coordinateLetter, coordinateNumber))
                    {
                        Console.Clear();
                        DrawBoard();
                        Console.Write("Trafiony zatopiony! Możesz strzelić jeszcze raz.");
                    }
                    else
                    {
                        Console.Clear();
                        DrawBoard();
                        Console.Write("Trafiony! Możesz strzelić jeszcze raz.");
                    }
                    return true;

                case CellContent.destroyedShip:
                case CellContent.hitShip:
                case CellContent.missedShot:
                    Console.Write("Już tu strzelałeś! Wybierz inną współrzędną.");
                    return true;

                default:
                    Console.Write("Wystapil blad z obiektem na planszy!");
                    return false;
            }
        }
    }
}
