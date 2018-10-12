using Nashet.UnitGame;
using System;
using System.Collections.Generic;

namespace Nashet.GameLogicAbstraction
{
    [Serializable]
    public class Command : ICommand
    {
        public Position Destination { get; protected set; }
        protected List<IUnitView> unitsList;

        public Command(Position destination, List<IUnitView> list)
        {
            Destination = destination;
            this.unitsList = list;
        }
        public IEnumerable<IUnitView> AllUnits
        {
            get
            {
                foreach (var item in unitsList)
                {
                    yield return item;
                }
            }
        }
    }
}