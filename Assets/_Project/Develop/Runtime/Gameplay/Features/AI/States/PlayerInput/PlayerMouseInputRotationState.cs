using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Utilities.StateMachineCore;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.AI.States
{
    public class PlayerMouseInputRotationState : State, IUpdatableState
    {
        // References
        private readonly Transform _transform = null;
        private readonly ReactiveVariable<Vector3> _mousePositionOnPlane = null;
        private readonly ReactiveVariable<Vector3> _rotationDirection = null;

        public PlayerMouseInputRotationState(Entity entity, Camera camera = null)
        {
            _mousePositionOnPlane = entity.MousePositionOnPlane;
            _rotationDirection = entity.RotationDirection;
            _transform = entity.Transform;
        }

        public void Update(float deltaTime)
        {
            if (_mousePositionOnPlane.Value == Vector3.zero)
                return;

            Vector3 lookDirection = (_mousePositionOnPlane.Value - _transform.position).normalized;
            lookDirection.y = 0f;

            _rotationDirection.Value = lookDirection;
        }
    }
}