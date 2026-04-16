using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public partial class MovementToTargetState : State, IUpdatableState
    {
        // References
        private readonly ITargetProvider _targetProvider = null;
        private readonly Transform _transform = null;
        private readonly ReactiveVariable<Entity> _currentTarget = null;
        private readonly ReactiveVariable<Vector3> _movementDirection = null;
        private readonly ReactiveVariable<Vector3> _rotationDirection = null;

        public MovementToTargetState(Entity entity, ITargetProvider targetProvider)
        {
            entity.AddCurrentTarget();

            _targetProvider = targetProvider;
            _transform = entity.Transform;
            _currentTarget = entity.CurrentTarget;
            _movementDirection = entity.MoveDirection;
            _rotationDirection = entity.RotationDirection;
        }

        public void Update(float deltaTime)
        {
            if (_targetProvider.TryGetTarget(out Entity target) == false)
                return;

            _currentTarget.Value = target;

            Vector3 directionToTarget
                = (_currentTarget.Value.Transform.position - _transform.position).normalized;

            _movementDirection.Value = directionToTarget;
            _rotationDirection.Value = directionToTarget;
        }

        public override void Exit()
        {
            base.Exit();

            _movementDirection.Value = Vector3.zero;
        }
    }
}
