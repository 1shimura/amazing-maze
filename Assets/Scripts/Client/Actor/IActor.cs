using System.Collections.Generic;
using Client.Abilities;
using Client.Managers;
using UnityEngine;

namespace Client.Actor
{
    public interface IActor
    {
        bool Initialized { get; }
        Transform Transform { get; }
        List<IActorAbility> Abilities { get; set; }
        void Initialize(IGameManager gameManager);
        void PrepareForNewLevel(Vector3 newPosition);
    }
}