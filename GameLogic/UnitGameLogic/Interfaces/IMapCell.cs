namespace Nashet.UnitGame
{
    internal interface IMapCell
    {
        bool IsWalkable { get; }
        bool IsEmpty { get; }
        void AddUnit(IUnit unit);
        IUnit Unit { get; set; }

        void MoveUnit(IMapCell mapCell);
    }

}