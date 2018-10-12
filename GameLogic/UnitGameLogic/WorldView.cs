using System;

namespace Nashet.UnitsGameLogic
{
    [Serializable]
    internal class  WorldView : IWorldView
    {
        protected WorldView()
        {
        }

        public IMapView MapView { get; protected set; }
    }
}