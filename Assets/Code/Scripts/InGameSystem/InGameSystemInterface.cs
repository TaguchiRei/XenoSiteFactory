using InGameSystem;
namespace InGameSystemInterface
{
    public interface IUseTurnAction { }

    public interface ITurnStartAction : IUseTurnAction
    {
        void StartTurn();
    }

    public interface IOnTurnAction : IUseTurnAction
    {
        void OnTurn();
    }

    public interface ITurnEndAction : IUseTurnAction
    {
        void EndTurn();
    }

    public interface ITurnAllEndAction : IUseTurnAction
    {
        void AllEndTurn();
    }
}
