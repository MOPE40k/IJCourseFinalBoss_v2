using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore;
using Assets._Project.Develop.Runtime.Gameplay.EntitiesCore.Mono;
using Assets._Project.Develop.Runtime.Utilities.Reactive;
using System;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.ApplyDamage.Views
{
    public class ApplyDamageViewBase : EntityView
    {
        // Runtime
        private ReactiveEvent<float> _takeDamageEvent = null;
        private IDisposable _takeDamageDisposable = null;

        protected override void OnEntityInitialized(Entity entity)
        {
            _takeDamageEvent = entity.TakeDamageEvent;

            _takeDamageDisposable = _takeDamageEvent.Subscribe(OnTakeDamage);
        }

        public override void Cleanup(Entity entity)
        {
            base.Cleanup(entity);

            _takeDamageDisposable.Dispose();
        }

        protected virtual void OnTakeDamage(float damage)
        { }
    }
}
