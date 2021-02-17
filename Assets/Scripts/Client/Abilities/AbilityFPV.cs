using Cinemachine;
using Client.Actor;
using Client.Managers;
using UnityEngine;

namespace Client.Abilities
{
    public class AbilityFPV : MonoBehaviour, IActorAbility
    {
        [SerializeField] private CinemachineVirtualCamera _fpvCamera;
        
        public IActor Actor { get; set; }

        public CinemachineVirtualCamera FpvCamera => _fpvCamera;

        public void Initialize(IActor actor, IGameManager gameManager)
        {
            Actor = actor;
        }
        
        public void Execute()
        {
        }
    }
}