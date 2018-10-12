namespace Nashet.UnitGame
{
    internal interface IUnit : IUnitView, ISimulatable
    {
        void SetDestination(Position destination);
    }
}