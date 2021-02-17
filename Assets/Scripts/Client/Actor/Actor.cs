using System.Collections.Generic;
using System.Linq;
using Client.Abilities;
using Client.Managers;
using UnityEngine;

namespace Client.Actor
{
    public class Actor : MonoBehaviour, IActor, IResettable
    {
        [Header("Abilities must be derived from IActor")] 
        [SerializeField] private List<MonoBehaviour> _abilities = new List<MonoBehaviour>();

        public bool Initialized { get; private set; }

        public Transform Transform
        {
            get
            {
                if (_transform == null)
                {
                    _transform = GetComponent<Transform>();
                }

                return _transform;
            }
        }

        public List<IActorAbility> Abilities { get; set; } = new List<IActorAbility>();

        private Transform _transform;
        private IGameManager _gameManager;

        public void Initialize(IGameManager gameManager)
        {
            if (Initialized) return;
            
            _gameManager = gameManager;

            _abilities.ForEach(ability =>
            {
                var newAbility = (IActorAbility) ability;
                
                if (newAbility != null)
                {
                    Abilities.Add(newAbility);
                }
            });

            foreach (var ability in Abilities)
            {
                ability.Initialize(this, _gameManager);
            }

            Initialized = true;
        }

        public void PrepareForNewLevel(Vector3 newPosition)
        {
            Transform.position = newPosition;

            PrewarmSetup();
        }

        public void Reset()
        {
            gameObject.SetActive(false);

            foreach (var ability in Abilities.Where(a => a is IResettable).ToList())
            {
                ((IResettable) ability)?.Reset();
            }
        }

        public void PrewarmSetup()
        {
            gameObject.SetActive(true);

            foreach (var ability in Abilities.Where(a => a is IResettable).ToList())
            {
                ((IResettable) ability)?.PrewarmSetup();
            }
        }
    }
}