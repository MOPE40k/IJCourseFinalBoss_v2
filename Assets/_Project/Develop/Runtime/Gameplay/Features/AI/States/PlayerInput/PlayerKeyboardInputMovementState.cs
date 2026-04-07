using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.PlayerInput
{
    public class PlayerKeyboardInputMovementState : State, IUpdatableState
    {
        // References
        private readonly IInputService _inputService = null;

        // Runtime
        private readonly ReactiveVariable<Vector3> _movementDirection = null;
        private readonly ReactiveVariable<Vector3> _rotationDirection = null;

        public PlayerKeyboardInputMovementState(Entity entity, IInputService inputService)
        {
            _inputService = inputService;
            _movementDirection = entity.MoveDirection;
            _rotationDirection = entity.RotationDirection;
        }

        public override void Exit()
        {
            base.Exit();

            _movementDirection.Value = Vector3.zero;
        }

        public void Update(float deltaTime)
        {
            _movementDirection.Value = _inputService.Direction;
            _rotationDirection.Value = _inputService.Direction;
        }
    }
}