
namespace Nashet.GameLogicAbstraction
{
    public static class GameLogicManager
    {
        public static IGameLogic GetOne()
        {
            return new GameLogic();
        }
    }
}
