using System;
using Client.Actor;
using Client.Managers;
using UnityEngine;

namespace Client.Abilities
{
    [RequireComponent(typeof(Rigidbody))]
    public class AbilityMoveByInput : MonoBehaviour, IActorAbility, IResettable
    {
        [SerializeField] private float _movementSpeed = 12f;
        [SerializeField] private float _turnSpeed = 180f;

        [SerializeField] private string _movementAxisName;
        [SerializeField] private string _turnAxisName;

        [Header("Animation Settings")] 
        [SerializeField] private bool _hasAnimation;
        [SerializeField] private string _walkAnimationParam;

        private float _movementInputValue;
        private float _turnInputValue;

        private Rigidbody _rigidbody;

        private bool _inputEnable;

        private Animator _animator;
        private bool _isAnimationRunning;

        private Vector3 _prevMovement;
        private Quaternion _prevTurn;

        public IActor Actor { get; set; }

        public void Initialize(IActor actor, IGameManager gameManager)
        {
            Actor = actor;
        }

        public void Execute()
        {
            Move();
            Turn();
        }

        public void PrewarmSetup()
        {
            _inputEnable = true;
        }

        public void Reset()
        {
            _inputEnable = false;
            _rigidbody.velocity = Vector3.zero;
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (_hasAnimation)
            {
                _animator = GetComponent<Animator>();
            }
        }

        private void Update()
        {
            if (!_inputEnable) return;

            _movementInputValue = Input.GetAxis(_movementAxisName);
            _turnInputValue = Input.GetAxis(_turnAxisName);
        }

        private void FixedUpdate()
        {
            Execute();
        }

        private void Move()
        {
            var movement = transform.forward * (_movementInputValue * _movementSpeed * Time.deltaTime);
            var newPosition = _rigidbody.position + movement;
            
            var posChanged = Vector3.Distance(_prevMovement, newPosition) > 0.01f;

            if (_hasAnimation && _isAnimationRunning != posChanged)
            {
                _animator.SetBool(_walkAnimationParam, posChanged);
                _isAnimationRunning = posChanged;
            }

            if (!posChanged) return;

            _rigidbody.MovePosition(newPosition);
            _prevMovement = newPosition;
        }

        private void Turn()
        {
            var turn = _turnInputValue * _turnSpeed * Time.deltaTime;
            var turnRotation = Quaternion.Euler(0f, turn, 0f);
            var newTurn = _rigidbody.rotation * turnRotation;

            var turnChanged = newTurn != _prevTurn;

            if (!turnChanged) return;

            _rigidbody.MoveRotation(newTurn);
            _prevTurn = newTurn;
        }
    }
}