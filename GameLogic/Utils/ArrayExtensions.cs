using System.Collections.Generic;

namespace Nashet.UnitsGameLogic
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Gives valid coordinates of array elements within specified radius of some point. 
        /// </summary>        
        public static IEnumerable<Position> GetNeighborsCoords(int xLength, int yLength, int x, int y, int radius)
        {
            if (x > 0)
                yield return new Position(x - 1, y);
            if (x + 1 < xLength)
                yield return new Position(x + 1, y);
            if (y > 0)
                yield return new Position(x, y - 1);
            if (y + 1 < yLength)
                yield return new Position(x, y + 1);
        }

        /// <summary>
        /// Gives valid coordinates of array elements within specified radius of some point. 
        /// </summary>        
        public static IEnumerable<Position> GetNeighborsCoords<T>(this T[,] array, Position position, int radius)
        {
            return GetNeighborsCoords(array.GetLength(0), array.GetLength(1), position.x, position.y, radius);
        }

        /// <summary>
        /// false means that cell doesn't exist (wrong index)
        /// </summary>   
        public static bool IsCellExists<T>(this T[,] array, Position position)
        {
            if (position.x < array.GetLength(0) && position.y < array.GetLength(1) && position.x >= 0 && position.y >= 0)
                return true;
            else
                return false;
        }
    }
}