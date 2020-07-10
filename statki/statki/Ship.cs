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
            public CellContent ShipPart { get; set; }
            public Coord Coord { get; set; }
        }

        public List<Part> Body;

        public Ship(Coord coord, Board board)
        {
            Body = new List<Part>
            {
                new Part
                {
                    ShipPart = CellContent.ship,
                    Coord = coord
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
                        Coord = new Coord { Letter = row, Number = column}
                    });
                }
            }

            UpdateShipOnBoard(board);
        }

        public Ship(int shipSize, Board board)
        {
            Body = new List<Part>();
            var Coord1 = new Coord();
            var Coord2 = new Coord();

            do
            {
                do
                {
                    Coord1.Letter =  RandomCoordinate.Next(10) + 1;
                    Coord1.Number = RandomCoordinate.Next(10) + 1;
                } while (!CellIsAvailable(Coord1, board));

                Coord2.Number = Coord1.Number;
                Coord2.Letter = Coord1.Letter;

                if (CellIsAvailable(Coord1.Letter + shipSize - 1, Coord1.Number, board))
                    Coord2.Letter = Coord1.Letter + shipSize - 1;
                else if (CellIsAvailable(Coord1.Letter, Coord1.Number + shipSize - 1, board))
                    Coord2.Number = Coord1.Number + shipSize - 1;
                else if (CellIsAvailable(Coord1.Letter, Coord1.Number - shipSize + 1, board))
                    Coord2.Number = Coord1.Number - shipSize + 1;
                else
                    Coord2.Letter = Coord1.Letter - shipSize + 1;

            } while (!CellIsAvailable(Coord2, board));

            for (int row = Math.Min(Coord1.Letter, Coord2.Letter);
                row <= Math.Max(Coord1.Letter, Coord2.Letter); row++)
            {
                for (int column = Math.Min(Coord1.Number, Coord2.Number);
                    column <= Math.Max(Coord1.Number, Coord2.Number); column++)
                {
                    Body.Add(new Part
                    {
                        ShipPart = CellContent.ship,
                        Coord = new Coord { Letter = row, Number = column }
                    });
                }
            }

            UpdateShipOnBoard(board);
        }

        public bool CellIsAvailable(Coord coord, Board board)
        {
            if (OutOfBoardRange(coord))
                return false;

            for (int row = coord.Letter - 1; row <= coord.Letter + 1; row++)
            {
                for (int column = coord.Number - 1; column <= coord.Number + 1; column++)
                {
                    if (board.BoardContent[row, column] != CellContent.empty)
                        return false;
                }
            }
            return true;
        }

        public bool OutOfBoardRange(Coord coord)
        {
            return coord.Letter < 1 || coord.Letter > 10 || coord.Number < 1 || coord.Number > 10;
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
                board.BoardContent[part.Coord.Letter, part.Coord.Number] = part.ShipPart;
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
