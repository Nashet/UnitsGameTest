using System;

namespace Nashet.UnitGame
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