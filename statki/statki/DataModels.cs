using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace statki
{
    public static class DataModels
    {
        public static Random RandomCoordinate = new Random();
        public const int BoardSize = 11;
        public const string PermissibleLetters = "ABCDEFGHIJ";
        public const string PermissibleNumbers = "123456789";
        public enum CellContent { empty, ship, hitShip, destroyedShip, missedShot };

        public static bool CellCordsAreIncorrect(string coordinate)
        {
            return string.IsNullOrEmpty(coordinate)
                     || (!PermissibleLetters.Contains(coordinate[0].ToString()))
                     || !PermissibleNumbers.Contains(coordinate[1].ToString())
                     || coordinate.Length == 3 && coordinate[2] != '0'
                     || coordinate.Length > 3;
        }
    }
}
