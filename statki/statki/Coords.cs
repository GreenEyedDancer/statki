using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static statki.DataModels;

namespace statki
{
    public class Coord
    {
        public Coord(string coordinate)
        {
            Letter = (int)coordinate[0] - 64;
            if (coordinate.Length == 3)
                Number = 10;
            else
                Number = Convert.ToInt32(coordinate[1]) - 48;
        }
        public int Letter { get; set; }
        public int Number { get; set; }
        public static bool ClassyCellCordsAreIncorrect(string coordinate)
        {
            return string.IsNullOrEmpty(coordinate)
                     || coordinate.Length < 2 || coordinate.Length > 3
                     || (!DataModels.PermissibleLetters.Contains(coordinate[0].ToString()))
                     || !DataModels.PermissibleNumbers.Contains(coordinate[1].ToString())
                     || coordinate.Length == 3 && coordinate[2] != '0';
        }

        public static Coord CreateNewCoord(int shipSize, string endOfShip)
        {
            string coordinate;
            do
            {
                Console.Write($"Podaj poprawna współrzędną {endOfShip} {shipSize}-polowego statku: ");
                coordinate = Console.ReadLine().ToUpper();
            }
            while (ClassyCellCordsAreIncorrect(coordinate));

            return new Coord(coordinate);
        }

        public bool CellIsAvailable(Board board)
        {
            if (OutOfBoardRange(Letter, Number))
                return false;

            for (int row = Letter - 1; row <= Letter + 1; row++)
            {
                for (int column = Number - 1; column <= Number + 1; column++)
                {
                    if (board.BoardContent[row, column] != CellContent.empty)
                        return false;
                }
            }
            return true;
        }

        public bool OutOfBoardRange(int coordLetter, int coordNumber)
        {
            return coordLetter < 1 || coordLetter > 10 || coordNumber < 1 || coordNumber > 10;
        }

        public static bool operator == (Coord c1, Coord c2)
        {
            return c1.Letter == c2.Letter && c1.Number == c2.Number;
        }

        public static bool operator != (Coord c1, Coord c2)
        {
            return !(c1 == c2);
        }
    }

    public class DoubleCoord
    {
        public DoubleCoord(int shipSize)
        {
            First = Coord.CreateNewCoord(shipSize, "początku");
            Second = Coord.CreateNewCoord(shipSize, "końca");
        }
        public Coord First { get; set; }
        public Coord Second { get; set; }

        public static DoubleCoord CreateBothCoords(int shipSize, Board board)
        {
            DoubleCoord result;
            do
            {
                result = new DoubleCoord(shipSize);
            }
            while (!(result.BothCellsAreAvailable(board) && result.CellsNotIdentical() && result.CorrectDistanceBetweenCoords(shipSize)));

            return result;
        }

        private bool BothCellsAreAvailable(Board board)
        {
            return First.CellIsAvailable(board) && Second.CellIsAvailable(board);
        }

        private bool CellsNotIdentical()
        {
            return First != Second;
        }

        private bool CorrectDistanceBetweenCoords(int shipSize)
        {
            return CorrectVertically(shipSize) || CorrectHorizontally(shipSize);
        }

        private bool CorrectVertically(int shipSize)
        {
            return Math.Abs(First.Letter - Second.Letter) == shipSize - 1
                && First.Number == Second.Number;
        }

        private bool CorrectHorizontally(int shipSize)
        {
            return Math.Abs(First.Number - Second.Number) == shipSize - 1
                && First.Letter == Second.Letter;
        }
    }
}
