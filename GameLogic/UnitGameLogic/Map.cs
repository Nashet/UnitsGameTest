using System;
using System.Collections.Generic;
using System.Linq;

namespace Nashet.UnitGame
{
    [Serializable]
    internal class Map : MapView, IMap
    {
        /// <summary> Path choice weight for non empty cell.
        /// Units will prefer to stay against obstacle if other way
        /// is more than NotEmptyCellPenalty steps</summary>
        protected const int NotEmptyCellPenalty = 100;

        [NonSerialized] protected readonly int[,] flood;

        internal Map(int xSize, int ySize) : base(xSize, ySize)
        {
            flood = new int[xSize, ySize];
        }

        IEnumerable<IUnit> IMap.AllUnits
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

        public void AddUnitInRandomPosition()
        {
            if (AllCells.Any(x => x.IsEmpty))
            {
                var randomPoint = GetRandomPoint();
                var cell = map[randomPoint.x, randomPoint.y];
                while (!cell.IsEmpty)
                {
                    randomPoint = GetRandomPoint();
                    cell = map[randomPoint.x, randomPoint.y];
                }
                var newUnit = new Unit(this, randomPoint.x, randomPoint.y);
                newUnit.UnitWalked += OnUnitWalked;
                cell.AddUnit(newUnit);
            }
        }

        protected void OnUnitWalked(object sender, EventArgs e)
        {
            var unit = sender as IUnit;
            var args = e as Unit.WalkedArgs;
            map[args.PreviousPosition.x, args.PreviousPosition.y].MoveUnit(map[args.NextPosition.x, args.NextPosition.y]);

        }

        internal bool IsCellExists(Position destination)
        {
            return map.IsCellExists(destination);
        }

        protected void FloodFillNeighbors(int x, int y, int step)
        {
            FloodFillNeighbors(new Position(x, y), step);
        }

        protected void FloodFillNeighbors(Position position, int step)
        {
            if (!map.IsCellExists(position))
                throw new ArgumentOutOfRangeException();

            var neighbors = map.GetNeighborsCoords(position, 1);
            foreach (var item in neighbors)
            {
                if (flood[item.x, item.y] == int.MaxValue)
                {
                    if (map[item.x, item.y].IsWalkable)
                        flood[item.x, item.y] = step;
                    else
                        flood[item.x, item.y] = step + NotEmptyCellPenalty;
                }
            }
        }

        /// <summary>
        /// If there is no clear way returns path across obstacles, assuming that obstacle will move away eventually.
        /// Uses flood fill algorithm.
        /// </summary>
        public List<Position> GetPath(Position startingPosition, Position destination)
        {
            if (!map.IsCellExists(startingPosition) || !map.IsCellExists(destination))
                throw new ArgumentOutOfRangeException();

            // just clears all values
            for (int x = 0; x < xSize; x++)
            {
                for (int y = 0; y < ySize; y++)
                {
                    flood[x, y] = int.MaxValue;
                }
            }

            // makes flood filling from startingPosition to destination
            // each next wave has +1 value
            flood[startingPosition.x, startingPosition.y] = 0;
            FloodFillNeighbors(startingPosition.x, startingPosition.y, 1);
            int step = 1;
            bool findDeWay = false;
            while (!findDeWay)
            {
                for (int x = 0; x < xSize && !findDeWay; x++)
                {
                    for (int y = 0; y < ySize && !findDeWay; y++)
                    {
                        if (flood[x, y] == step)
                        {
                            FloodFillNeighbors(x, y, step + 1);
                            if (destination.x == x && destination.y == y)
                            {
                                findDeWay = true;                                
                            }
                        }
                    }
                }
                step++;
            }
            
            // builds the path in backward order - from destination to startingPosition
            var res = new List<Position> { destination };
            var foundStep = destination;
            do
            {
                // chooses step with minimal value
                foundStep = map.GetNeighborsCoords(foundStep, 1)
                    .Where(coords => flood[coords.x, coords.y] < flood[foundStep.x, foundStep.y])
                    .MinByRandom(coords => flood[coords.x, coords.y]);
                res.Add(foundStep);
            } while (foundStep != startingPosition);

            res.RemoveAt(res.Count - 1);
            res.Reverse();
            return res;
        }

        public void SimulateOneTick()
        {
            foreach (var unit in AllUnits.ToArray())// makes copy
            {
                unit.SimulateOneTick();
            }
        }
    }
}