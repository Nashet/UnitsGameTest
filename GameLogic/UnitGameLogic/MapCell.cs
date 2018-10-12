using System;

namespace Nashet.UnitGame
{
    [Serializable]
    internal class MapCell : IMapCell
    {
        public MapCell()
        {
        }

        public IUnit Unit { get; set; }

        public bool IsWalkable { get { return Unit == null; } }
        public bool IsEmpty { get { return Unit == null; } }

        public void AddUnit(IUnit unit)
        {
            this.Unit = unit;
        }

        public void MoveUnit(IMapCell anotherMapCell)
        {
            anotherMapCell.Unit = Unit;
            this.Unit = null;
        }
    }
}