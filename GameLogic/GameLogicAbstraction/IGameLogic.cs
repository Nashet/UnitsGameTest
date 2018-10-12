using Nashet.UnitGame;

namespace Nashet.GameLogicAbstraction
{
    public interface IGameLogic :ISimulatable
    {
        IWorldView World { get; }

        void ProcessCommand(ICommand command);
    }
}