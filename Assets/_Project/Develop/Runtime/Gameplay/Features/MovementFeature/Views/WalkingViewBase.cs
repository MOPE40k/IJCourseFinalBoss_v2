using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;

namespace CourseGameVideo.Assets._Project.Develop.Runtime.Gameplay.Features.MovementFeature.Views
{
    public class WalkingViewBase : EntityView
    {
        // Runtime
        private ReactiveVariable<bool> _isMoving = null;
        private IDisposable _isMovingChangedDisposable = null;

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _isMovingChangedDisposable.Dispose();
        }

        protected override void OnEntityInitialized(Entity entity)
        {
            _isMoving = entity.IsMoving;

            _isMovingChangedDisposable = _isMoving.Subscribe(OnIsMovingChanged);

            UpdateIsMoving(_isMoving.Value);
        }

        private void OnIsMovingChanged(bool oldValue, bool isMoving)
            => UpdateIsMoving(_isMoving.Value);

        protected virtual void UpdateIsMoving(bool isMoving)
        { }
    }
}