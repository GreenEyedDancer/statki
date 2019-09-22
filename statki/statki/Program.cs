using System;
using System.Collections.Generic;

namespace statki
{
    class Program
    {
        public static Random randomCoordinate = new Random();
        const int BoardSize = 11;
        public const string permissibleLetters = "ABCDEFGHIJ";
        public const string permissibleNumbers = "123456789";
        public enum CellContent { empty, ship, hitShip, destroyedShip, missedShot };

        public class Board
        {
            public CellContent[,] BoardContent = new CellContent[12, 12];
            public Fleet fleet;
            public Board()
            {
                for (int row = 1; row < BoardSize; row++)
                {
                    for (int column = 1; column < BoardSize; column++)
                        BoardContent[row, column] = (int)CellContent.empty;
                }
            }

            public virtual void DrawBoard() { } 

            //to change
            public bool CheckWinCondition()
            {
                for (int row = 1; row<BoardSize; row++)
                {
                    for (int column = 1; column<BoardSize; column++)
                    {
                        if (this.BoardContent[row, column] == CellContent.ship)
                            return false;
                    }
                }
                return true;
            }

            public class Fleet
            {
                public List<Ship> AllShips = new List<Ship>();

                public Fleet(Board board)
                {
                    for (int shipSize = 4; shipSize > 0; shipSize--)
                    {
                        for (int shipQuantity = 1; shipQuantity < 6 - shipSize; shipQuantity++)
                        {
                            AllShips.Add(new Ship(shipSize, board));
                        }
                    }
                }
                //?
                public void UpdateFleetOnBoard(Board board)
                {
                    foreach (Ship ship in AllShips)
                    {
                        ship.UpdateShipOnBoard(board);
                    }
                }
            }

            public class Ship
            {
                public class part
                {
                    public CellContent shipPart;
                    public int coordinateLetter;
                    public int coordinateNumber;
                }

                public List<part> body;

                public Ship(int shipSize, Board board)
                {
                    body = new List<part>();
                    int firstCoordinateLetter, secondCoordinateLetter,
                        firstCoordinateNumber, secondCoordinateNumber;
                    do
                    {
                        do
                        {
                            firstCoordinateLetter = randomCoordinate.Next(10) + 1;
                            firstCoordinateNumber = randomCoordinate.Next(10) + 1;
                        } while (!CheckSingleCellAvailability(firstCoordinateLetter, firstCoordinateNumber, board));

                        secondCoordinateNumber = firstCoordinateNumber;
                        secondCoordinateLetter = firstCoordinateLetter;

                        if (CheckSingleCellAvailability(firstCoordinateLetter + shipSize - 1, firstCoordinateNumber, board))
                            secondCoordinateLetter = firstCoordinateLetter + shipSize - 1;
                        else if (CheckSingleCellAvailability(firstCoordinateLetter, firstCoordinateNumber + shipSize - 1, board))
                            secondCoordinateNumber = firstCoordinateNumber + shipSize - 1;
                        else if (CheckSingleCellAvailability(firstCoordinateLetter, firstCoordinateNumber - shipSize + 1, board))
                            secondCoordinateNumber = firstCoordinateNumber - shipSize + 1;
                        else
                            secondCoordinateLetter = firstCoordinateLetter - shipSize + 1;

                    } while (!CheckSingleCellAvailability(secondCoordinateLetter, secondCoordinateNumber, board));

                    for (int row = Math.Min(firstCoordinateLetter, secondCoordinateLetter); row <= Math.Max(firstCoordinateLetter, secondCoordinateLetter); row++)
                        for (int column = Math.Min(firstCoordinateNumber, secondCoordinateNumber); column <= Math.Max(firstCoordinateNumber, secondCoordinateNumber); column++)
                        {
                            body.Add(new part
                            {
                                shipPart = CellContent.ship,
                                coordinateLetter = row,
                                coordinateNumber = column
                            });
                        }
                    UpdateShipOnBoard(board);

                }


                public bool CheckSingleCellAvailability(int coordinateLetter, int coordinateNumber, Board board)
                {
                    if ((coordinateLetter < 1 || coordinateLetter > 10) || (coordinateNumber < 1 || coordinateNumber > 10))
                        return false;
                    for (int row = coordinateLetter - 1; row <= coordinateLetter + 1; row++)
                    {
                        for (int column = coordinateNumber - 1; column <= coordinateNumber + 1; column++)
                        {
                            if (board.BoardContent[row, column] != CellContent.empty)
                                return false;
                        }
                    }
                    return true;
                }

                public void UpdateShipOnBoard(Board board)
                {
                    foreach (var p in body)
                    {
                        board.BoardContent[p.coordinateLetter, p.coordinateNumber] = p.shipPart;
                    }
                }

                public void SinkShipIfFullyDamaged()
                {
                    if (CheckIfShipSank())
                    {
                        foreach (part p in body)
                        {
                            p.shipPart = CellContent.destroyedShip;
                        }
                    }
                }

                public bool CheckIfShipSank()
                {
                    bool result = true;
                    foreach (part p in body)
                    {
                        if (p.shipPart == CellContent.ship)
                            result = false;
                    }
                    return result;
                }

            }
        }

        public class PlayerBoard : Board
        {
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

            public override void DrawBoard()
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

            /*public void PlaceAllShipsOnBoard()
            {
                for (int shipSize = 4; shipSize > 0; shipSize--)
                {
                    for (int shipQuantity = 1; shipQuantity < 6-shipSize; shipQuantity++)
                    {
                        AskPlayerToPlaceShip(shipSize);
                        Console.Clear();
                        Console.WriteLine("Twoja aktualna plansza: ");
                        DrawBoard();
                    }
                }
                
            }*/

            /*public void AskPlayerToPlaceShip(int shipSize)
            {
                int firstCoordinateLetter, secondCoordinateLetter,
                    firstCoordinateNumber, secondCoordinateNumber;
                if (shipSize > 1)
                {
                    do
                    {
                        Console.Write("Podaj porawna współrzędną początku {0}-polowego statku: ", shipSize);
                        string coordinate = Console.ReadLine();
                        firstCoordinateLetter = (int)coordinate[0]-64;
                        if (coordinate.Length == 3)
                            firstCoordinateNumber = 10;
                        else
                        firstCoordinateNumber = Convert.ToInt32(coordinate[1])-48;
                        Console.Write("Podaj poprawna współrzędną końca {0}-polowego statku: ", shipSize);
                        coordinate = Console.ReadLine();
                        secondCoordinateLetter = (int)coordinate[0]-64;
                        if (coordinate.Length == 3)
                            secondCoordinateNumber = 10;
                        else
                            secondCoordinateNumber = Convert.ToInt32(coordinate[1])-48;
                    }
                    while (!CheckShipAvaliability(firstCoordinateLetter, secondCoordinateLetter, firstCoordinateNumber, secondCoordinateNumber, shipSize));

                    PlaceShipOnBoard(firstCoordinateLetter, secondCoordinateLetter, firstCoordinateNumber, secondCoordinateNumber);
                }
                else
                {
                    do
                    {
                        Console.Write("Podaj poprawna współrzędną 1-polowego statku: ");
                        string coordinate = Console.ReadLine();
                        firstCoordinateLetter = (int)coordinate[0]-64;
                        if (coordinate.Length == 3)
                            firstCoordinateNumber = 10;
                        else
                            firstCoordinateNumber = (int)coordinate[1]-48;
                    }
                    while (!CheckShipAvaliability(firstCoordinateLetter, firstCoordinateLetter, firstCoordinateNumber, firstCoordinateNumber, shipSize));

                    PlaceShipOnBoard(firstCoordinateLetter, firstCoordinateLetter, firstCoordinateNumber, firstCoordinateNumber);
                }
            }*/

            public void EnemyShoots(Board board)
            {
                bool hit = false;

                do
                {
                    Console.Clear();
                    board.DrawBoard();
                    hit = ChooseCellToShoot(board);

                    if (this.CheckWinCondition())
                        break;

                    Console.ReadLine();
                } while (hit);

            }

            public bool ChooseCellToShoot(Board board, int? lastHitLetter = null, int? lastHitNumber = null)
            {
                int coordinateLetter, coordinateNumber;
                bool correctShot;
                do
                {
                    coordinateNumber = randomCoordinate.Next(10) + 1;
                    coordinateLetter = randomCoordinate.Next(10) + 1;
                    correctShot = true;
                    switch (BoardContent[coordinateLetter, coordinateNumber])
                    {
                        case CellContent.empty:
                            BoardContent[coordinateLetter, coordinateNumber] = CellContent.missedShot;
                            Console.Clear();
                            board.DrawBoard();
                            Console.WriteLine("Pudło!");
                            return false;
                        case CellContent.ship:
                            if (CheckIfShootSinkTheShip(board, coordinateLetter, coordinateNumber))
                            {
                                Console.Clear();
                                board.DrawBoard();
                                Console.WriteLine("Trafiony zatopiony! Przeciwnik może strzelić jeszcze raz.");
                            }
                            else
                            {
                                Console.Clear();
                                board.DrawBoard();
                                Console.WriteLine("Trafiony! Przeciwnik może strzelić jeszcze raz.");
                            }

                            return true;
                        default:
                            correctShot = false;
                            break;
                    }
                }
                while (!correctShot);

                return false;
            }

            public bool CheckIfShootSinkTheShip(Board board, int coordinateLetter, int coordinateNumber)
            {
                bool correctShip = false;
                foreach (Ship ship in fleet.AllShips)
                {
                    foreach (Ship.part p in ship.body)
                    {
                        if (p.coordinateLetter == coordinateLetter && p.coordinateNumber == coordinateNumber)
                        {
                            correctShip = true;
                            p.shipPart = CellContent.hitShip;
                        }
                    }
                    if (correctShip)
                    {
                        ship.SinkShipIfFullyDamaged();
                        ship.UpdateShipOnBoard(board);
                        if (ship.CheckIfShipSank())
                            return true;
                    }
                }
                return false;
            }

        }

        public class EnemyBoard : Board
        {
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

            public override void DrawBoard()
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
                bool hit = false;

                do
                {
                    Console.Clear();
                    DrawBoard();
                    hit = ChooseCellToShoot(board);
                    if (this.CheckWinCondition())
                        break;
                    Console.ReadLine();
                } while (hit);

            }

            public bool ChooseCellToShoot(Board board)
            {
                int coordinateNumber;
                string coordinate;
                bool incorrectThreeChars;
                do
                {
                    do
                    {
                        incorrectThreeChars = false;
                        Console.WriteLine("Podaj prawidłową współrzędna do strzału");
                        coordinate = Console.ReadLine().ToUpper();

                        if (coordinate.Length == 3 && coordinate[2] != '0')
                            incorrectThreeChars = true;
                    }
                    while (string.IsNullOrEmpty(coordinate));
                }
                while ((!permissibleLetters.Contains(coordinate[0].ToString()))
                      || !permissibleNumbers.Contains(coordinate[1].ToString())
                      || incorrectThreeChars || coordinate.Length > 3);


                int coordinateLetter = (int)coordinate[0] - 64;
                if (coordinate.Length == 3)
                    coordinateNumber = 10;
                else
                    coordinateNumber = coordinate[1]-48;

                switch (BoardContent[coordinateLetter, coordinateNumber])
                {
                    case CellContent.empty:
                        BoardContent[coordinateLetter, coordinateNumber] = CellContent.missedShot;
                        Console.Clear();
                        board.DrawBoard();
                        Console.WriteLine("Pudło!");
                        break;
                    case CellContent.ship:
                        if (CheckIfShootSinkTheShip(board, coordinateLetter, coordinateNumber))
                        {
                            Console.Clear();
                            board.DrawBoard();
                            Console.WriteLine("Trafiony zatopiony! Możesz strzelić jeszcze raz.");
                        }
                        else
                        {
                            Console.Clear();
                            board.DrawBoard();
                            Console.WriteLine("Trafiony! Możesz strzelić jeszcze raz.");
                        }
                        return true;
                    case CellContent.destroyedShip:
                    case CellContent.hitShip:
                    case CellContent.missedShot:
                        Console.Write("Już tu strzelałeś! Wybierz inną współrzędną.");
                        return true;
                    default:
                        Console.Write("Wystapil blad z obiektem na planszy!");
                        break;
                }
                return false;
            }

            public bool CheckIfShootSinkTheShip(Board board, int coordinateLetter, int coordinateNumber)
            {
                bool correctShip = false;
                foreach (Ship ship in fleet.AllShips)
                {
                    foreach (Ship.part p in ship.body)
                    {
                        if (p.coordinateLetter == coordinateLetter && p.coordinateNumber == coordinateNumber)
                        {
                            correctShip = true;
                            p.shipPart = CellContent.hitShip;
                        }
                    }
                    if (correctShip)
                    {
                        ship.SinkShipIfFullyDamaged();
                        ship.UpdateShipOnBoard(board);
                        if (ship.CheckIfShipSank())
                            return true;
                    }
                }
                return false;
            }

        }

        public class Game
        {
            public static void Great()
            {
                Console.WriteLine("Witaj w grze w statki!");
            }

            public static void Start()
            {
                PlayerBoard playerBoard = new PlayerBoard();
                EnemyBoard enemyBoard = new EnemyBoard();

                Game.Great();
                playerBoard.fleet = new Board.Fleet(playerBoard);
                enemyBoard.fleet = new Board.Fleet(enemyBoard);
                bool gameOver = false;

                do
                {
                    Game.RefreshScreen(playerBoard, enemyBoard);
                    Console.ReadLine();
                    Game.PlayersShoot(playerBoard, enemyBoard);
                    gameOver = Game.CheckIfGameIsOver(playerBoard, enemyBoard);
                } while (!gameOver);

                if (playerBoard.CheckWinCondition())
                {
                    Console.Clear();
                    Console.WriteLine("Przegrałeś!");
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Wygrałeś!");
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
                if (playerBoard.CheckWinCondition() || enemyBoard.CheckWinCondition())
                    return true;

                return false;
            }
        }

        static void Main(string[] args)
        {
            Game.Start();
        }
    }
}