using Nashet.GameLogicAbstraction;
namespace Nashet.UnitsGameLogic
{
    internal interface IWorld : IWorldView, ISimulatable
    {
        void ProcessCommand(ICommand command);
    }
}