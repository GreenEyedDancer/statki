using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static statki.DataModels;
using static statki.Ship;

namespace statki
{
    public class PlayerBoard : Board
    {
        public void CreateFleet()
        {
            for (int shipSize = 4; shipSize > 0; shipSize--)
            {
                for (int shipQuantity = 1; shipQuantity < 6 - shipSize; shipQuantity++)
                {
                    AskPlayerToPlaceShip(shipSize);
                    Console.Clear();
                    DrawBoard();
                }
            }
        }

        public void AskPlayerToPlaceShip(int shipSize)
        {
            if (shipSize > 1)
            {
                var doubleCoord = DoubleCoord.CreateBothCoords(shipSize, this);
                AllShips.Add(new Ship(doubleCoord, this));
            }
            else
            {
                var coord = Coord.CreateNewCoord(this, shipSize, "");
                AllShips.Add(new Ship(coord, this));
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
                    Console.Write("O");
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
            Console.WriteLine("Twoja Plansza:");
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

        public void EnemyShoots(Board board)
        {
            bool hit;

            do
            {
                Console.Clear();
                DrawBoard();
                hit = EnemyChooseCellToShoot(board);

                if (hit && this.LostAllFleet())
                    break;

                Console.ReadLine();
            } while (hit);
        }

        public bool EnemyChooseCellToShoot(Board board)
        {
            var coord = new Coord();
            do
            {
                coord.Number = RandomCoordinate.Next(10) + 1;
                coord.Letter = RandomCoordinate.Next(10) + 1;
                switch (BoardContent[coord.Letter, coord.Number])
                {
                    case CellContent.empty:
                        BoardContent[coord.Letter, coord.Number] = CellContent.missedShot;
                        Console.Clear();
                        DrawBoard();
                        Console.Write("Pudło!");
                        return false;
                    case CellContent.ship:
                        if (CheckIfShootSinkTheShip(board, coord))
                        {
                            Console.Clear();
                            DrawBoard();
                            Console.Write("Trafiony zatopiony! Przeciwnik może strzelić jeszcze raz.");
                        }
                        else
                        {
                            Console.Clear();
                            DrawBoard();
                            Console.Write("Trafiony! Przeciwnik może strzelić jeszcze raz.");
                        }
                        return true;
                    default:
                        break;
                }
            }
            while (true);
        }
    }
}
