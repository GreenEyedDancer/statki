using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static statki.DataModels;
using static statki.Ship;

namespace statki
{
    public class Board
    {
        public CellContent[,] BoardContent = new CellContent[12, 12];
        public List<Ship> AllShips = new List<Ship>();
        public Board()
        {
            for (int row = 1; row < BoardSize; row++)
            {
                for (int column = 1; column < BoardSize; column++)
                    BoardContent[row, column] = (int)CellContent.empty;
            }
        }

        public bool LostAllFleet()
        {
            for (int row = 1; row < BoardSize; row++)
            {
                for (int column = 1; column < BoardSize; column++)
                {
                    if (this.BoardContent[row, column] == CellContent.ship)
                        return false;
                }
            }
            return true;
        }

        public bool CheckIfShootSinkTheShip(Board board, Coord coord)
        {
            bool correctShip = false;
            foreach (Ship ship in AllShips)
            {
                foreach (Part p in ship.Body)
                {
                    if (p.Coord == coord)
                    {
                        correctShip = true;
                        p.ShipPart = CellContent.hitShip;
                    }
                }
                if (correctShip)
                {
                    bool isSank = ship.SinkShipIfFullyDamaged();
                    ship.UpdateShipOnBoard(board);

                    return isSank;
                }
            }
            return false;
        }
    }
}
