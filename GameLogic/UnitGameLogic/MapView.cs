using Nashet.UnitGame;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nashet.UnitGame
{
    [Serializable]
    internal class MapView : IMapView
    {
        protected readonly IMapCell[,] map;

        protected readonly int ySize;
        public int YSize { get { return ySize; } }

        protected readonly int xSize;
        public int XSize { get { return xSize; } }

        public int TotalSize { get { return xSize * ySize; } }

        protected MapView(int xSize, int ySize)
        {
            this.ySize = ySize;
            this.xSize = xSize;

            // creates map
            map = new IMapCell[xSize, ySize];
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    map[x, y] = new MapCell();
                }
            }
        }

        public IEnumerable<IUnitView> AllUnits
        {
            get
            {
                for (int x = 0; x < xSize; x++)
                {
                    for (int y = 0; y < YSize; y++)
                    {
                        if (!map[x, y].IsEmpty)
                            yield return map[x, y].Unit;
                    }
                }
            }
        }

        internal IEnumerable<IMapCell> AllCells
        {
            get
            {
                for (int x = 0; x < xSize; x++)
                {
                    for (int y = 0; y < YSize; y++)
                    {
                        yield return map[x, y];
                    }
                }
            }
        }

        internal Position GetRandomPoint()
        {
            Random random = new Random();
            return new Position(random.Next(0, xSize), random.Next(0, ySize));
        }

        internal IMapCell GetRandomCell()
        {
            Random random = new Random();
            return map[random.Next(0, xSize), random.Next(0, ySize)];
        }

        /// <summary>
        /// May return null if there is no empty cell
        /// </summary>
        internal IMapCell GetEmptyRandomCell()
        {
            for (int i = 0; i < TotalSize; i++)
            {
                var point = GetRandomCell();
                if (point.IsEmpty)
                {
                    return point;
                }
            }
            return null;
        }  

        internal bool IsWalkable(Position position)
        {
            if (!map.IsCellExists(position))
                throw new ArgumentOutOfRangeException();

            return map[position.x, position.y].IsWalkable;
        }
    }
}