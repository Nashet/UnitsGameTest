using Nashet.GameLogicAbstraction;
namespace Nashet.UnitGame
{
    internal interface IWorld : IWorldView, ISimulatable
    {
        void ProcessCommand(ICommand command);
    }
}