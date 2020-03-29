using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static statki.DataModels;

namespace statki
{
    public class Ship
    {
        public class Part
        {
            public CellContent ShipPart;
            public int CoordLetter;
            public int CoordNumber;
        }

        public List<Part> Body;

        public Ship(Coord coord, Board board)
        {
            Body = new List<Part>
            {
                new Part
                {
                    ShipPart = CellContent.ship,
                    CoordLetter = coord.Letter,
                    CoordNumber = coord.Number
                }
            };

            UpdateShipOnBoard(board);
        }

        public Ship (DoubleCoord doubleCoord, Board board)
        {
            Body = new List<Part>();

            for (int row = Math.Min(doubleCoord.First.Letter, doubleCoord.Second.Letter);
                     row <= Math.Max(doubleCoord.First.Letter, doubleCoord.Second.Letter); row++)
            {
                for (int column = Math.Min(doubleCoord.First.Number, doubleCoord.Second.Number);
                     column <= Math.Max(doubleCoord.First.Number, doubleCoord.Second.Number); column++)
                {
                    Body.Add(new Part
                    {
                        ShipPart = CellContent.ship,
                        CoordLetter = row,
                        CoordNumber = column
                    });
                }
            }

            UpdateShipOnBoard(board);
        }

        public Ship(int shipSize, Board board)
        {
            Body = new List<Part>();
            int firstCoordLetter, secondCoordLetter,
                firstCoordNumber, secondCoordNumber;
            do
            {
                do
                {
                    firstCoordLetter = RandomCoordinate.Next(10) + 1;
                    firstCoordNumber = RandomCoordinate.Next(10) + 1;
                } while (!CellIsAvailable(firstCoordLetter, firstCoordNumber, board));

                secondCoordNumber = firstCoordNumber;
                secondCoordLetter = firstCoordLetter;

                if (CellIsAvailable(firstCoordLetter + shipSize - 1, firstCoordNumber, board))
                    secondCoordLetter = firstCoordLetter + shipSize - 1;
                else if (CellIsAvailable(firstCoordLetter, firstCoordNumber + shipSize - 1, board))
                    secondCoordNumber = firstCoordNumber + shipSize - 1;
                else if (CellIsAvailable(firstCoordLetter, firstCoordNumber - shipSize + 1, board))
                    secondCoordNumber = firstCoordNumber - shipSize + 1;
                else
                    secondCoordLetter = firstCoordLetter - shipSize + 1;

            } while (!CellIsAvailable(secondCoordLetter, secondCoordNumber, board));

            for (int row = Math.Min(firstCoordLetter, secondCoordLetter); row <= Math.Max(firstCoordLetter, secondCoordLetter); row++)
                for (int column = Math.Min(firstCoordNumber, secondCoordNumber); column <= Math.Max(firstCoordNumber, secondCoordNumber); column++)
                {
                    Body.Add(new Part
                    {
                        ShipPart = CellContent.ship,
                        CoordLetter = row,
                        CoordNumber = column
                    });
                }
            UpdateShipOnBoard(board);
        }

        public bool CellIsAvailable(int coordLetter, int coordNumber, Board board)
        {
            if (OutOfBoardRange(coordLetter, coordNumber))
                return false;

            for (int row = coordLetter - 1; row <= coordLetter + 1; row++)
            {
                for (int column = coordNumber - 1; column <= coordNumber + 1; column++)
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

        public void UpdateShipOnBoard(Board board)
        {
            foreach (var part in Body)
            {
                board.BoardContent[part.CoordLetter, part.CoordNumber] = part.ShipPart;
            }
        }

        public bool SinkShipIfFullyDamaged()
        {
            if (IsShipDestroyed())
            {
                foreach (Part p in Body)
                    p.ShipPart = CellContent.destroyedShip;

                return true;
            }
            return false;
        }

        public bool IsShipDestroyed()
        {
            foreach (Part p in Body)
            {
                if (p.ShipPart == CellContent.ship)
                    return false;
            }
            return true;
        }
    }
}
