using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.MainHero;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class MovementToTargetState : State, IUpdatableState
    {
        // References
        private readonly MainHeroHolderService _mainHeroHolderService = null;
        private readonly Transform _transform = null;
        private readonly ReactiveVariable<Entity> _currentTarget = null;
        private readonly ReactiveVariable<Vector3> _movementDirection = null;
        private readonly ReactiveVariable<Vector3> _rotationDirection = null;

        // Runtime
        private IDisposable _mainHeroRegistered = null;

        public MovementToTargetState(Entity entity, MainHeroHolderService mainHeroHolderService)
        {
            entity.AddCurrentTarget();

            _mainHeroHolderService = mainHeroHolderService;
            _transform = entity.Transform;
            _currentTarget = entity.CurrentTarget;
            _movementDirection = entity.MoveDirection;
            _rotationDirection = entity.RotationDirection;
        }

        public override void Enter()
        {
            base.Enter();

            if (_mainHeroHolderService.MainHero is null)
                _mainHeroRegistered = _mainHeroHolderService.HeroRegistred.Subscribe(OnHeroRegistered);
            else
                _currentTarget.Value = _mainHeroHolderService.MainHero;
        }

        private void OnHeroRegistered(Entity entity)
            => _currentTarget.Value = entity;

        public void Update(float deltaTime)
        {
            if (_currentTarget.Value is null)
                return;

            Vector3 directionToTarget
                = (_currentTarget.Value.Transform.position - _transform.position).normalized;

            _movementDirection.Value = directionToTarget;
            _rotationDirection.Value = directionToTarget;
        }

        public override void Exit()
        {
            base.Exit();

            _movementDirection.Value = Vector3.zero;

            _mainHeroRegistered?.Dispose();
        }
    }
}
