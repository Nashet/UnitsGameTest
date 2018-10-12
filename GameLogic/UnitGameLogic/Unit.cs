using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Nashet.UnitsGameLogic
{
    [Serializable]
    internal class Unit : IUnit, IUnitView
    {
        [NonSerialized] private Map map;

        protected Position[] path = new Position[0];
        public Position Position { get; protected set; }
        public Position Destination
        {
            get
            {
                if (path.Length == 0)
                    return Position;
                else
                    return path[path.Length - 1];
            }
        }
        //public Position Destination { get; protected set; }

        protected static int UIDCounter;
        public int UID { get; protected set; }

        internal Unit(Map map, int xPositon, int yPositon)
        {
            this.map = map;
            Position = new Position(xPositon, yPositon);            
            UID = UIDCounter;
            UIDCounter++;
        }

        public bool IsWalking { get; protected set; }

        public IEnumerable<Position> Path
        {
            get
            {
                if (path != null)
                    foreach (var item in path)
                    {
                        yield return item;
                    }
            }
        }


        public void SetDestination(Position destination)
        {
            if (map.IsCellExists(destination))
            {
                if (destination != Position)// do nothing if unit is already in position
                    path = map.GetPath(Position, destination).ToArray();
            }
            else
                throw new ArgumentOutOfRangeException();
        }

        internal event EventHandler UnitWalked;

        internal class WalkedArgs : EventArgs
        {
            public WalkedArgs(Position previousPosition, Position nextPosition)
            {
                PreviousPosition = previousPosition;
                NextPosition = nextPosition;
            }

            public Position PreviousPosition { get; protected set; }
            public Position NextPosition { get; protected set; }
        }

        protected void Walk()
        {
            //map.MoveUnit(this, path[0]);
            Position previousPosition = Position;
            Position = path[0];
            // deletes first element
            path = path.Skip(1).ToArray();

            EventHandler handler = UnitWalked;
            if (handler != null)
            {
                handler(this, new WalkedArgs(previousPosition, Position));
            }
        }

        public void SimulateOneTick()
        {
            if (Path.IsEmpty())
                IsWalking = false;
            else
            {
                if (map.IsWalkable(path[0]))
                {
                    Walk();
                    IsWalking = true;
                }
                else // find another path
                {
                    path = map.GetPath(Position, Destination).ToArray(); // try to find new way
                    IsWalking = false;
                }
            }
        }
    }
}