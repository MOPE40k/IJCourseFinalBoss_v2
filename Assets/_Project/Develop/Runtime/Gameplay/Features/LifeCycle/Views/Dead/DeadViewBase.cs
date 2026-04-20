using System;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.LifeCycle.Views.Dead
{
    public class DeadViewBase : EntityView
    {
        // Runtime
        private IReadOnlyVariable<bool> _isDead = null;
        private IDisposable _isDeadChangedDisposable = null;

        protected override void OnEntityInitialized(Entity entity)
        {
            _isDead = entity.IsDead;

            _isDeadChangedDisposable = _isDead.Subscribe(OnIsDeadChanged);

            UpdateIsDead(_isDead.Value);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _isDeadChangedDisposable.Dispose();
        }

        private void OnIsDeadChanged(bool oldValue, bool isDead)
            => UpdateIsDead(isDead);

        protected virtual void UpdateIsDead(bool isDead)
        { }
    }
}