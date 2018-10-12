using System.Collections.Generic;

namespace Nashet.UnitGame
{
    public interface IUnitView : ISimulatable
    {
        IEnumerable<Position> Path { get; }
        int UID { get; }
        bool IsWalking { get; }
        Position Position { get; }
        Position Destination { get; }
    }
}