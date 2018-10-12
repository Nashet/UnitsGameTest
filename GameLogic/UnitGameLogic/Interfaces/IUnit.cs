namespace Nashet.UnitsGameLogic
{
    internal interface IUnit : IUnitView, ISimulatable
    {
        void SetDestination(Position destination);
    }
}