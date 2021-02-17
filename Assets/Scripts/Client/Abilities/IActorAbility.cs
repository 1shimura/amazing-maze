using Client.Actor;
using Client.Managers;

namespace Client.Abilities
{
    public interface IActorAbility
    {
        IActor Actor { get; set; }
        void Execute();
        void Initialize(IActor actor, IGameManager gameManager);
    }
}