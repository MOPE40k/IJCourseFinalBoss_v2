using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States.PlayerInput
{
    public class PlayerKeyboardInputRotationState : State, IUpdatableState
    {
        // References
        private IInputService _inputService;
        private ReactiveVariable<Vector3> _rotationDirection;

        public PlayerKeyboardInputRotationState(
            Entity entity,
            IInputService inputService)
        {
            _inputService = inputService;
            _rotationDirection = entity.RotationDirection;
        }

        public void Update(float deltaTime)
            => _rotationDirection.Value = _inputService.Direction;
    }
}