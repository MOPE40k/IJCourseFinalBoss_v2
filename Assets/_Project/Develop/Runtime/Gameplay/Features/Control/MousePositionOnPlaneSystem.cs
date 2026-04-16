using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Systems;
using Assets._Project.Develop.Runtime.Gameplay.Features.InputFeature;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.Control
{
    public class MousePositionOnPlaneSystem : IInitializableSystem, IUpdatableSystem
    {
        private readonly IInputService _inputService = null;
        private readonly Camera _cameraForRay = null;
        private Transform _planeTransform = null;
        private ReactiveVariable<Vector3> _mousePositionOnPlane = null;

        public MousePositionOnPlaneSystem(IInputService inputService, Camera cameraForRay = null)
        {
            _inputService = inputService;

            if (cameraForRay == null)
                _cameraForRay = Camera.main;
            else
                _cameraForRay = cameraForRay;
        }

        public void OnInit(Entity entity)
        {
            _planeTransform = entity.Transform;
            _mousePositionOnPlane = entity.MousePositionOnPlane;
        }

        public void OnUpdate(float deltaTime)
        {
            Ray mousePointRay = _cameraForRay.ScreenPointToRay(_inputService.MousePosition);

            Plane groundPlane = new Plane(Vector3.up, _planeTransform.position);

            if (groundPlane.Raycast(mousePointRay, out float distanceToMousePoint))
                _mousePositionOnPlane.Value = mousePointRay.GetPoint(distanceToMousePoint);
        }
    }
}