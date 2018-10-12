using System.Collections.Generic;

namespace Nashet.UnitGame
{
    public interface IMapView
    {
        IEnumerable<IUnitView> AllUnits { get; }
        int TotalSize { get; }
        int XSize { get; }
        int YSize { get; }
    }
}