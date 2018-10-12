using Nashet.GameLogicAbstraction;
using System;
using System.Linq;


namespace Nashet.UnitGame
{
    [Serializable]
    internal class World : WorldView, IWorld
    {
        protected const int MaxAmountOfUnits = 5;
        protected const int MinAmountOfUnits = 2;
        protected const int MaxMapSize = 12;
        protected const int MinMapSize = 7;

        [NonSerialized]
        internal IMap map;

        internal World()
        {
            var random = new Random();
            var mapSize = random.Next(MinMapSize, MaxMapSize + 1);

            MapView = new Map(mapSize, mapSize);

            map = MapView as Map;
            var unitsToCreate = random.Next(MinAmountOfUnits, MaxAmountOfUnits + 1);
            for (int i = 0; i < unitsToCreate; i++)
            {
                map.AddUnitInRandomPosition();
            }
        }

        public void SimulateOneTick()
        {
            map.SimulateOneTick();
        }

        public void ProcessCommand(ICommand command)
        {
            foreach (var unitOnClient in command.AllUnits)
            {
                var unitOnServer = map.AllUnits.FirstOrDefault(x => x.UID == unitOnClient.UID);
                if (unitOnServer == null)
                    ;// todo check if no such unit
                else
                    unitOnServer.SetDestination(command.Destination);
            }
        }
    }
}