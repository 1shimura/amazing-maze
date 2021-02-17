using Client.Actor;
using Client.Managers;
using UnityEngine;

namespace Client.Abilities
{
    public class AbilityPickupableItem : MonoBehaviour, IActorAbility
    {
        public IActor Actor { get; set; }
        private IGameManager _gameManager;
        
        public void Initialize(IActor actor, IGameManager gameManager)
        {
            Actor = actor;
            _gameManager = gameManager;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            var actor = other.gameObject.GetComponent<IActor>();
            
            if (actor == null) return;
            
            Execute();
        }
        
        public void Execute()
        {
            (Actor as IResettable)?.Reset();
            _gameManager.ItemFound();
        }
    }
}