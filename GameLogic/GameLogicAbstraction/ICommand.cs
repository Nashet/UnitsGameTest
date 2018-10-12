using Nashet.UnitGame;
using System.Collections.Generic;

namespace Nashet.GameLogicAbstraction
{
    public interface ICommand
    {
        IEnumerable<IUnitView> AllUnits { get; }
        Position Destination { get; }
    }
}