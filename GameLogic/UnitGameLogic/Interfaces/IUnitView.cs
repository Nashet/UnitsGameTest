using System.Collections.Generic;

namespace Nashet.UnitsGameLogic
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