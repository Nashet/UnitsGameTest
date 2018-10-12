using System.Collections.Generic;

namespace Nashet.UnitGame
{
    internal interface IMap :ISimulatable
    {
        List<Position> GetPath(Position startingPosition, Position destination);
        void AddUnitInRandomPosition();
        IEnumerable<IUnit> AllUnits { get; }
    }
}