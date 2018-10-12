using Nashet.UnitGame;
using System;
using System.Collections.Generic;

namespace Nashet.GameLogicAbstraction
{
    [Serializable]
    internal class GameLogic : IGameLogic, ISimulatable, IExpandableContent
    {
        [NonSerialized]
        protected List<ISimulatable> components = new List<ISimulatable>();
        public IWorldView World { get; protected set; }
        protected IWorld world;

        internal GameLogic()
        {
            world = new World();
            World = world;
            AddNewContent(world);
        }

        public void SimulateOneTick()
        {
            foreach (var item in components)
            {
                item.SimulateOneTick();
            }
        }

        public void ProcessCommand(ICommand command)
        {
            world.ProcessCommand(command);
        }

        public void AddNewContent(ISimulatable content)
        {
            if (content == null)
                throw new ArgumentNullException();
            components.Add(content);
        }
    }
}
